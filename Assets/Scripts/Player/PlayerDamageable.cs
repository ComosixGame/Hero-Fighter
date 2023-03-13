using System;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private AnimationCurve knockBackCuvre;
    [SerializeField] private float standupTime;
    [SerializeField] private float maxHealth;
    private float health, knockTimer;
    private int hitHash, knockHash, standupHash, deathHash, dyingHash;
    [SerializeField] private bool knocking;
    private bool destroyed;
    private Animator animator;
    private CharacterController controller;
    private HealthBarPlayer healthBarPlayer;
    private GameManager gameManager;
    public event Action<Vector3, float, AttackType> OnTakeDamageStart;
    public event Action OnTakeDamageEnd;
    private UIMenu ui;
    [Header("VFX")]
    [SerializeField] private GameObjectPool attackVFX;
    private ObjectPoolerManager ObjectPoolerManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        gameManager.SetPlayer(transform);
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        hitHash = Animator.StringToHash("hit");
        knockHash = Animator.StringToHash("knock");
        standupHash = Animator.StringToHash("standup");
        deathHash = Animator.StringToHash("death");
        dyingHash = Animator.StringToHash("dying");
        healthBarPlayer = FindObjectOfType<HealthBarPlayer>();
        ui = FindObjectOfType<UIMenu>();
        ObjectPoolerManager = ObjectPoolerManager.Instance;
    }

    private void Update()
    {
        if (knocking)
        {
            knockTimer += Time.deltaTime;
            float speed = knockBackCuvre.Evaluate(knockTimer) * 10f;
            controller.SimpleMove(-transform.forward.normalized * speed);
        }
    }

    private void Start()
    {
        health = maxHealth;
        if (healthBarPlayer != null)
        {
            healthBarPlayer.CreateHealthBar(maxHealth);
        }
    }

    public void TakeDamgae(Vector3 hitPoint, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            health -= damage;

            ObjectPoolerManager.SpawnObject(attackVFX, hitPoint, Quaternion.identity);
            if (ui != null)
            {
                ui.DisplayHitPoint(false);
            }

            if (healthBarPlayer != null)
            {
                healthBarPlayer.UpdateHealthBarValue(health);
            }
            if (health <= 0)
            {
                Destroy(attackType);
                return;
            }
            if (!knocking)
            {
                if (attackType == AttackType.light)
                {
                    animator.SetTrigger(hitHash);
                }
                else
                {
                    Vector3 dirAttack = hitPoint - transform.position;
                    dirAttack.y = 0;
                    transform.rotation = Quaternion.LookRotation(dirAttack);
                    knocking = true;
                    animator.SetTrigger(knockHash);
                }
            }
            OnTakeDamageStart?.Invoke(hitPoint, damage, attackType);

        }
    }

    public void KnockDone()
    {
        knocking = false;
        knockTimer = 0;
        if (!destroyed)
        {
            Invoke("StandUp", standupTime);
        }
    }

    private void StandUp()
    {
        animator.SetTrigger(standupHash);
    }

    public void TakeDamagaEnd()
    {
        OnTakeDamageEnd?.Invoke();
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
            knocking = true;
            animator.SetTrigger(knockHash);
            animator.SetBool(dyingHash, true);
        }
        // gameManager.GameLose();
    }
}
