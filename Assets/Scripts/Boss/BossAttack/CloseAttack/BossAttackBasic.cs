using UnityEngine;
using UnityEngine.AI;

public class BossAttackBasic : AbsBossCloseAttack
{
    [SerializeField] private EnemyHurtBox hurtBox;
    [SerializeField] private AnimatorOverrideController animatorOverride;
    [SerializeField] private AnimationClip animationClip;
    private int attackHash;
    private Animator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        attackHash = Animator.StringToHash("BasicAttack");

        animatorOverride["BasicAttack"] = animationClip;
        animator.runtimeAnimatorController = animatorOverride;
    }

    public override void Action()
    {
        agent.SetDestination(transform.position);
        animator.SetTrigger(attackHash);
    }

    public void AttackStart()
    {
        hurtBox?.gameObject.SetActive(true);
    }

    public void AttackEnd()
    {
        hurtBox?.gameObject.SetActive(false);
        attacking = false;
    }
}
