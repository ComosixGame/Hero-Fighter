using UnityEngine;

public class FollowMissile : AbsPlayerSkill
{
    [SerializeField] private Vector3 missilePosition;
    [SerializeField] private EffectObjectPool missileObject;
    private ParticleSystem generatedLaserObject;
    private ObjectPoolerManager objectPooler;

    override protected void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        energy = skillLevels[currentLevel].energy;
        maxCoolDownTime = skillLevels[currentLevel].maxCoolDownTime;
    }

    public void CastMissile()
    {
        generatedLaserObject = objectPooler.SpawnObject(
                missileObject,
                transform.TransformPoint(missilePosition),
                transform.rotation
            ).GetComponent<ParticleSystem>();
        generatedLaserObject.Play();
    }

    public void CastDone()
    {
        generatedLaserObject.Stop();
    }
}
