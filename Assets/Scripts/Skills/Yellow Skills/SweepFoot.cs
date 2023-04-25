using UnityEngine;

public class SweepFoot : PlayerSkill
{
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
    }

}
