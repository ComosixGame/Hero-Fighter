using UnityEngine;

public class Explosion : EffectObjectPool
{
    [SerializeField] private LayerMask layerTarget;
    [SerializeField] private AttackType attackType;
    public float damage;
    private bool explored;

    private void OnParticleCollision(GameObject other)
    {
        if ((layerTarget & (1 << other.layer)) != 0)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                Vector3 dirAttack = transform.position - other.transform.position;
                damageable.TakeDamgae(dirAttack.normalized, damage, attackType);
            }
        }
    }
}
