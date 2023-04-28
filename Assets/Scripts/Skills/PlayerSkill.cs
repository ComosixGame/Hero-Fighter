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

public class PlayerSkill : MonoBehaviour
{
    public int id;
    public string skillName;
    protected int playerSkillHash;
    protected Animator animator;
    public event Action OnStart;
    public event Action OnDone;
    public skillHolder skillHolder;
    public event Action<float> OnCooldownTimer;
    public event Action OnCooldownTimerDone;
    public event Action OnNotEnoughEnergy;
    public event Action OnEnoughEnergy;

    [SerializeField] protected float maxCoolDownTime;
    [SerializeField] protected SkillLevel[] skillLevels = new SkillLevel[6];
    public SkillState skillState;
    protected int energy;
    private float cooldownTimer = 0;
    protected SkillSystem skillSystem;
    protected bool ready = true, coolDown;
    protected int currentLevel;
    private string animationStateName;
    private static bool running;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        skillSystem = GetComponent<SkillSystem>();
        playerSkillHash = Animator.StringToHash("PlayerSkill");

        switch (skillHolder)
        {
            case skillHolder.skill1:
                animationStateName = "PlayerSkill1";
                playerSkillHash = Animator.StringToHash(animationStateName);
                break;
            case skillHolder.skill2:
                animationStateName = "PlayerSkill2";
                playerSkillHash = Animator.StringToHash(animationStateName);
                break;
            case skillHolder.skill3:
                animationStateName = "PlayerSkill3";
                playerSkillHash = Animator.StringToHash(animationStateName);
                break;
            default:
                animationStateName = "PlayerSkill4";
                playerSkillHash = Animator.StringToHash(animationStateName);
                break;
        }
    }

    private void Update()
    {
        if (!skillSystem.CheckEnergy(energy))
        {
            OnNotEnoughEnergy?.Invoke();
        }
        else
        {
            OnEnoughEnergy?.Invoke();
        }
    }

    public bool Cast()
    {
        if (!running && cooldownTimer == 0 && skillSystem.CheckEnergy(energy))
        {
            running =true;
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
            yield return null;
        }
        ready = true;
        cooldownTimer = 0;
        OnCooldownTimerDone?.Invoke();
    }

    public void Done()
    {
        running = false;
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

