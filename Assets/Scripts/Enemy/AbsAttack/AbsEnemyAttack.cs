using System;
using UnityEngine;


public abstract class AbsEnemyAttack : MonoBehaviour
{
    public enum Type
    {
        meleeAttack,
        rangedAttack

    }
    [SerializeField] private Type type;
    protected bool readyAttack = true;
    public abstract void HandleAttack();
    protected void AttackCoolDown()
    {
        readyAttack = true;
    }
}
