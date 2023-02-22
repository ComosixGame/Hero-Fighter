using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    private enum ComboState
    {
        None,
        Attack1,
        Attack2,
        Attack3
    }

    private enum State
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Looking
    }

    private NavMeshAgent agent;
    private Animator animator;
    private int attack1Hash;
    private int attack2Hash;
    private int attack3Hash;
    private int velocityHash;
    private bool activeTimerToReset;
    private ComboState current_Combo_State;
    private State state, preState;
    public GameObject leftHandAttackPoint, rightHandAttackPoint, RightLegAttackPoint;
    private Transform targetChase;
    private Vector3 playerPosition;
    public LayerMask playerLayer;
    private bool isAttack, isChase;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        attack1Hash = Animator.StringToHash("Attack1");
        attack2Hash = Animator.StringToHash("Attack2");
        attack3Hash = Animator.StringToHash("Attack3");
        velocityHash = Animator.StringToHash("Velocity");

        targetChase = FindObjectOfType<CharacterController>().transform;
    }


    private void Start()
    {
        current_Combo_State = ComboState.None;

        isChase = true;
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(current_Combo_State);
        playerPosition = targetChase.position;
        HandleAnimation();
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Chase:
                Chase(playerPosition);
                break;
            case State.Attack:
                Attack(playerPosition);
                break;
            default:
                break;
        }
    }

    //Attacking
    private void Attacking()
    {
        // if (current_Combo_State == ComboState.Attack3)
        //     return;

        // current_Combo_State++;
        // activeTimerToReset = true;
        
        // if (current_Combo_State == ComboState.Attack1)
        // {
        //     animator.SetTrigger(attack1Hash);
        // }

        // if (current_Combo_State == ComboState.Attack2)
        // {
        //     animator.SetTrigger(attack2Hash);
        // }

        // if (current_Combo_State == ComboState.Attack3)
        // {
        //     animator.SetTrigger(attack3Hash);
        // }
        
        agent.stoppingDistance = 10;

    }

    private void ResetComboState()
    {
        if (activeTimerToReset)
        {
            current_Combo_State = ComboState.None;
            activeTimerToReset = false;
        }
    }

    //Assign Key Frame Animation Clip
    public void Attack1On()
    {
        leftHandAttackPoint.SetActive(true);
    }
    public void Attack2On()
    {
        rightHandAttackPoint.SetActive(true);
    }
    public void Attack3On()
    {
        RightLegAttackPoint.SetActive(true);
    }

    public void Attack1Off()
    {
        leftHandAttackPoint.SetActive(false);
    }
    public void Attack2Off()
    {
        rightHandAttackPoint.SetActive(false);
    }
    public void Attack3Off()
    {
        RightLegAttackPoint.SetActive(false);
    }


    //State Machine
    private void Idle()
    {
        ResetComboState();
        agent.SetDestination(transform.position);
        state = State.Chase;
    }

    private void Patrol()
    {

    }

    private void Attack(Vector3 playerPos)
    {
        agent.SetDestination(transform.position);
        Attacking();
    }

    private void Chase(Vector3 pos)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(playerPosition, out hit, agent.height * 2, 1);
        agent.SetDestination(hit.position);
    }

    private void Looking()
    {

    }

    private void HandleAnimation()
    {
        Vector3 horizontalVelocity = new Vector3(agent.velocity.x, 0, agent.velocity.z);
        float Velocity = horizontalVelocity.magnitude / agent.speed;
        if (Velocity > 0)
        {
            animator.SetFloat(velocityHash, Velocity);
        }
        else
        {
            float v = animator.GetFloat(velocityHash);
            v = Mathf.Lerp(v, -0.1f, 20f * Time.deltaTime);
            animator.SetFloat(velocityHash, v);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer & (1 << other.gameObject.layer)) != 0)
        {
            state = State.Attack;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((playerLayer & (1 << other.gameObject.layer)) != 0)
        {
            state = State.Chase;
        }
    }
}
