using UnityEngine;
using UnityEngine.Events;

public class MeleeEnemyAttack : AbsEnemyAttack
{
    [SerializeField] private EnemyHurtBox[] hurtBoxes;
    private int attackHash, backwardHash;
    private Animator animator;
    private EnemyDamageable damageable;
    private GameManager gameManager;
    private EnemyBehaviour enemyBehaviour;
    [SerializeField] private float timeStepBack;
    private void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        damageable = GetComponent<EnemyDamageable>();

        attackHash = Animator.StringToHash("Attack");
        backwardHash = Animator.StringToHash("backward");
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    private void Start()
    {
        foreach (EnemyHurtBox hurtBox in hurtBoxes)
        {
            hurtBox.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if(attacking) {
            AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
            if (!animationState.IsName("Attacking"))
            {
                attacking = false;
                foreach (EnemyHurtBox hurtBox in hurtBoxes)
                {
                    hurtBox.gameObject.SetActive(false);
                }
            }

        }
    }

    protected override void Action()
    {
        animator.SetTrigger(attackHash);
        OnAction?.Invoke();
    }

    public void StartAttack(int index)
    {
        attacking = true;
        hurtBoxes[index].gameObject.SetActive(true);
        OnStartAttack?.Invoke();
    }

    public void EndAttack(int index)
    {
        attacking = false;
        hurtBoxes[index].gameObject.SetActive(false);
        animator.SetBool(backwardHash, true);
        Invoke("CancleStepBack", timeStepBack);
        OnEndAttack?.Invoke();
    }

    private void CancleStepBack()
    {
        animator.SetBool(backwardHash, false);
    }
}