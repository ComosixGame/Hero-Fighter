using UnityEngine;

public class FollowMissile : PlayerSkill
{
    [SerializeField] private Vector3 missilePosition;
    [SerializeField] private GameObjectPool missileObjectPool;
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
        Debug.Log(currentLevel);
    }


    public void CastMissile()
    {
        gameObjectPool = objectPooler.SpawnObject(missileObjectPool, transform.TransformPoint(missilePosition)
        , missileObjectPool.transform.rotation);
    }
}