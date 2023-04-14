using System;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private AttackType attackType;
    [SerializeField] private float damage;
    [SerializeField] private int bonusEnergy;
    [SerializeField] private EffectObjectPool hitEffect;
    private ObjectPoolerManager objectPooler;
    public static event Action<int> OnHit;

    private SoundManager soundManager;
    public AudioClip hit;


    private void Awake() {
        objectPooler = ObjectPoolerManager.Instance;
        soundManager = SoundManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                Vector3 hitPoint = other.GetComponent<Collider>().ClosestPoint(transform.position);
                objectPooler.SpawnObject(hitEffect, hitPoint, Quaternion.identity);
                Vector3 dirAttack = transform.position - other.transform.position;
                damageable.TakeDamgae(dirAttack.normalized, damage, attackType);
                OnHit?.Invoke(bonusEnergy);
                soundManager.PlaySound(hit);
            }
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
