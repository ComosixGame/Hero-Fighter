using UnityEngine;
using MyCustomAttribute;

public class EnemyAttack : MonoBehaviour
{
    public LayerMask targetLayer;
    [SerializeField] private float damage;
    [SerializeField] private AttackType attackType;
    [ReadOnly] public EnemyBehaviour enemyBehaviour;
    private void OnTriggerEnter(Collider other)
    {
        if (enemyBehaviour.attacking && !enemyBehaviour.inTakeDamage)
        {
            if ((targetLayer & (1 << other.gameObject.layer)) != 0)
            {
                IDamageable damageable = other.GetComponentInParent<IDamageable>();
                if(damageable != null) {
                    Vector3 hitPoint = other.GetComponent<Collider>().ClosestPoint(transform.position);
                    damageable.TakeDamgae(hitPoint, damage, attackType);
                }
            }

        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(255, 0, 0, 120);
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
#endif
}
