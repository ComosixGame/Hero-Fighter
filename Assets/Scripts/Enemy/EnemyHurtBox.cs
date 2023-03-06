using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{
    public LayerMask targetLayer;
    [SerializeField] private float damage;
    [SerializeField] private AttackType attackType;
    private void OnTriggerEnter(Collider other)
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(255, 0, 0, 80);
        Vector3 size = GetComponent<BoxCollider>().size;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, size);
    }
#endif
}
