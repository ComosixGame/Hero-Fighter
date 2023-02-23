using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform targetChase;
    [SerializeField] private float attackRange;
    [SerializeField] private float readyRange;
    [SerializeField] private Vector3 centerAttackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackAnimTime;
    [SerializeField] private float stepBackTime;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] AnimationCurve speepStepBackCuvre;
    [SerializeField] EnemyAttack[] enemyAttacks;
    public float damage;
    private float stepBackTimer;
    private int attackHash, velocityHash, stepBackHash, hitHash, hitIndeHash;
    private bool readyAttack = true, stepBack;
    public bool attacking { get; private set; }
    public bool inTakeDamage { get; private set; }
    private bool inAttackRange;
    private bool addForce;
    private Vector3 force;
    private int hitIndex;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyDamageable damageable;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<EnemyDamageable>();

        velocityHash = Animator.StringToHash("Velocity");
        attackHash = Animator.StringToHash("Attack");
        stepBackHash = Animator.StringToHash("StepBack");
        hitHash = Animator.StringToHash("Hit");
        hitIndeHash = Animator.StringToHash("HitIndex");

        agent.speed = maxSpeed;

        foreach (EnemyAttack enemyAttack in enemyAttacks)
        {
            enemyAttack.enemyBehaviour = this;
        }

    }

    private void OnEnable()
    {
        damageable.OnTakeDamage += HandleHitReaction;
    }

    private void OnDisable()
    {
        damageable.OnTakeDamage -= HandleHitReaction;
    }


    void Update()
    {
        if (!inTakeDamage)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + centerAttackRange, readyRange, playerLayer);
            if (hitColliders.Length == 0)
            {
                inAttackRange = false;
                agent.speed = maxSpeed;
                if (!attacking && !stepBack)
                {
                    agent.SetDestination(targetChase.position);
                }
            }
            else
            {
                inAttackRange = true;
                agent.speed = walkSpeed;
                HandleAttack();
            }
        }
        HandleStepBack();
        HandleAnimationMove();

    }

    private void HandleAttack()
    {
        if (readyAttack)
        {
            Collider[] hitCollidersAttack = Physics.OverlapSphere(transform.position + centerAttackRange, attackRange, playerLayer);
            if (hitCollidersAttack.Length > 0)
            {
                agent.SetDestination(transform.position);
                readyAttack = false;
                attacking = true;
                animator.SetTrigger(attackHash);
                Invoke("AttackCoolDown", attackCooldown);
                Invoke("AttackDone", attackAnimTime);
            }
            else
            {
                agent.SetDestination(targetChase.position);
            }
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }

    private void AttackCoolDown()
    {
        readyAttack = true;
    }

    private void AttackDone()
    {
        attacking = false;
        if (inAttackRange)
        {
            stepBack = true;
            animator.SetTrigger(stepBackHash);
            Invoke("StepBackDone", stepBackTime);
        }
    }

    private void StepBackDone()
    {
        stepBack = false;
        stepBackTimer = 0;
    }

    private void HandleStepBack()
    {
        if (stepBack)
        {
            stepBackTimer += Time.deltaTime;
            float speed = speepStepBackCuvre.Evaluate(stepBackTimer / stepBackTime) * 2f * Time.deltaTime;
            agent.Move(-transform.forward.normalized * speed);
        }
    }
    private void HandleAnimationMove()
    {
        Vector3 horizontalVelocity = new Vector3(agent.velocity.x, 0, agent.velocity.z);
        float Velocity = horizontalVelocity.magnitude / maxSpeed;
        if (Velocity > 0)
        {
            animator.SetFloat(velocityHash, Velocity);
        }
        else
        {
            float v = animator.GetFloat(velocityHash);
            v = Mathf.MoveTowards(v, 0, 2f * Time.deltaTime);
            animator.SetFloat(velocityHash, v);
        }
    }

    private void HandleHitReaction(float damage)
    {
        agent.ResetPath();
        transform.LookAt(targetChase);
        CancelInvoke("HitDone");
        inTakeDamage = true;
        animator.SetFloat(hitIndeHash, hitIndex);
        animator.SetTrigger(hitHash);
        hitIndex++;
        if (hitIndex > 1)
        {
            hitIndex = 0;
        }
        Invoke("HitDone", 0.4f);
    }

    private void HitDone()
    {
        inTakeDamage = false;
        hitIndex = 0;

    }

    public void AddForce(Vector3 force) {
        agent.Move(force);
    }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + centerAttackRange, readyRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + centerAttackRange, attackRange);
        }
#endif
}
