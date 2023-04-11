using UnityEngine;

public class Eyeslaser : AbsSpecialSkill
{
    [SerializeField] private Vector3 laserPosition;
    [SerializeField] private EffectObjectPool laserObject;
    [SerializeField] private float damage;
    private bool casting;
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

    private void LateUpdate() {
        if(casting) {
            AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
            if(!animationState.IsName("SpecialSkill")) {
                CastLaserDone();
            }
        }
    }

    protected override void Action()
    {
        animator.SetTrigger(SpecialSkilHash);
    }

    public void CastLaser()
    {
        casting =  true;
        generatedLaserObject = objectPooler.SpawnObject(
                laserObject,
                transform.TransformPoint(laserPosition),
                transform.rotation
            ).GetComponent<ParticleSystem>();
        generatedLaserObject.Play();
    }

    public void CastLaserDone()
    {
        casting = false;
        generatedLaserObject.Stop();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.TransformPoint(laserPosition), 0.1f);
    }


}
