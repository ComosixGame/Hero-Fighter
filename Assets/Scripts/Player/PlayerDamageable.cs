using System;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private AnimationCurve knockBackCuvre;
    [SerializeField] private float standupTime;
    [SerializeField] private float maxHealth;
    private float health, knockTimer;
    private int hitHash, knockHash, standupHash, deathHash;
    public bool knock { get; private set; }
    private bool knocking, destroyed;
    private Animator animator;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        hitHash = Animator.StringToHash("hit");
        knockHash = Animator.StringToHash("knock");
        standupHash = Animator.StringToHash("standup");
        deathHash = Animator.StringToHash("death");
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
    }

    public void TakeDamgae(Vector3 hitPoint, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            health -= damage;
            if (health <= 0)
            {
                Destroy(attackType);
            }
            if (attackType == AttackType.light)
            {
                animator.SetTrigger(hitHash);
            }
            else
            {
                knock = true;
                knocking = true;
                animator.SetTrigger(knockHash);
            }

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

    public void StandUpDone()
    {
        knock = false;
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
            knock = true;
            knocking = true;
            animator.SetTrigger(knockHash);
        }
    }
}
