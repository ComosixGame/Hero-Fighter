using UnityEngine;

public class FlameThrower : AbsPlayerSkill
{
    [SerializeField] private Vector3 flamePosition;
    [SerializeField] private EffectObjectPool flameObject;
    private ParticleSystem generatedFlameObject;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastFlameThrower()
    {
        generatedFlameObject = objectPooler.SpawnObject(
                flameObject,
                transform.TransformPoint(flamePosition),
                transform.rotation
            ).GetComponent<ParticleSystem>();
        generatedFlameObject.Play();
    }

    public void CastFlameThrowerDone()
    {
        generatedFlameObject.Stop();
    }
}
