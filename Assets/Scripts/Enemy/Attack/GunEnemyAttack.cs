using UnityEngine;
using UnityEngine.AI;

public class GunEnemyAttack : AbsEnemyAttack
{
    public float damage;
    public float speedBullet;
    [SerializeField] private LayerMask layerTarget;
    [SerializeField] private Vector3 centerMeleeRange;
    [SerializeField] private float meleeRange;
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform shotPos;
    [SerializeField] private EnemyHurtBox[] hurtBoxes;
    private int shootHash;
    private int rifleKickHash;
    private bool disable, attackMelee;
    private NavMeshAgent agent;
    private GameObjectPool gameObjectPool;
    private ObjectPoolerManager ObjectPoolerManager;
    private Animator animator;
    private EnemyDamageable damageable;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        gameObjectPool = GetComponent<GameObjectPool>();
        damageable = GetComponent<EnemyDamageable>();

        shootHash = Animator.StringToHash("Shoot");
        rifleKickHash = Animator.StringToHash("RifleKick");
        ObjectPoolerManager = ObjectPoolerManager.Instance;
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

    private void Start()
    {
        foreach (EnemyHurtBox hurtBox in hurtBoxes)
        {
            hurtBox.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        HandleMeleeAttack();
    }

    public override void HandleAttack()
    {
        MoveToPosition(transform.position);

        if (!disable && !attackMelee && readyAttack)
        {
            Bullet newBullet = ObjectPoolerManager.SpawnObject(bullet, shotPos.position, shotPos.rotation) as Bullet;
            newBullet.Fire(damage, speedBullet, transform.forward.normalized);
            readyAttack = false;
            animator.SetTrigger(shootHash);
            Invoke("AttackCoolDown", attackCooldown);
        }
    }

    public void HandleMeleeAttack()
    {
        if (!disable && !attackMelee)
        {
            Collider[] hitCollidersAttack = Physics.OverlapSphere(transform.position + centerMeleeRange, meleeRange, layerTarget);
            if (hitCollidersAttack.Length > 0)
            {
                // CancelInvoke("AttackMeleeCoolDown");
                attackMelee = true;
                animator.SetTrigger(rifleKickHash);
                // Invoke("AttackMeleeCoolDown", attackCooldown);
            }
        }
    }
    

    public void StartMeleeAttack(int index)
    {
        if (!disable)
        {
            hurtBoxes[index].gameObject.SetActive(true);
        }
    }

    public void EndMeleeAttack(int index)
    {
        attackMelee = false;
        hurtBoxes[index].gameObject.SetActive(false);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + centerMeleeRange, meleeRange);
    }
#endif
}
