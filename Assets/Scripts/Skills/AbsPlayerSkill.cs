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

public class AbsPlayerSkill : MonoBehaviour
{
    public string skillName;
    protected int playerSkillHash;
    protected Animator animator;
    public event Action OnStart;
    public event Action OnDone;
    public skillHolder skillHolder;
    public event Action<float> OnCooldownTimer;

    [SerializeField] protected float maxCoolDownTime;
    [SerializeField] protected SkillLevel[] skillLevels = new SkillLevel[6];
    public SkillState skillState;
    protected int energy;
    private float cooldownTimer = 0;
    protected SkillSystem skillSystem;
    protected bool ready = true, coolDown;
    protected int currentLevel;


    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        playerSkillHash = Animator.StringToHash("PlayerSkill");
    }

    private void Update() {
        switch (skillHolder)
        {
            case skillHolder.skill1:
                playerSkillHash = Animator.StringToHash("PlayerSkill1");
                break;
            case skillHolder.skill2:
                playerSkillHash = Animator.StringToHash("PlayerSkill2");
                break;
            case skillHolder.skill3:
                playerSkillHash = Animator.StringToHash("PlayerSkill3");
                break;
            default:
                playerSkillHash = Animator.StringToHash("PlayerSkill4");
                break;
        }
    }


    public bool Cast()
    {
        if (cooldownTimer == 0 && skillSystem.CheckEnergy(energy))
        {
            cooldownTimer = maxCoolDownTime;
            skillSystem.UpdateEnergy(-energy);
            animator.SetTrigger(playerSkillHash);
            OnStart?.Invoke();
            StartCoroutine(CoolDownCoroutine());
            return true;
        }
        return false;
    }

    private IEnumerator CoolDownCoroutine()
    {
        cooldownTimer = maxCoolDownTime;
        while (cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
            OnCooldownTimer?.Invoke(cooldownTimer);
            yield return new WaitForSeconds(0.1f);
        }
        ready = true;
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

