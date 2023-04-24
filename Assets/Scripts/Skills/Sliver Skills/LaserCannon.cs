using UnityEngine;

public class LaserCannon : PlayerSkill
{
    [SerializeField] private Vector3 laserPosition;
    [SerializeField] private EffectObjectPool laserObject;
    private bool casting;
    private ParticleSystem generatedLaserObject;
    private ObjectPoolerManager objectPooler;

    override protected void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
        currentLevel = skillState.level;
    }

    private void Start() {
        energy = skillLevels[currentLevel].energy;
        maxCoolDownTime = skillLevels[currentLevel].maxCoolDownTime;
        laserObject.GetComponent<Laser>().damage = skillLevels[currentLevel].damage;
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
