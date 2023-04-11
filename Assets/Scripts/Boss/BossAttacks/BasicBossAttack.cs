using UnityEngine;

public class BasicBossAttack : AbsBossAttack
{
    [SerializeField] private EnemyHurtBox hurtBox;

    private void OnEnable()
    {
        OnCancelCloseAttack += () => {
            hurtBox.gameObject.SetActive(false);
        };
    }

    private void Start()
    {
        hurtBox.gameObject.SetActive(false);
    }

    protected override void Action()
    {
        animator.SetTrigger(attackHash);
    }

    public void StartAttack()
    {
        if (attacking)
        {
            hurtBox.gameObject.SetActive(true);
        }
    }

    public void EndAttack()
    {
        if (attacking)
        {
            hurtBox.gameObject.SetActive(false);
            attacking = false;
        }
    }
}
