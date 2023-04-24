using UnityEngine;

public class MountainSummon : PlayerSkill
{
    [SerializeField] private GameObjectPool mountainObjectPool;
    [SerializeField] private Vector3 mountainPosition;
    [SerializeField] private SkillState skillState;
    private ObjectPoolerManager objectPooler;
    private ParticleSystem generatedMountainObject;
    public float damage;

    override protected void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
        // currentLevel = skillState.level;
    }

    // Start is called before the first frame update
    void Start()
    {
        energy = skillLevels[currentLevel].energy;
        maxCoolDownTime = skillLevels[currentLevel].maxCoolDownTime;
    }

    public void CastMountainSummon()
    {
        generatedMountainObject = objectPooler.SpawnObject(
                mountainObjectPool,
                transform.TransformPoint(mountainPosition),
                transform.rotation
            ).GetComponent<ParticleSystem>();
        generatedMountainObject.Play();
    }
}
