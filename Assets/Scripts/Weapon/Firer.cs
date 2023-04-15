using UnityEngine;

public class Firer : EffectObjectPool
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

    private void FixedUpdate()
    {
        if (particle.IsAlive() && !explored)
        {
            explored = true;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f, layerTarget);
            if (hitColliders.Length > 0)
            {
                foreach (Collider collider in hitColliders)
                {
                    if ((layerTarget & (1 << collider.gameObject.layer)) != 0)
                    {
                        if (collider.TryGetComponent(out IDamageable damageable))
                        {
                            Vector3 dirAttack = transform.position - collider.transform.position;
                            damageable.TakeDamgae(dirAttack.normalized, damage * 2, AttackType.heavy);
                        }
                    }
                }
            }
        }
    }

    private void OnDisable() {
        explored = false;
    }

}
