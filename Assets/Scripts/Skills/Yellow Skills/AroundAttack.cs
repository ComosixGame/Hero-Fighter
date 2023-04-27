using UnityEngine;

public class AroundAttack : PlayerSkill
{
    [SerializeField] private PlayerHurtBox playerHurtBox;
    [SerializeField] private int charaterId;
    [SerializeField] private int skillId;
    private PlayerData playerData;
    override protected void Awake()
    {
        base.Awake();
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
}
