using System;
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
    [SerializeField] private float viewRange;
    [SerializeField] private Vector3 centerAttackRange;
    [SerializeField] private float stepBackTime;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float walkSpeed;
    private int velocityHash;
    private State state = State.chase;
    private bool disable;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyDamageable damageable;
    private AbsEnemyAttack absEnemyAttack;
    private GameManager gameManager;
    private bool isStart;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<EnemyDamageable>();
        absEnemyAttack = GetComponent<AbsEnemyAttack>();

        velocityHash = Animator.StringToHash("Velocity");
    }

    private void OnEnable()
    {
        damageable.OnTakeDamageStart += DisableEnemy;
        damageable.OnTakeDamageEnd += EnableEnemy;
        gameManager.OnInitUiDone += StartGame;
    }

    private void OnDisable()
    {
        isStart = false;
        damageable.OnTakeDamageStart -= DisableEnemy;
        damageable.OnTakeDamageEnd -= EnableEnemy;
        gameManager.OnInitUiDone -= StartGame;
    }
        
    private void StartGame()
    {
        isStart = true;
        disable = !isStart;
    }

    private void Update()
    {
        if (isStart)
        {
            CheckPlayerInView();
            CheckEnemyTakeDamage();
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
                case State.disable:
                    break;
                default:
                    throw new InvalidCastException("invlid state");

            }
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

    private void CheckEnemyTakeDamage()
    {
        if (disable)
        {
            state = State.disable;
        }
    }

    private void HandleChase()
    {
        agent.speed = maxSpeed;
        MoveToPosition(gameManager.player.position);
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
        if (agent.velocity.x > 0)
        {
            Quaternion rot = Quaternion.LookRotation(Vector3.right);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, rot, 40 * Time.deltaTime);
        }
        else if (agent.velocity.x < 0)
        {
            Quaternion rot = Quaternion.LookRotation(Vector3.left);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, rot, 40 * Time.deltaTime);
        }
    }

    private void DisableEnemy(Vector3 hitPoint, float damage, AttackType attackType)
    {
        disable = true;
        agent.ResetPath();

    }

    private void EnableEnemy()
    {
        disable = false;
    }

    private void MoveToPosition(Vector3 targetPos)
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(targetPos);
        }
    }

    //Attach Animation Event
    public void CancelHurtBox()
    {
        absEnemyAttack.CancleAttack();
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + centerAttackRange, viewRange);
    }
#endif
}
