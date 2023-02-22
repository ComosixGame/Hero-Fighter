using System;
using UnityEngine;

public class EnemyDamageable : MonoBehaviour, IDamageable
{
    public event Action<float> OnTakeDamage;

    public void TakeDamgae(Vector3 hitPoint, float damage = 0)
    {
        OnTakeDamage?.Invoke(damage);
    }
    
}
