using UnityEngine;

public class Eyeslaser : AbsSpecialSkill
{
    [SerializeField] private GameObject laserPosition;
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
        generatedLaserObject = Instantiate(laserObject, laserPosition.transform.position, laserPosition.transform.rotation);
        generatedLaserObject.Play();
    }

    public void CastLaserDone()
    {
        generatedLaserObject.Stop();
    }


}
