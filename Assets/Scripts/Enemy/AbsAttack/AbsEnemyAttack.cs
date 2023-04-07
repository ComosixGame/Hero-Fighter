using UnityEngine;
using UnityEngine.Events;


public abstract class AbsEnemyAttack : MonoBehaviour
{
    [SerializeField] protected float coolDownTime;
    protected bool attacking;
    protected bool readyAttack = true;
    public UnityEvent OnAction;
    public UnityEvent OnStartAttack;
    public UnityEvent OnEndAttack;

    public void Attack()
    {
        if (readyAttack)
        {
            readyAttack = false;
            Action();
            Invoke("AttackCoolDown", coolDownTime);
        }
    }
    protected abstract void Action();
    protected void AttackCoolDown()
    {
        readyAttack = true;
    }
}
