using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAttack : AbsEnemyAttack
{
    [SerializeField] private Vector3 centerAttackRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private EnemyHurtBox[] hurtBoxes;
    private int attackHash;
    private int stepBackHash;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyBehaviour enemyBehaviour;
    private GameManager gameManager;
    private bool stepBack;
    private float stepBackTimer;
    [SerializeField] private float stepBackTime;
    [SerializeField] private AnimationCurve speepStepBackCuvre;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();

        attackHash = Animator.StringToHash("Attack");
        stepBackHash = Animator.StringToHash("StepBack");
    }

    private void Start()
    {
        foreach (EnemyHurtBox hurtBox in hurtBoxes)
        {
            hurtBox.gameObject.SetActive(false);
        }
    }

    private void Update() {
        HandleStepBack();
    }

    public override void HandleAttack()
    {
        if (readyAttack)
        {
            Collider[] hitCollidersAttack = Physics.OverlapSphere(transform.position + centerAttackRange, attackRange, playerLayer);
            if (hitCollidersAttack.Length > 0)
            {
                MoveToPosition(transform.position);
                readyAttack = false;
                animator.SetTrigger(attackHash);
                Invoke("AttackCoolDown", attackCooldown);
            }
            else
            {
                MoveToPosition(gameManager.player.position);
            }
        }
        else
        {
            MoveToPosition(transform.position);
        }
    }

    public void StartAttack(int index)
    {
        hurtBoxes[index].gameObject.SetActive(true);
    }

    public void EndAttack(int index)
    {
        hurtBoxes[index].gameObject.SetActive(false);
        stepBack = true;
        animator.SetTrigger(stepBackHash);
    }

    private void MoveToPosition(Vector3 targetPos)
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(targetPos);
        }
    }

    private void HandleStepBack()
    {
        if (stepBack)
        {
            stepBackTimer += Time.deltaTime;
            float speed = speepStepBackCuvre.Evaluate(stepBackTimer / stepBackTime) * 5f;
            agent.Move(-transform.forward.normalized * speed * Time.deltaTime);
            Invoke("StepBackDone", stepBackTime);
        }
    }

    private void StepBackDone()
    {
        stepBack = false;
        stepBackTimer = 0;
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + centerAttackRange, attackRange);
    }
#endif
}