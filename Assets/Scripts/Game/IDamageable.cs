using UnityEngine;

public enum AttackType {
    light,
    heavy,
}

public interface IDamageable
{
    public void TakeDamgae(Vector3 hitPoint, float damage, AttackType attackType);
}
