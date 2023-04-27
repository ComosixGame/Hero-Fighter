using UnityEngine;

public class LaserCannon : PlayerSkill
{
    [SerializeField] private Vector3 laserPosition;
    [SerializeField] private EffectObjectPool laserObject;
    [SerializeField] private int charaterId;
    [SerializeField] private int skillId;
    private bool casting;
    private ParticleSystem generatedLaserObject;
    private ObjectPoolerManager objectPooler;
    private PlayerData playerData;

    override protected void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
        playerData = PlayerData.Load();
        currentLevel = playerData.characters[charaterId].levelSkills[skillId];
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
