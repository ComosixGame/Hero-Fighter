using UnityEngine;

public class Eyeslaser : AbsSpecialSkill
{
    [SerializeField] private Vector3 laserPosition;
    [SerializeField] private EffectObjectPool laserObject;
    [SerializeField] private float damage;
    private ParticleSystem generatedLaserObject;
    private ObjectPoolerManager objectPooler;

    override protected void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
    }

    private void OnEnable()
    {
        PlayerHurtBox.OnHit += AccumulationEnergy;
    }

    protected override void Action()
    {
        animator.SetTrigger(SpecialSkilHash);
    }

    public void CastLaser()
    {
        generatedLaserObject = objectPooler.SpawnObject(
                laserObject,
                transform.TransformPoint(laserPosition),
                transform.rotation
            ).GetComponent<ParticleSystem>();
        generatedLaserObject.Play();
    }

    public void CastLaserDone()
    {
        generatedLaserObject.Stop();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.TransformPoint(laserPosition), 0.1f);
    }


}
