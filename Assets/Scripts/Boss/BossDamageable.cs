using System;
using UnityEngine;
using UnityEngine.AI;
using MyCustomAttribute;


public class BossDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxStun;
    [SerializeField, ReadOnly] private float health;
    [SerializeField, ReadOnly] private float stunLevel;
    private bool stunning, destroyed;
    private int dizzyHash, deadHash;
    private BossBehaviour bossBehaviour;
    private Collider colliderObject;
    private NavMeshAgent agent;
    private Animator animator;
    public event Action<float> OnTakeDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bossBehaviour = GetComponent<BossBehaviour>();
        colliderObject = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();

        dizzyHash = Animator.StringToHash("Dizzy");
        deadHash = Animator.StringToHash("Dead");

        health = maxHealth;
    }

    private void OnEnable()
    {
        destroyed = false;
        bossBehaviour.enabled = true;
        colliderObject.enabled = true;
    }

    public void TakeDamgae(Vector3 dirAttack, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            if (!stunning)
            {
                stunLevel += damage;
                if (stunLevel >= maxStun)
                {
                    Dizzy();
                }
            }
            else
            {
                health -= damage;
            }

            if (health <= 0)
            {
                Destroy();
            }

        }


        OnTakeDamage?.Invoke(health);
    }

    private void Dizzy()
    {
        stunLevel = 0;
        stunning = true;
        if (agent.isOnNavMesh)
            agent.ResetPath();
        bossBehaviour.enabled = false;
        animator.SetBool(dizzyHash, true);
        Invoke("CancelStun", 5f);
    }

    private void CancelStun()
    {
        stunning = false;
        animator.SetBool(dizzyHash, false);
        bossBehaviour.enabled = true;
    }

    private void Destroy()
    {
        Time.timeScale = 0.3f;
        destroyed = true;
        CancelInvoke("CancelStun");
        CancelStun();
        if (agent.isOnNavMesh)
            agent.ResetPath();
        bossBehaviour.enabled = false;
        colliderObject.enabled = false;
        animator.SetTrigger(deadHash);
        Invoke("CancelSlowMotion", 0.3f);
    }

    private void CancelSlowMotion()
    {
        Time.timeScale = 1;
    }

    public void KnockDownEffect()
    {
        throw new NotImplementedException();
    }
}
