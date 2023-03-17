using UnityEngine;
using UnityEngine.AI;

public class BossAttackPunch : AbsBossCloseAttack
{
    private int punchHash;
    [SerializeField] private EnemyHurtBox hurtBox;
    private Animator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        punchHash = Animator.StringToHash("Punch");
    }

    public override void Action()
    {
        agent.SetDestination(transform.position);
        animator.SetTrigger(punchHash);
    }

    public void PuchStart()
    {
        hurtBox?.gameObject.SetActive(true);
    }

    public void PuchEnd()
    {
        hurtBox?.gameObject.SetActive(false);
        attacking = false;
    }
}
