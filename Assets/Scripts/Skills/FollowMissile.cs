using UnityEngine;

public class FollowMissile : AbsPlayerSkill
{
    [SerializeField] private Vector3 missilePosition;
    [SerializeField] private EffectObjectPool missileObject;
    [SerializeField] private GameObjectPool missileObjectPool;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed = 95;
    private ParticleSystem generatedMissileObject;
    private ObjectPoolerManager objectPooler;
    private Transform target;
    private GameObjectPool gameObjectPool;
    private bool flag;
    private Rigidbody rb;

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
        target = FindObjectOfType<EnemyDamageable>().transform;
        
    }

    void FixedUpdate() {
        if (flag)
        {
            rb = gameObjectPool.GetComponent<Rigidbody>();
            rb.velocity = transform.forward*speed;

        } 
        Debug.Log(target);
            
    }

    

    public void CastMissile()
    {
        // generatedMissileObject = objectPooler.SpawnObject(
        //         missileObject,
        //         transform.TransformPoint(missilePosition),
        //         transform.rotation
        //     ).GetComponent<ParticleSystem>();
        // generatedMissileObject.Play();
        gameObjectPool = objectPooler.SpawnObject(missileObjectPool, transform.TransformPoint(missilePosition) , missileObjectPool.transform.rotation);
        flag = true;
    }

    public void CastDone()
    {
        generatedMissileObject.Stop();
    }
}
