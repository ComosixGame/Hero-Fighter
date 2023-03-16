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
    public event Action OnStart;
    public event Action OnDone;
    public event Action<int> OnAccumulationEnergy;

    protected virtual void Awake() {
        animator = GetComponent<Animator>();
        SpecialSkilHash = Animator.StringToHash("SpecialSkill");
        animatorOverride["SpecialSkill"] = animationClip;
        animator.runtimeAnimatorController = animatorOverride;
        energy = 10;
    }

    private void Update() {
        if(energy == MaxEnergy) {
            ready = true;
        } else {
            ready = false;
        }
    }

    protected abstract void Action();

    public bool Cast()
    {
        if (ready)
        {   
            energy = 0;
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

    public void AccumulationEnergy(int bonusEnergy)
    {
        energy += bonusEnergy;
        if (energy >= MaxEnergy)
        {
            energy = MaxEnergy;
        }

        OnAccumulationEnergy?.Invoke(energy);
    }

}
