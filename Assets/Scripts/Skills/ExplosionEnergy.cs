using UnityEngine;

public class ExplosionEnergy : AbsPlayerSkill
{
    [SerializeField] private Vector3 explosionPosition;
    [SerializeField] private EffectObjectPool explosionForceObject;
    [SerializeField] private EffectObjectPool explosionObject;
    private ParticleSystem generatedExplosionObject;
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
        // laserObject.GetComponent<Laser>().damage = skillLevels[currentLevel].damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastExplosion()
    {
        generatedExplosionObject = objectPooler.SpawnObject(
                explosionObject,
                transform.TransformPoint(explosionPosition),
                transform.rotation
            ).GetComponent<ParticleSystem>();
        generatedExplosionObject.Play();
    }

    public void CastExplosionForce()
    {
        generatedExplosionObject = objectPooler.SpawnObject(
                explosionForceObject,
                transform.TransformPoint(explosionPosition),
                transform.rotation
            ).GetComponent<ParticleSystem>();
        generatedExplosionObject.Play();
    }
}
