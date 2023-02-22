using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public LayerMask targetLayer;
    public EnemyBehaviour enemyBehaviour;
    private void OnTriggerEnter(Collider other)
    {
        if (enemyBehaviour.attacking && !enemyBehaviour.inTakeDamage)
        {
            if ((targetLayer & (1 << other.gameObject.layer)) != 0)
            {
                if(other.TryGetComponent(out IDamageable damageable)) {
                    Vector3 hitPoint = other.GetComponent<Collider>().ClosestPoint(transform.position);
                    damageable.TakeDamgae(hitPoint, enemyBehaviour.damage);
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
