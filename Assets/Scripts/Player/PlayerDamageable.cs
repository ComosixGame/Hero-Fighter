using System;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private AnimationCurve knockBackCuvre;
    [SerializeField] private float standupTime;
    [SerializeField] private float maxHealth;
    private float health, knockTimer;
    private int hitHash, knockHash, standupHash, deathHash;
    [SerializeField] private bool knocking;
    private bool destroyed;
    private Animator animator;
    private CharacterController controller;
    private HealthBarPlayer healthBarPlayer;
    private GameManager gameManager;
    public event Action<Vector3, float, AttackType> OnTakeDamageStart;
    public event Action OnTakeDamageEnd;

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
        healthBarPlayer = FindObjectOfType<HealthBarPlayer>();
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
        healthBarPlayer.CreateHealthBar(maxHealth);
        health = maxHealth;
    }

    public void TakeDamgae(Vector3 hitPoint, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            health -= damage;
            healthBarPlayer.UpdateHealthBarValue(health);
            if (health <= 0)
            {
                Destroy(attackType);
                return;
            }
            if(!knocking) {
                if (attackType == AttackType.light)
                {
                    animator.SetTrigger(hitHash);
                }
                else
                {
                    knocking = true;
                    animator.SetTrigger(knockHash);
                }
            }
            OnTakeDamageStart?.Invoke(hitPoint, damage, attackType);

        }
    }

    public void KnockDone()
    {
        if (!destroyed)
        {
            knocking = false;
            knockTimer = 0;
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
        gameManager.GameLose();
        destroyed = true;
        if (attackType == AttackType.light)
        {
            animator.SetTrigger(deathHash);
        }
        else
        {
            knocking = true;
            animator.SetTrigger(knockHash);
        }
    }
}
