using UnityEngine;

public class SweepFoot : AbsPlayerSkill
{
    [SerializeField] private Vector3 sweepFootPosition;
    [SerializeField] private EffectObjectPool sweepFootObject;
    [SerializeField] private SkillState skillState;
    private ParticleSystem generatedSweepFootObject;
    private ObjectPoolerManager objectPooler;

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

    public void CastSweepFoot()
    {
        generatedSweepFootObject = objectPooler.SpawnObject(
                sweepFootObject,
                transform.TransformPoint(sweepFootPosition),
                transform.rotation
            ).GetComponent<ParticleSystem>();
        generatedSweepFootObject.Play();
    }

}
