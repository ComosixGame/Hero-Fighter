using UnityEngine;
using UnityEngine.AI;

public class RocketAttack : AbsBossRangedAttack
{
    [SerializeField] private Bullet rocket;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private int numberOfRocket;
    private int rocketCount;
    private int castHash;
    private Animator animator;
    private NavMeshAgent agent;
    private ObjectPoolerManager objectPooler;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        objectPooler = ObjectPoolerManager.Instance;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        castHash = Animator.StringToHash("Cast");
    }

    protected override void Action()
    {
        agent.ResetPath();
        animator.SetTrigger(castHash);
    }

    public void RocketLaunch()
    {
        Vector3 PositionLaunch = transform.position + Vector3.up * 10f;
        Quaternion rot = Quaternion.LookRotation(gameManager.player.position - PositionLaunch);
        Bullet newRocket = objectPooler.SpawnObject(rocket, PositionLaunch, rot) as Bullet;
        newRocket.Fire(damage, speed);
        rocketCount++;
        Invoke("NextLauch", 0.3f);
    }

    private void NextLauch()
    {

        if (rocketCount < numberOfRocket)
        {
            RocketLaunch();
        }
        else
        {
            rocketCount = 0;
            Invoke("RocketLaunchDone", 0.7f);
        }
    }

    private void RocketLaunchDone()
    {
        attacking = false;
    }

    protected override void SideActionAttack()
    {
        base.SideActionAttack();
        if (!attacking)
        {
            agent.SetDestination(gameManager.player.position);
            Quaternion rot = Ultils.GetRotationLook((gameManager.player.position - transform.position), transform.forward);
            transform.rotation = rot;

        }
    }

}
