using System.Collections;
using UnityEngine;

public class ShardShooting : AbsMagic
{
    [SerializeField] private float effectTime;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float damageExplode;
    [SerializeField] private float radiusHurtBox;
    [SerializeField] private float radiusHurtBoxExplode;
    private float radius;
    private Vector3 postionHurtBox;

    private void OnEnable() {
        postionHurtBox = transform.position;
    }

    public override void Cast()
    {
        effect.Play();
        Shoot();
    }

    private void Shoot()
    {
        StartCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        float time = 0;
        radius = radiusHurtBox;
        while (time <= effectTime)
        {
            postionHurtBox += transform.forward.normalized * speed * Time.deltaTime;
            time += Time.deltaTime;
            Collider[] hitColliders = Physics.OverlapSphere(postionHurtBox, radius, targetLayer);
            if (hitColliders.Length > 0)
            {
                foreach (Collider collider in hitColliders)
                {
                    if (collider.TryGetComponent(out IDamageable damageable))
                    {
                        Vector3 dir = transform.position - collider.transform.position;
                        damageable.TakeDamgae(dir, damage, AttackType.light);
                    }

                }
            }
            yield return null;
        }
        
        radius = radiusHurtBoxExplode;
        Collider[] hitCollidersExplode = Physics.OverlapSphere(postionHurtBox, radius, targetLayer);
        if (hitCollidersExplode.Length > 0)
        {
            foreach (Collider collider in hitCollidersExplode)
            {
                if (collider.TryGetComponent(out IDamageable damageable))
                {
                    Vector3 dir = transform.position - collider.transform.position;
                    damageable.TakeDamgae(dir, damageExplode, AttackType.heavy);
                }

            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.7f);
        if (!Application.isPlaying)
        {
            Gizmos.DrawSphere(transform.TransformPoint(postionHurtBox), radiusHurtBox);
            Gizmos.DrawSphere(transform.position, radiusHurtBoxExplode);
        }
        else
        {
            Gizmos.DrawSphere(postionHurtBox, radius);
        }
    }
#endif
}
