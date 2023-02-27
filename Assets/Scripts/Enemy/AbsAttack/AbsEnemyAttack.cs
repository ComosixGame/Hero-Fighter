using System;
using UnityEngine;

public abstract class AbsEnemyAttack : MonoBehaviour
{
    public bool readyAttack = true;
    public bool attacking { get; protected set; }
    public event Action OnAttackDone;

    public void Attack()
    {
        HandleAttack();
    }

    protected abstract void HandleAttack();
    protected void AttackCoolDown()
    {
        readyAttack = true;
    }

    protected void AttackDone()
    {
        attacking = false;
        OnAttackDone?.Invoke();
    }
}
