using System;
using UnityEngine;


public abstract class AbsEnemyAttack : MonoBehaviour
{
    [SerializeField] protected float attackCooldown;
    protected bool readyAttack = true;
    public abstract void HandleAttack();
    //Sometime enemy die hurtBox still take dame player
    public abstract void CancleAttack();
    protected void AttackCoolDown()
    {
        readyAttack = true;
    }
}
