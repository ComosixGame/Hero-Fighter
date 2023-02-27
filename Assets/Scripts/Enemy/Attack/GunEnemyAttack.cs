using UnityEngine;
using UnityEngine.AI;

public class GunEnemyAttack : AbsEnemyAttack
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform targetChase;
    [SerializeField] private GameObject bullet;
    private NavMeshAgent agent;
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
    protected override void HandleAttack()
    {
        agent.SetDestination(transform.position);
        Invoke("ShootPlayer", 1f);
        
    }

    private void ShootPlayer()
    {
        Instantiate(bullet, targetChase.transform.position, targetChase.transform.rotation);
        CancelInvoke();
    }
}
