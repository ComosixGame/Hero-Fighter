using System;
using UnityEngine;
using MyCustomAttribute;

public abstract class AbsSpecialSkill : MonoBehaviour
{
    public string skillName;
    public string skillDescription;
    [SerializeField] private int MaxEnergy;
    private bool ready;
    [SerializeField, ReadOnly] private int energy;
    protected int SpecialSkilHash;
    [SerializeField] protected AnimationClip animationClip;
    [SerializeField] protected AnimatorOverrideController animatorOverride;
    protected Animator animator;
    public event Action OnDone;
    public event Action<int> OnAccumulationEnergy;

    protected virtual void Awake() {
        animator = GetComponent<Animator>();
        SpecialSkilHash = Animator.StringToHash("SpecialSkill");
        animatorOverride["SpecialSkill"] = animationClip;
        animator.runtimeAnimatorController = animatorOverride;
    }

    protected abstract void Action();

    public bool Cast()
    {
        if (ready)
        {
            ready = false;
            energy = 0;
            Action();
            return true;
        }

        return false;
    }

    public void Done()
    {
        OnDone?.Invoke();
    }

    public void AccumulationEnergy(int bonusEnergy)
    {
        energy += bonusEnergy;
        if (energy >= MaxEnergy)
        {
            ready = true;
            energy = MaxEnergy;
        }

        OnAccumulationEnergy?.Invoke(energy);
    }

}
