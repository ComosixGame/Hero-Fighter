using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private AttackType attackType;
    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayer & (1 << other.gameObject.layer)) != 0)
        {
            if(other.TryGetComponent(out IDamageable damageable)) {
                Vector3 hitPoint = other.GetComponent<Collider>().ClosestPoint(transform.position);
                damageable.TakeDamgae(hitPoint, 0, attackType);
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
