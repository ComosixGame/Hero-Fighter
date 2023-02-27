using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class EnemyBehaviour : MonoBehaviour
{

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform targetChase;
    // [SerializeField] private float attackRange;
    [SerializeField] private float readyRange;
    [SerializeField] private Vector3 centerAttackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackAnimTime;
    [SerializeField] private float stepBackTime;
    [SerializeField] private float standUpTime;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] AnimationCurve speepStepBackCuvre;
    [SerializeField] AnimationCurve knockBackCuvre;
    [SerializeField] EnemyAttack[] enemyAttacks;
    public float damage;
    [SerializeField] private float stepBackTimer, knockTimer;
    private int attackHash, velocityHash, stepBackHash, hitHash, hitIndeHash, knockHash, standUpHash;
    private bool stepBack;
    public bool knockBack;
    public bool attacking { get; private set; }
    public bool inTakeDamage { get; private set; }
    public bool inAttackRange { get; private set; }
    private int hitIndex;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private Animator animator;
    private EnemyDamageable damageable;
    private AbsEnemyAttack absEnemyAttack;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<EnemyDamageable>();
        absEnemyAttack = GetComponent<AbsEnemyAttack>();

        velocityHash = Animator.StringToHash("Velocity");
        attackHash = Animator.StringToHash("Attack");
        stepBackHash = Animator.StringToHash("StepBack");
        hitHash = Animator.StringToHash("Hit");
        hitIndeHash = Animator.StringToHash("HitIndex");
        knockHash = Animator.StringToHash("Knock");
        standUpHash = Animator.StringToHash("StandUp");

        foreach (EnemyAttack enemyAttack in enemyAttacks)
        {
            enemyAttack.enemyBehaviour = this;
        }

    }

    private void OnEnable()
    {
        damageable.OnTakeDamage += HandleHitReaction;
        absEnemyAttack.OnAttackDone += AttackDone;
    }

    private void OnDisable()
    {
        damageable.OnTakeDamage -= HandleHitReaction;
    }

    private void Update()
    {
        HandleStepBack();
        HandleKnockBack();

        if (!inTakeDamage)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + centerAttackRange, readyRange, playerLayer);
            if (hitColliders.Length == 0)
            {
                inAttackRange = false;
                agent.speed = maxSpeed;
                if (!absEnemyAttack.attacking && ! stepBack && !knockBack)
                {
                    MoveToPosition(targetChase.position);
                }
            }
            else
            {
                inAttackRange = true;
                agent.speed = walkSpeed;
                absEnemyAttack.Attack();
            }
        }

        HandleLook();
        HandleAnimationMove();
    }


    private void HandleStepBack()
    {
        if (stepBack)
        {
            stepBackTimer += Time.deltaTime;
            float speed = speepStepBackCuvre.Evaluate(stepBackTimer / stepBackTime) * 5f;
            agent.Move(-transform.forward.normalized * speed * Time.deltaTime);
        }
    }
    private void AttackDone()
    {
        if (inAttackRange && !knockBack)
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

    private void HandleKnockBack()
    {
        if (knockBack)
        {
            knockTimer += Time.deltaTime;
            float speed = knockBackCuvre.Evaluate(knockTimer) * 10f;
            agent.Move(-transform.forward.normalized * speed * Time.deltaTime);
        }
    }

    public void KnockEnd()
    {
        knockBack = false;
        knockTimer = 0;
        Invoke("StandUp", standUpTime);
    }

    private void StandUp()
    {
        animator.SetTrigger(standUpHash);
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

    private void HandleHitReaction(Vector3 hitPoint, float damage, AttackType attackType)
    {
        Quaternion rot = Quaternion.LookRotation(targetChase.position - transform.position);
        rot.x = 0;
        rot.z = 0;
        transform.rotation = rot;
        inTakeDamage = true;
        if (attackType == AttackType.light)
        {
            animator.SetFloat(hitIndeHash, hitIndex);
            animator.SetTrigger(hitHash);
            hitIndex++;
            if (hitIndex > 1)
            {
                hitIndex = 0;
            }
        }
        else
        {
            animator.SetTrigger(knockHash);
            animator.ResetTrigger(standUpHash);
            knockBack = true;
            agent.ResetPath();
        }
    }

    public void HitDone()
    {
        inTakeDamage = false;
        hitIndex = 0;
    }

    private void HandleLook()
    {
        if (!inTakeDamage)
        {
            Quaternion rot = Quaternion.LookRotation(targetChase.position - transform.position);
            rot.x = 0;
            rot.z = 0;
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, rot, 40 * Time.deltaTime);
        }
    }

    public void Push(Vector3 force)
    {
        CancelInvoke("CancelPush");
        rb.isKinematic = false;
        agent.enabled = false;
        rb.AddForce(force, ForceMode.Impulse);
        Invoke("CancelPush", 0.02f);
    }

    public void CancelPush()
    {
        rb.isKinematic = true;
        agent.enabled = true;
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + centerAttackRange, readyRange);
    }
#endif
}
