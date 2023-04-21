using System.Collections;
using UnityEngine;

public class GunEnemyAttack : AbsEnemyAttack
{
    [SerializeField] private Vector3 offsetCenter;
    [SerializeField] private float closeRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private EnemyHurtBox hurtBox;
    [SerializeField] private Transform gun;
    private int attackHash, attack2Hash;
    private float timer;
    private bool readyCloseAttack = true;
    private bool closeAttacking;
    private ObjectPoolerManager ObjectPoolerManager;
    private Animator animator;
    private GameManager gameManager;
    private ObjectPoolerManager objectPooler;
    private EnemyBehaviour behaviour;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        objectPooler = ObjectPoolerManager.Instance;
        behaviour = GetComponent<EnemyBehaviour>();
        animator = GetComponent<Animator>();
        attackHash = Animator.StringToHash("Attack");
        attack2Hash = Animator.StringToHash("Attack 2");
        ObjectPoolerManager = ObjectPoolerManager.Instance;
    }

    private void Start()
    {
        hurtBox.gameObject.SetActive(false);
        StartCoroutine(CheckPlayerInCloseRange());
    }

    private void Update()
    {
        if (attacking && !closeAttacking)
        {
            Vector3 dirLook = gameManager.player.position - transform.position;
            dirLook.y = 0;
            Quaternion rot = Quaternion.LookRotation(dirLook) * Quaternion.FromToRotation(gun.forward, transform.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5f * Time.deltaTime);

            if (Vector3.Angle(gun.forward, dirLook) <= 10f)
            {
                timer += Time.deltaTime;
                if (timer >= 2f)
                {
                    timer = 0;
                    animator.SetTrigger(attackHash);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (closeAttacking)
        {
            AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
            if (!animationState.IsName("Attacking 2"))
            {
                closeAttacking = false;
                hurtBox.gameObject.SetActive(false);
            }

            if (!animationState.IsName("Attacking 2"))
            {
                closeAttacking = false;
                hurtBox.gameObject.SetActive(false);
            }

        }

        if (attacking)
        {
            AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
            if (!animationState.IsName("Move"))
            {
                timer = 0;
                EndFiring();
            }
        }
    }

    private IEnumerator CheckPlayerInCloseRange()
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + offsetCenter, closeRange, playerLayer);
            if (hitColliders.Length > 0)
            {
                if (!closeAttacking && readyCloseAttack)
                {
                    readyCloseAttack = false;
                    behaviour.lockRotation = true;
                    timer = 0;
                    animator.SetTrigger(attack2Hash);
                    Invoke("CoolDownCloseAttack", 3f);
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }


    protected override void Action()
    {
        behaviour.lockRotation = false;
        attacking = true;
        OnAction?.Invoke();
    }

    public void StartFiring()
    {
        OnStartAttack?.Invoke();
    }

    public void EndFiring()
    {
        attacking = false;
        behaviour.lockRotation = true;
        OnEndAttack?.Invoke();
    }

    public void StartCloseAttack()
    {
        closeAttacking = true;
        hurtBox.gameObject.SetActive(true);
    }

    public void EndCloseAttack()
    {
        closeAttacking = false;
        hurtBox.gameObject.SetActive(false);
    }

    private void CoolDownCloseAttack()
    {
        readyCloseAttack = true;
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + offsetCenter, closeRange);
    }
#endif
}
