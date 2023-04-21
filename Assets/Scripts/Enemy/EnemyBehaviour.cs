using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    public enum State
    {
        chase,
        attack,
        disable,
    }
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRange;
    [SerializeField] private Vector3 centerAttackRange;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float walkSpeed;
    [HideInInspector] public bool lockRotation = true;
    private int velocityXHash, velocityZHash;
    private State state = State.chase;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyDamageable damageable;
    private AbsEnemyAttack absEnemyAttack;
    private GameManager gameManager;
    private bool active;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<EnemyDamageable>();
        absEnemyAttack = GetComponent<AbsEnemyAttack>();

        velocityXHash = Animator.StringToHash("VelocityX");
        velocityZHash = Animator.StringToHash("VelocityZ");
    }

    private void OnEnable()
    {
        agent.enabled = true;
        if (agent.hasPath)
        {
            agent.ResetPath();
        }
        StartGameEvent.OnStart += StartGame;
        gameManager.OnNextWaveDone += NextWaveDone;
        StartCoroutine(CheckPlayerInAttackRange());

        AreaColliderTrigger.onEnter += PlayterEnter;
    }

    private void OnDisable()
    {
        active = false;
        agent.enabled = false;
        StartGameEvent.OnStart -= StartGame;
        AreaColliderTrigger.onEnter -= PlayterEnter;
        gameManager.OnNextWaveDone -= NextWaveDone;
        StopAllCoroutines();
    }

    private void StartGame()
    {
        active = true;
    }

    private void PlayterEnter() {
        if(gameObject.activeSelf) {
            active = true;
        }
    }

    private void Update()
    {
        if (active && gameManager.player != null)  
        {
            HandleLook();
            HandleAnimationMove();
            switch (state)
            {
                case State.chase:
                    HandleChase();
                    break;
                case State.attack:
                    HandleAttack();
                    break;
                default:
                    break;
            }
        }
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
            agent.enabled = false;
        }
    }

    private IEnumerator CheckPlayerInAttackRange()
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + centerAttackRange, attackRange, playerLayer);
            if (hitColliders.Length == 0)
            {
                state = State.chase;
            }
            else
            {
                if (agent.hasPath) agent.ResetPath();
                state = State.attack;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void HandleChase()
    {
        if (Vector3.Distance(gameManager.player.position, transform.position) <= 5f)
        {
            agent.speed = walkSpeed;
        }
        else
        {
            agent.speed = maxSpeed;
        }
        MoveToPosition(gameManager.player.position);
    }

    private void HandleAttack()
    {
        absEnemyAttack.Attack();
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
        if (lockRotation && agent.enabled)
        {
            Vector3 dirLook = gameManager.player.position - transform.position;
            transform.rotation = Ultils.GetRotationLook(dirLook, transform.forward);
        }
    }


    private void MoveToPosition(Vector3 targetPos)
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(targetPos);
        }
    }

    private void NextWaveDone()
    {
        active = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + centerAttackRange, attackRange);
    }
#endif
}
