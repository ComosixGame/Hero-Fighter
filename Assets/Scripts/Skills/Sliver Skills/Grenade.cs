using UnityEngine;

public class Grenade : AbsPlayerSkill
{
    [SerializeField] private Vector3 grenadePosition;
    [SerializeField] private GameObjectPool grenadeObjectPool;
    [SerializeField] private SkillState skillState;
    private ObjectPoolerManager objectPooler;
    private GameObjectPool gameObjectPool;

    override protected void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
        currentLevel = skillState.level;
    }

    // Start is called before the first frame update
    void Start()
    {
        energy = skillLevels[currentLevel].energy;
        maxCoolDownTime = skillLevels[currentLevel].maxCoolDownTime;
    }

    public void CastGrenade()
    {
        gameObjectPool = objectPooler.SpawnObject(grenadeObjectPool, transform.TransformPoint(grenadePosition)
        ,grenadeObjectPool.transform.rotation);
    }
}
