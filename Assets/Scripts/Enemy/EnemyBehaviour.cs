using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Transform player;
    private NavMeshAgent agent;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance <= agent.stoppingDistance) {
            Vector3 randomPos = Ultils.RandomNavmeshLocation(transform.position, 20f, 30f);
            agent.SetDestination(randomPos);
        }
    }
}
