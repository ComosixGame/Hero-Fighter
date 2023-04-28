using System;
using UnityEngine;
using MyCustomAttribute;
using System.Collections.Generic;
public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField, ReadOnly] private float health;
    private int hitHash, knockHash, deathHash, revivalHash, deathKnock;
    private bool destroyed;
    private Animator animator;
    private HealthBarPlayer healthBarPlayer;
    private GameManager gameManager;
    [Header("VFX")]
    [SerializeField] private EffectObjectPool knockDownVFX;
    private ObjectPoolerManager objectPoolerManager;
    private PlayerController playerController;
    private List<GameObject> newPlayers = new List<GameObject>();
    public static event Action<float> onTakeDamage;
    public static event Action<float> onInit;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        hitHash = Animator.StringToHash("hit");
        knockHash = Animator.StringToHash("knock");
        deathHash = Animator.StringToHash("death");
        revivalHash = Animator.StringToHash("Revival");
        deathKnock = Animator.StringToHash("deathKnock");
        objectPoolerManager = ObjectPoolerManager.Instance;
        playerController = GetComponent<PlayerController>();
        onInit?.Invoke(maxHealth);
    }


    private void Start()
    {
        health = maxHealth;
    }

    private void LateUpdate()
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        if(!animationState.IsName("Hit")) {
            animator.ResetTrigger(hitHash);
        }

        if(!animationState.IsName("knock")) {
            animator.ResetTrigger(knockHash);
        }
    }

    public void TakeDamgae(Vector3 dirAttack, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            animator.applyRootMotion = true;
            health -= damage;
            onTakeDamage?.Invoke(health);
            if (health <= 0)
            {
                Destroy(attackType);
                return;
            }

            if (attackType == AttackType.light)
            {
                animator.SetTrigger(hitHash);
            }
            else
            {
                transform.rotation = Ultils.GetRotationLook(dirAttack, transform.forward);
                animator.SetTrigger(knockHash);
            }
        }
    }

    private void Destroy(AttackType attackType)
    {
        destroyed = true;
        gameManager.playerDestroyed = destroyed;
        if (attackType == AttackType.light)
        {
            animator.SetTrigger(deathHash);
        }
        else
        {
            animator.SetTrigger(deathKnock);
        }
        gameManager.GameLose();
    }

    public void Revival()
    {
        foreach(GameObject pl in newPlayers)
        {
            pl.SetActive(false);
        }
        gameObject.SetActive(false);
        playerController.isStart = true;
        GameObject newPlayer = Instantiate(gameObject, transform.position, transform.rotation);
        newPlayers.Add(newPlayer);
        newPlayer.SetActive(true);
        animator.SetTrigger(revivalHash);
        destroyed = false;
        gameManager.player = newPlayer.transform;
        gameManager.playerDestroyed = destroyed;
        gameManager.virtualCamera.Follow = newPlayer.transform;
    }

    public void KnockDownEffect()
    {
        CinemachineShake.Instance.ShakeCamera(5, .2f);
        objectPoolerManager.SpawnObject(knockDownVFX, transform.position, Quaternion.identity);
    }
}
