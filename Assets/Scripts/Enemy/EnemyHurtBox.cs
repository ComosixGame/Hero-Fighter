using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyHurtBox : MonoBehaviour
{
    public LayerMask targetLayer;
    [SerializeField] private float damage;
    [SerializeField] private AttackType attackType;
    [SerializeField] private EffectObjectPool hitEffect;
    private ObjectPoolerManager objectPooler;
    private EnemyBehaviour enemyBehaviour;

    private void Awake()
    {
        objectPooler = ObjectPoolerManager.Instance;
        enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayer & (1 << other.gameObject.layer)) != 0)
        {
            IDamageable damageable = other.GetComponentInParent<IDamageable>();
            Vector3 hitPoint = other.GetComponent<Collider>().ClosestPoint(transform.position);
            if(hitEffect != null) {
                objectPooler.SpawnObject(hitEffect, hitPoint, Quaternion.identity);
            }
            Vector3 dirAttack = transform.position - other.transform.position;
            damageable?.TakeDamgae(dirAttack.normalized, damage, attackType);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(255, 0, 0, 80);
        BoxCollider box = GetComponent<BoxCollider>();
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(box.center, box.size);
    }
#endif
}
