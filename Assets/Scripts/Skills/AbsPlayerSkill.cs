using System;
using UnityEngine;

public abstract class AbsPlayerSkill : MonoBehaviour
{
    public string skillName;
    protected int PlayerSkillHash;
    protected Animator animator;
    public event Action OnStart;
    public event Action OnDone;
    public event Action<int> OnAccumulationEnergy;
    [SerializeField] private int attritionEnergy;
    [SerializeField] private int currentEnergy;
    [SerializeField] private float coolDownTime;
    [SerializeField] private float maxCoolDownTime;

    [SerializeField] protected AnimationClip animationClip;
    private bool ready, coolDown;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        PlayerSkillHash = Animator.StringToHash("PlayerSkill");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (attritionEnergy >= currentEnergy)
        {
            ready = true;
        }
        else
        {
            ready = false;
        }

        if (coolDown)
        {
            coolDownTime -= Time.deltaTime;

            if (coolDownTime == 0)
            {
                coolDown = false;
            }
        }

    }

    protected abstract void Action();

    public bool Cast()
    {
        if (ready)
        {
            ready = false;
            currentEnergy -= attritionEnergy;
            coolDownTime = maxCoolDownTime;
            Action();
            OnStart?.Invoke();
            return true;
        }

        return false;
    }

    public void Done()
    {
        OnDone?.Invoke();
    }

    public void Active()
    {
        AnimatorOverrideController animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animatorOverride["PlayerSkill"] = animationClip;
        animator.runtimeAnimatorController = animatorOverride;
    }
}
