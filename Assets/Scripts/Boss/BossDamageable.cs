using System;
using UnityEngine;
using UnityEngine.AI;

public class BossDamageable : MonoBehaviour, IDamageable
{
    public float maxHealth;
    [SerializeField] private float health;
    private bool destroyed;
    private BossBehaviour bossBehaviour;
    private Collider colliderObject;
    private NavMeshAgent agent;
    public event Action<float> OnTakeDamage;

    private void Awake() {
        bossBehaviour = GetComponent<BossBehaviour>();
        colliderObject = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        health = maxHealth;
    }

    public void TakeDamgae(Vector3 hitPoint, float damage, AttackType attackType)
    {
        health -= damage;
        if(health <= 0 && !destroyed) {
            if(!destroyed) {
                Destroy();
            }
        }

        OnTakeDamage?.Invoke(health);
    }

    private void Destroy() {
        bossBehaviour.enabled = false;
        colliderObject.enabled = false;
        agent.ResetPath();
        destroyed = true;
    }

}
