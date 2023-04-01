using UnityEngine;

public enum AttackType {
    light,
    heavy,
    continuous,
}

public interface IDamageable
{
    public void TakeDamgae(Vector3 dirAttack, float damage, AttackType attackType);
    public void KnockDownEffect();
}
