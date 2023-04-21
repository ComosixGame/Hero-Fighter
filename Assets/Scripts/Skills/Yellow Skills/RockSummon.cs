using UnityEngine;

public class RockSummon : AbsPlayerSkill
{
    [SerializeField] private GameObjectPool rockObjectPool;
    [SerializeField] private SkillState skillState;
    [SerializeField] private Transform target;
    private ObjectPoolerManager objectPooler;
    private ParticleSystem generatedRockObject;
    private GameManager gameManager;
    private bool isStart;
    public float damage;

    override protected void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
        objectPooler = ObjectPoolerManager.Instance;
        // currentLevel = skillState.level;
    }

    private void StartGame()
    {
        isStart = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        energy = skillLevels[currentLevel].energy;
        maxCoolDownTime = skillLevels[currentLevel].maxCoolDownTime;
    }

    public void CastRockSummon()
    {
        generatedRockObject = objectPooler.SpawnObject(
                rockObjectPool,
                target.transform.position,
                Quaternion.identity
            ).GetComponent<ParticleSystem>();
        generatedRockObject.Play();
    }
}
