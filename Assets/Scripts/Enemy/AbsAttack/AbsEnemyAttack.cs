using System;
using UnityEngine;


public abstract class AbsEnemyAttack : MonoBehaviour
{
    [SerializeField] protected float attackCooldown;
    protected bool readyAttack = true;
    public abstract void HandleAttack();
    protected void AttackCoolDown()
    {
        readyAttack = true;
    }
}
