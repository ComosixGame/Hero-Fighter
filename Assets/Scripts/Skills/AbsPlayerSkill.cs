using System;
using UnityEngine;
using System.Collections;

public enum skillHolder
{
    skill1,
    skill2,
    skill3,
    skill4
}

public abstract class AbsPlayerSkill : MonoBehaviour
{
    public string skillName;
    protected int PlayerSkillHash;
    protected int PlayerSkill1Hash;
    protected int PlayerSkill2Hash;
    protected int PlayerSkill3Hash;
    protected int PlayerSkill4Hash;
    protected Animator animator;
    public event Action OnStart;
    public event Action OnDone;
    public skillHolder skillHolder;
    
    [SerializeField] protected float maxCoolDownTime;
    [SerializeField] protected SkillLevel[] skillLevels = new SkillLevel[5];
    public int energy;
    public float cooldownTimer = 0;
    protected SkillSystem skillSystem;
    protected bool ready, coolDown;
    protected int currentLevel;


    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        PlayerSkillHash = Animator.StringToHash("PlayerSkill");
        PlayerSkill1Hash = Animator.StringToHash("PlayerSkill1");
        PlayerSkill2Hash = Animator.StringToHash("PlayerSkill2");
        PlayerSkill3Hash = Animator.StringToHash("PlayerSkill3");
        PlayerSkill4Hash = Animator.StringToHash("PlayerSkill4");
    }

    // Update is called once per frame
    void Update()
    {   
        if ((this.energy >= skillLevels[currentLevel].energy) && (cooldownTimer <=0))
        {   
            ready = true;
        }
    }

    protected void Action(skillHolder skillHolder)
    {
        switch(skillHolder)
        {
            case skillHolder.skill1:
                    animator.SetTrigger(PlayerSkill1Hash);
                    break;
            case skillHolder.skill2:
                    animator.SetTrigger(PlayerSkill2Hash);
                    break;
            case skillHolder.skill3:
                    animator.SetTrigger(PlayerSkill3Hash);
                    break;
            case skillHolder.skill4:
                    animator.SetTrigger(PlayerSkill4Hash);
                    break;
            default:
                    throw new InvalidOperationException("invalid skill skillHolder");
        }
    }

    public void UpgradeLevelSkill(int level)
    {
        if(currentLevel <= 5)
        {
            currentLevel=level;  
        }
    }

    public bool Cast(skillHolder skillHolder)
    {
        Action(skillHolder);
        OnStart?.Invoke();
        StartCoroutine(CoolDownCoroutine());
        return true;
    }

    private IEnumerator CoolDownCoroutine()
    {
        SkillSystem.currentEnergy -= skillLevels[currentLevel].energy;
        cooldownTimer = maxCoolDownTime;
        while(cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }

        cooldownTimer = 0;
    }

    public void Done()
    {
        OnDone?.Invoke();
    }

    [Serializable]
    protected class SkillLevel
    {
        public int energy;
        public int maxCoolDownTime;
        public float damage;
    }
}

