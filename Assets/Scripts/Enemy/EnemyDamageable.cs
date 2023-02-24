using System;
using UnityEngine;

public class EnemyDamageable : MonoBehaviour, IDamageable
{
    public event Action<Vector3, float, AttackType> OnTakeDamage;

    public void TakeDamgae(Vector3 hitPoint, float damage, AttackType attackType)
    {
        OnTakeDamage?.Invoke(hitPoint, damage, attackType);
    }
    
}
