using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAttack : AbsEnemyAttack
{
    [SerializeField] private Vector3 centerAttackRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform targetChase;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackAnimTime;
    private NavMeshAgent agent;
    private Animator animator;
    private int attackHash;
    private EnemyBehaviour enemyBehaviour;

    private void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        attackHash = Animator.StringToHash("Attack");
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void HandleAttack()
    {
        if (readyAttack)
        {
            Collider[] hitCollidersAttack = Physics.OverlapSphere(transform.position + centerAttackRange, attackRange, playerLayer);
            if (hitCollidersAttack.Length > 0)
            {
                MoveToPosition(transform.position);

                readyAttack = false;
                attacking = true;
                animator.SetTrigger(attackHash);
                Invoke("AttackCoolDown", attackCooldown);
                Invoke("AttackDone", attackAnimTime);
            }
            else
            {
                MoveToPosition(targetChase.position);
            }
        }
        else
        {
            MoveToPosition(transform.position);
        }
    }

    private void MoveToPosition(Vector3 targetPos)
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(targetPos);
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + centerAttackRange, attackRange);
    }
#endif
}