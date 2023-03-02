using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAttack : AbsEnemyAttack
{
    [SerializeField] private Vector3 centerAttackRange;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private EnemyHurtBox[] hurtBoxes;
    private int attackHash;
    private int stepBackHash;
    private bool stepBack;
    private float stepBackTimer;
    private bool disable;
    [SerializeField] private float stepBackTime;
    [SerializeField] private AnimationCurve speepStepBackCuvre;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyDamageable damageable;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<EnemyDamageable>();

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

    private void OnEnable()
    {
        damageable.OnTakeDamageStart += DisableEnemy;
        damageable.OnTakeDamageEnd += EnableEnemy;
    }

    private void OnDisable()
    {
        damageable.OnTakeDamageStart -= DisableEnemy;
        damageable.OnTakeDamageEnd -= EnableEnemy;
    }

    private void Update()
    {
        HandleStepBack();
    }

    public override void HandleAttack()
    {
        if (!disable && readyAttack)
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
        if(!disable) {
            hurtBoxes[index].gameObject.SetActive(true);
        }
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

    private void DisableEnemy(Vector3 hitPoint, float damage, AttackType attackType)
    {
        foreach (EnemyHurtBox hurtBox in hurtBoxes)
        {
            hurtBox.gameObject.SetActive(false);
        }
        disable = true;
        agent.ResetPath();
    }

    private void EnableEnemy()
    {
        disable = false;
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + centerAttackRange, attackRange);
    }
#endif
}