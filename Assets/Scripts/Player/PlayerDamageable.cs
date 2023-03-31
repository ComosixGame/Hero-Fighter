using System;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float health;
    private int hitHash, knockHash, deathHash, dyingHash, revivalHash;
    private bool destroyed;
    private Animator animator;
    private HealthBarPlayer healthBarPlayer;
    private GameManager gameManager;
    public event Action<Vector3, float, AttackType> OnTakeDamageStart;
    public event Action OnTakeDamageEnd;
    private UIMenu ui;
    [Header("VFX")]
    [SerializeField] private EffectObjectPool hitEffect;
    [SerializeField] private EffectObjectPool knockDownVFX;
    private ObjectPoolerManager objectPoolerManager;
    private PlayerController playerController;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        hitHash = Animator.StringToHash("hit");
        knockHash = Animator.StringToHash("knock");
        deathHash = Animator.StringToHash("death");
        dyingHash = Animator.StringToHash("dying");
        revivalHash = Animator.StringToHash("revival");
        healthBarPlayer = FindObjectOfType<HealthBarPlayer>();
        ui = FindObjectOfType<UIMenu>();
        objectPoolerManager = ObjectPoolerManager.Instance;
        playerController = GetComponent<PlayerController>();
    }


    private void Start()
    {
        health = maxHealth;
        healthBarPlayer?.CreateHealthBar(maxHealth);
    }

    public void TakeDamgae(Vector3 hitPoint, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            health -= damage;

            objectPoolerManager.SpawnObject(hitEffect, hitPoint, Quaternion.identity);
            ui?.DisplayHitPoint(false);
            healthBarPlayer?.UpdateHealthBarValue(health);

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
                Vector3 dirAttack = hitPoint - transform.position;
                transform.rotation = Ultils.GetRotationLook(dirAttack, transform.forward);
                animator.SetTrigger(knockHash);
            }

            OnTakeDamageStart?.Invoke(hitPoint, damage, attackType);


        }
    }

    private void Destroy(AttackType attackType)
    {
        destroyed = true;
        if (attackType == AttackType.light)
        {
            animator.SetTrigger(deathHash);
        }
        else
        {
            animator.SetTrigger(knockHash);
            animator.SetBool(dyingHash, true);
        }
        gameManager.GameLose();
    }

    public void Revival()
    {
        gameObject.SetActive(false);
        animator.SetTrigger(revivalHash);
        playerController.isStart = true;
        GameObject newPlayer = Instantiate(gameObject, transform.position, transform.rotation);
        gameManager.player = newPlayer.transform;
        newPlayer.SetActive(true);
        destroyed = false;
        gameManager.virtualCamera.Follow = newPlayer.transform;
    }

    public void KnockDownEffect()
    {
        CinemachineShake.Instance.ShakeCamera(5, .2f);
        objectPoolerManager.SpawnObject(knockDownVFX, transform.position, Quaternion.identity);
    }
}
