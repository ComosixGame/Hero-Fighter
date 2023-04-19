using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerHurtBox : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private AttackType attackType;
    public float damage;
    [SerializeField] private int bonusEnergy;
    [SerializeField] private EffectObjectPool hitEffect;

    private ObjectPoolerManager objectPooler;
    private SoundManager soundManager;
    public AudioClip hit;
    public static event Action<int> OnHit;
    private SkillSystem skillSystem;


    private void Awake()
    {
        objectPooler = ObjectPoolerManager.Instance;
        soundManager = SoundManager.Instance;
        skillSystem = GetComponentInParent<SkillSystem>();
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
                skillSystem.energyBarPlayer.UpdateEnergyBarValue(bonusEnergy);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(255, 0, 0, 80);
        Collider collider = GetComponent<Collider>();
        Type colliderType = collider.GetType();
        if (colliderType == typeof(BoxCollider)) {
            BoxCollider box = collider as BoxCollider;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.center, box.size);
        } else if(colliderType == typeof(SphereCollider)) {
            SphereCollider sphere = collider as SphereCollider;
            Gizmos.DrawSphere(transform.TransformPoint(sphere.center), sphere.radius);
        }
    }
#endif
}
