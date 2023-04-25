using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossBehaviour : MonoBehaviour
{
    public enum State
    {
        chase,
        rangeAttack,
        closeAttack,
        disable,
    }
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRange;
    [SerializeField] private float closeAttackRange;
    [SerializeField] private Vector3 centerAttackRange;
    [SerializeField] private float maxSpeed;
    [SerializeField] private AbsBossAttack closeAttack;
    [SerializeField] private AbsBossAttack rangeAttack;
    private int velocityXHash, velocityZHash;
    private Collider[] hitColliders = new Collider[1];
    private Collider[] closeColliders = new Collider[1];
    [SerializeField] private State state = State.chase;
    private NavMeshAgent agent;
    private Animator animator;
    private BossDamageable damageable;
    private GameManager gameManager;
    private bool active;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityZHash = Animator.StringToHash("VelocityZ");
        damageable = GetComponent<BossDamageable>();
    }

    private void OnEnable()
    {
        active = false;
        StartGameEvent.OnStart += StartGame;
    }

    private void OnDisable() {
        StartGameEvent.OnStart -= StartGame;
    }

    private void StartGame()
    {
        active = true;
    }

    private void Start()
    {
        StartCoroutine(CheckPlayerInAttackRange());
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        HandleLook();

        switch (state)
        {
            case State.chase:
                HandleChase();
                break;
            case State.rangeAttack:
                if (rangeAttack?.Attack() == false)
                {
                    MoveToPosition(gameManager.player.position);
                }
                break;
            case State.closeAttack:
                if (!closeAttack?.Attack() == false)
                {
                    MoveToPosition(gameManager.player.position);
                }
                break;
            default:
                break;
        }

        HandleAnimationMove();
    }

    private void FixedUpdate()
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        if (animationState.IsName("Move"))
        {
            agent.enabled = true;
        }
        else
        {
            if(agent.hasPath) agent.ResetPath();
            agent.enabled = false;
        }
    }

    private IEnumerator CheckPlayerInAttackRange()
    {
        while (true)
        {
            int hitColliderCount = Physics.OverlapSphereNonAlloc(transform.position + centerAttackRange, attackRange, hitColliders, playerLayer);
            if (hitColliderCount == 0)
            {
                state = State.chase;
            }
            else
            {
                state = State.rangeAttack;
                int closeColliderCount = Physics.OverlapSphereNonAlloc(transform.position + centerAttackRange, closeAttackRange, closeColliders, playerLayer);
                if (closeColliderCount > 0)
                {
                    state = State.closeAttack;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void HandleChase()
    {
        MoveToPosition(gameManager.player.position);
    }

    private void MoveToPosition(Vector3 targetPos)
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(targetPos);
        }
    }

    private void HandleAnimationMove()
    {
        Vector3 Velocity = new Vector3(agent.velocity.x, 0, agent.velocity.z).normalized;
        if (Velocity.magnitude > 0)
        {
            animator.SetFloat(velocityXHash, Math.Abs(agent.velocity.x / maxSpeed));
            bool isLookRight = transform.forward.normalized == Vector3.right;
            animator.SetFloat(velocityZHash, isLookRight ? -Velocity.z : Velocity.z);
        }
        else
        {
            float vX = animator.GetFloat(velocityXHash);
            float vZ = animator.GetFloat(velocityZHash);
            vX = vX > 0.10f ? Mathf.Lerp(vX, 0, 20f * Time.deltaTime) : 0;
            vZ = vZ > 0.10f || vZ < -0.1f ? Mathf.Lerp(vZ, 0, 20f * Time.deltaTime) : 0;
            animator.SetFloat(velocityXHash, vX);
            animator.SetFloat(velocityZHash, vZ);
        }
    }

    private void HandleLook()
    {
        if (agent.enabled)
        {
            Vector3 dirLook = gameManager.player.position - transform.position;
            transform.rotation = Ultils.GetRotationLook(dirLook, transform.forward);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + centerAttackRange, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + centerAttackRange, closeAttackRange);
    }
#endif
}
