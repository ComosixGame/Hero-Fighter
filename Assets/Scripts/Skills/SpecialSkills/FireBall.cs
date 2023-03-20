using UnityEngine;

public class FireBall : AbsSpecialSkill
{

    [SerializeField] private Vector3 firePosition;
    [SerializeField] private Bullet fireBallObject;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    private ObjectPoolerManager objectPooler; 

    override protected void Awake() {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
    }
    
    private void OnEnable()
    {
        PlayerHurtBox.OnHit += AccumulationEnergy;
    }

    protected override void Action()
    {
        animator.SetTrigger(SpecialSkilHash);
    }

    public void CastFireBall() {
        Bullet bullet = objectPooler.SpawnObject(fireBallObject, transform.TransformPoint(firePosition), transform.rotation) as Bullet;
        bullet.Fire(damage, speed);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(firePosition), 0.1f);
    }

}
