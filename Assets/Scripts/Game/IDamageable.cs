using UnityEngine;

public interface IDamageable
{
    public void TakeDamge(Vector3 hitPoint, Vector3 force, float damage = 0);
}
