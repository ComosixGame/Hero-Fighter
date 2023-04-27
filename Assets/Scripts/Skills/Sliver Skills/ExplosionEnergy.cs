using UnityEngine;

public class ExplosionEnergy : PlayerSkill
{
    [SerializeField] private Vector3 explosionPosition;
    [SerializeField] private EffectObjectPool explosionForceObject;
    [SerializeField] private EffectObjectPool explosionObject;
    [SerializeField] private PlayerHurtBox playerHurtBox;
    private ParticleSystem generatedExplosionObject;
    private ObjectPoolerManager objectPooler;
    [SerializeField] private int charaterId;
    [SerializeField] private int skillId;
    private PlayerData playerData;
    override protected void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
        playerData = PlayerData.Load();
        currentLevel = playerData.characters[charaterId].levelSkills[skillId];
    }

    // Start is called before the first frame update
    void Start()
    {
        energy = skillLevels[currentLevel].energy;
        maxCoolDownTime = skillLevels[currentLevel].maxCoolDownTime;
        playerHurtBox.damage = skillLevels[currentLevel].damage;
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
