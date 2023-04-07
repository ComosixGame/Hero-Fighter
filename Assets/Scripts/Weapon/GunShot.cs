using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class GunShot : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private AttackType attackType;
    [SerializeField] private float damage;
    [SerializeField] private EffectObjectPool hitEffect;
    private ObjectPoolerManager objectPooler;

    private void Awake() {
        objectPooler = ObjectPoolerManager.Instance;
    }

    private void OnParticleCollision(GameObject other)
    {
        if ((targetLayer & (1 << other.layer)) != 0)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                Vector3 hitPoint = other.GetComponent<Collider>().ClosestPoint(transform.position);
                objectPooler.SpawnObject(hitEffect, hitPoint, Quaternion.identity);
                damageable.TakeDamgae(transform.position - other.transform.position, damage, attackType);
            }
        }
    }
}
