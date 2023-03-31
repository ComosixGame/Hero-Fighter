using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float health;
    public bool destroyed;
    private int hitHash;
    private int knockHash;
    private int deathHash;
    private int deathKnockHash;
    private Animator animator;
    private EnemyBehaviour enemyBehaviour;
    private Collider colliderGameObject;
    private GameManager gameManager;
    [SerializeField] private HealthBarRennder healthBarRennder = new HealthBarRennder();
    private UIMenu uI;

    [Header("VFX")]
    [SerializeField] private EffectObjectPool hitEffect;
    [SerializeField] private EffectObjectPool knockDownVFX;

    private ObjectPoolerManager objectPoolerManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        colliderGameObject = GetComponent<Collider>();

        hitHash = Animator.StringToHash("Hit");
        knockHash = Animator.StringToHash("Knock");
        deathHash = Animator.StringToHash("death");
        deathKnockHash = Animator.StringToHash("deathKnock");
        objectPoolerManager = ObjectPoolerManager.Instance;
        healthBarRennder.CreateHealthBar(transform, maxHealth);
    }

    private void OnEnable()
    {
        uI = FindObjectOfType<UIMenu>();
        enemyBehaviour.enabled = true;
        colliderGameObject.enabled = true;
        destroyed = false;
        health = maxHealth;
        healthBarRennder.UpdateHealthBarValue(health);
    }

    private void FixedUpdate()
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        if (!animationState.IsName("KnockDown"))
        {
            animator.ResetTrigger(knockHash);
        }

        if (!animationState.IsName("Hit"))
        {
            animator.ResetTrigger(hitHash);
        }
    }

    private void LateUpdate()
    {
        healthBarRennder.UpdateHealthBarRotation();
    }

    public void TakeDamgae(Vector3 hitPoint, Vector3 dirAttack, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            uI?.DisplayHitPoint(true);
            AttackVFX(hitPoint);
            health -= damage;
            healthBarRennder.UpdateHealthBarValue(health);
            if (health <= 0)
            {
                destroyed = true;
                Destroy(attackType);
                return;
            }
            HandleHitReaction(hitPoint, attackType);
        }
    }

    private void HandleHitReaction(Vector3 dirAttack, AttackType attackType)
    {
        transform.rotation = Ultils.GetRotationLook(dirAttack, transform.forward);
        if (attackType == AttackType.light)
        {
            animator.SetTrigger(hitHash);
        }
        else
        {
            animator.SetTrigger(knockHash);
        }
    }

    private void AttackVFX(Vector3 pos)
    {
        objectPoolerManager.SpawnObject(hitEffect, pos, Quaternion.identity);
    }

    private void Destroy(AttackType attackType)
    {
        if (attackType == AttackType.light)
        {
            animator.SetTrigger(deathHash);
        }
        else
        {
            animator.SetTrigger(deathKnockHash);
        }

        enemyBehaviour.enabled = false;
        colliderGameObject.enabled = false;
        healthBarRennder.DestroyHeathBar();
        destroyed = true;
    }

    public void KnockDownEffect()
    {
        objectPoolerManager.SpawnObject(knockDownVFX, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(5, .1f);
    }
}
