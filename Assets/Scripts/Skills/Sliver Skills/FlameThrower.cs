using UnityEngine;

public class FlameThrower : PlayerSkill
{
    [SerializeField] private Vector3 flamePosition;
    [SerializeField] private EffectObjectPool flameObject;
    [SerializeField] private int charaterId;
    [SerializeField] private int skillId;
    private ParticleSystem generatedFlameObject;
    private ObjectPoolerManager objectPooler;
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
        flameObject.GetComponent<Firer>().damage = skillLevels[currentLevel].damage;
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
