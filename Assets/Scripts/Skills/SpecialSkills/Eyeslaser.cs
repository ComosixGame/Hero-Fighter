using UnityEngine;

public class Eyeslaser : AbsSpecialSkill
{
    [SerializeField] private Vector3 laserPosition;
    [SerializeField] private ParticleSystem laserObject;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    private ParticleSystem generatedLaserObject;
    override protected void Awake() {
        base.Awake();
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
        generatedLaserObject = Instantiate(laserObject, transform.TransformPoint(laserPosition), transform.rotation);
        generatedLaserObject.Play();
    }

    public void CastLaserDone()
    {
        generatedLaserObject.Stop();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.TransformPoint(laserPosition), 0.1f);
    }


}
