using UnityEngine;

public class AroundAttack : AbsPlayerSkill
{
    [SerializeField] private Vector3 aroundAttackPosition;
    [SerializeField] private EffectObjectPool aroundAttackObject;
    [SerializeField] private SkillState skillState;
    private ParticleSystem generatedAroundAttackObject;
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

    public void CastAroundAttack()
    {
        generatedAroundAttackObject = objectPooler.SpawnObject(
                aroundAttackObject,
                transform.TransformPoint(aroundAttackPosition),
                transform.rotation
            ).GetComponent<ParticleSystem>();
        generatedAroundAttackObject.Play();
    }
}
