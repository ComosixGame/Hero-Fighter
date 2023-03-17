using System;
using UnityEngine;
using UnityEngine.AI;
using MyCustomAttribute;

[RequireComponent(typeof(NavMeshAgent))]
public class BossBehaviour : MonoBehaviour
{
    public enum State
    {
        chase,
        attack,
        attacking,
        disable,
    }
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float rangedAttackRange;
    [SerializeField] private float closeAttackRange;
    [SerializeField] private Vector3 centerAttackRange;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private EnemyHurtBox[] hurtBoxes;
    [SerializeField] private BossPhase[] phases;
    private int velocityHash;
    [SerializeField, ReadOnly] private int indexPhase;
    private State state = State.chase;
    private NavMeshAgent agent;
    private Animator animator;
    private BossDamageable damageable;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        hurtBoxes = GetComponentsInChildren<EnemyHurtBox>();
        damageable = GetComponent<BossDamageable>();

    }

    private void OnEnable() {
        damageable.OnTakeDamage += CheckPhase;
    }

    private void OnDisable() {
        damageable.OnTakeDamage -= CheckPhase;
    }

    private void Start()
    {
        foreach (EnemyHurtBox hurtBox in hurtBoxes)
        {
            hurtBox.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInView();
        CheckAttacking();
        HandleLook();

        switch (state)
        {
            case State.chase:
                HandleChase();
                break;
            case State.attack:
            case State.attacking:
                HandleAttack();
                break;
            case State.disable:
                break;
            default:
                throw new InvalidCastException("invlid state");
        }

        HandleAnimationMove();
    }

    private void CheckPlayerInView()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + centerAttackRange, rangedAttackRange, playerLayer);
        if (hitColliders.Length == 0)
        {
            agent.speed = maxSpeed;
            state = State.chase;
        }
        else
        {
            agent.speed = walkSpeed;
            state = State.attack;
        }
    }

    private void CheckPhase(float health) {
        for(int i = 0; i < phases.Length ; i++) {
            if(health <= damageable.maxHealth * phases[i].healthThreshold / 100) {
                indexPhase = i;
            }
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


    private void CheckAttacking()
    {
        BossPhase currentPhase = phases[indexPhase];
        if (currentPhase.rangedAttack?.attacking == true || currentPhase.closeAttack?.attacking == true)
        {
            state = State.attacking;
        }
    }

    private void HandleAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + centerAttackRange, closeAttackRange, playerLayer);


        BossPhase currentPhase = phases[indexPhase];
        if (hitColliders.Length == 0)
        {
            if (!currentPhase.closeAttack?.attacking == true)
            {
                currentPhase.rangedAttack?.Attack();
            }
        }
        else
        {
            if (!currentPhase.rangedAttack?.attacking == true)
            {
                currentPhase.closeAttack?.Attack();
            }
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

    private void HandleLook()
    {
        // Quaternion rot = Utils.HandleLook;
        Quaternion rot = Ultils.GetRotationLook(agent.velocity, transform.forward);
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, rot, 40 * Time.deltaTime);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + centerAttackRange, rangedAttackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + centerAttackRange, closeAttackRange);
    }
#endif

    [Serializable]
    private class BossPhase
    {
        public string name;
        [Label("Health Threshold (%)")] public float healthThreshold = 100;
        public AbsBossCloseAttack closeAttack;
        public AbsBossRangedAttack rangedAttack;
    }
}
