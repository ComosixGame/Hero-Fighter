using UnityEngine;
using UnityEngine.AI;

public class GunEnemyAttack : AbsEnemyAttack
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private GameObject shotPos;
    [SerializeField] private float attackAnimTime;
    public float damage;
    public float speed = 100;
    private NavMeshAgent agent;
    private GameObjectPool gameObjectPool;
    private ObjectPoolerManager ObjectPoolerManager;
    private Animator animator;
    private int shootHash;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shootHash = Animator.StringToHash("Shoot");
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        gameObjectPool = GetComponent<GameObjectPool>();
    }
    protected override void HandleAttack()
    {
        agent.SetDestination(transform.position);
        
        if (readyAttack)
        {
            Bullet newBullet = ObjectPoolerManager.SpawnObject(bullet, shotPos.transform.position, shotPos.transform.rotation) as Bullet;
            newBullet.Fire(damage, speed, transform.forward.normalized);
            readyAttack = false;
            animator.SetTrigger(shootHash);
            Invoke("AttackCoolDown",0.2f);
            Invoke("AttackDone", attackAnimTime);
        }
    }
}
