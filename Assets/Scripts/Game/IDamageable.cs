using UnityEngine;

public enum AttackType {
    light,
    heavy,
}

public interface IDamageable
{
    public void TakeDamgae(Vector3 hitPoint, Vector3 dirAttack, float damage, AttackType attackType);
    public void KnockDownEffect();
}
