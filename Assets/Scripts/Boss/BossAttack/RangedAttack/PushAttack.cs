using UnityEngine;
using UnityEngine.AI;

public class PushAttack : AbsBossRangedAttack
{
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private int numberOfPushes;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private EnemyHurtBox hurtBox;
    private int rangedAttackHash;
    private bool startPush;
    private float accelerationOrigin;
    private int pushCount;
    private Vector3 dir;
    private Vector3 startPos;
    private Animator animator;
    private NavMeshAgent agent;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rangedAttackHash = Animator.StringToHash("RangedAttack");
    }

    private void Start()
    {
        accelerationOrigin = agent.acceleration;
    }

    protected override void Action()
    {
        if (!startPush)
        {
            StartPush();
        }
    }

    private void StartPush()
    {
        startPush = true;
        agent.acceleration = 50;
        hurtBox?.gameObject.SetActive(true);
        animator.SetBool(rangedAttackHash, true);
        dir = (gameManager.player.position - transform.position).normalized;
        startPos = transform.position;
        HandleLook();
        Invoke("CancelPush", 3);
    }

    private void CancelPush()
    {
        startPush = false;
        pushCount++;
        if (pushCount < numberOfPushes)
        {
            hurtBox?.gameObject.SetActive(false);
            animator.SetBool(rangedAttackHash, false);
            agent.acceleration = accelerationOrigin;
            Invoke("StartPush", 0.5f);
        }
        else
        {
            hurtBox?.gameObject.SetActive(false);
            animator.SetBool(rangedAttackHash, false);
            agent.acceleration = accelerationOrigin;
            pushCount = 0;
            attacking = false;
        }
    }

    private void Update()
    {
        if (startPush)
        {
            agent.ResetPath();
            agent.Move(dir * speed * Time.deltaTime);

            if (Vector3.Distance(startPos, transform.position) >= distance)
            {
                CancelInvoke("CancelPush");
                CancelPush();
            }
        }
        else
        {
            agent.SetDestination(gameManager.player.position);
        }

    }

    private void HandleLook()
    {
        Quaternion rot = Ultils.GetRotationLook(dir, transform.forward);
        transform.rotation = rot;
    }
}
