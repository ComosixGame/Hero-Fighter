using System;
using UnityEngine;


public abstract class AbsEnemyAttack : MonoBehaviour
{
    [SerializeField] protected float coolDownTime;
    protected bool readyAttack = true;
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
