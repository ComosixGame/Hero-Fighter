using UnityEngine;

public class AroundAttack : PlayerSkill
{
    [SerializeField] private PlayerHurtBox playerHurtBox;
    override protected void Awake()
    {
        base.Awake();
        currentLevel = skillState.level;
    }

    // Start is called before the first frame update
    void Start()
    {
        energy = skillLevels[currentLevel].energy;
        maxCoolDownTime = skillLevels[currentLevel].maxCoolDownTime;
        playerHurtBox.damage = skillLevels[currentLevel].damage;
    }
}
