using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class EnemyBehaviour : MonoBehaviour
{
    public enum State
    {
        chase,
        attack,
        notReady,
    }
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform targetChase;
    // [SerializeField] private float attackRange;
    [SerializeField] private float viewRange;
    [SerializeField] private Vector3 centerAttackRange;
    [SerializeField] private float stepBackTime;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float walkSpeed;
    private int velocityHash, reloadHash;
    [HideInInspector] public State state = State.chase;
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
        reloadHash = Animator.StringToHash("Reload");
    }


    private void Update()
    {
        CheckPlayerInView();
        HandleLook();
        HandleAnimationMove();
        switch (state)
        {
            case State.chase:
                HandleChase();
                break;
            case State.attack:
                agent.speed = walkSpeed;
                absEnemyAttack.HandleAttack();
                break;
            case State.notReady:
                break;
            default:
                throw new InvalidCastException("invlid state");

        }
    }

    private void CheckPlayerInView()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + centerAttackRange, viewRange, playerLayer);
        if (hitColliders.Length == 0)
        {
            state = State.chase;
        }
        else
        {
            state = State.attack;
        }
    }

    private void HandleChase()
    {
        agent.speed = maxSpeed;
        MoveToPosition(targetChase.position);
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

    private void HandleLook()
    {
        Quaternion rot = Quaternion.LookRotation(targetChase.position - transform.position);
        rot.x = 0;
        rot.z = 0;
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, rot, 40 * Time.deltaTime);
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
        Gizmos.DrawWireSphere(transform.position + centerAttackRange, viewRange);
    }
#endif
}
