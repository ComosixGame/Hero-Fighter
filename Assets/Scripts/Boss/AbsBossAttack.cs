using System;
using UnityEngine;

public abstract class AbsBossAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Type
    {
        ranged,
        close
    }
    [SerializeField] protected float coolDownTime;
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private Type type;
    protected int attackHash;
    private bool readyAttack = true;
    protected bool attacking;
    protected Animator animator;
    private static AnimatorOverrideController animatorOverride;
    protected event Action OnCancelCloseAttack;
    protected event Action OnCancelRangeAttack;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        if (animatorOverride == null)
        {
            animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
        }
        string animkey;
        if (type == Type.ranged)
        {
            animkey = "RangedAttack";
        }
        else
        {
            animkey = "CloseAttack";
        }
        attackHash = Animator.StringToHash(animkey);
        animatorOverride[animkey] = animationClip;
        animator.runtimeAnimatorController = animatorOverride;
    }

    private void update()
    {
        if (attacking)
        {
            AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
            if (type == Type.close)
            {
                if (!animationState.IsName("CloseAttack"))
                {
                    attacking = false;
                    OnCancelCloseAttack?.Invoke();
                }
            }
            else
            {
                if (!animationState.IsName("RangedAttack"))
                {
                    attacking = false;
                    OnCancelRangeAttack?.Invoke();
                }
            }

        }
    }

    public bool Attack()
    {
        if (readyAttack)
        {
            readyAttack = false;
            attacking = true;
            Action();
            Invoke("AttackCoolDown", coolDownTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    protected abstract void Action();

    protected void AttackCoolDown()
    {
        readyAttack = true;
    }
}
