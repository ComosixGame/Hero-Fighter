using UnityEngine;

public class MagicBossAttack : AbsBossAttack
{
    [SerializeField] private AbsMagic magic;
    [SerializeField] private Vector3 firePosition;
    private Vector3 dirAttack;
    private ObjectPoolerManager objectPooler;
    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
        gameManager =  GameManager.Instance;
    }

    protected override void Action()
    {
        animator.SetTrigger(attackHash);
        dirAttack = gameManager.player.position - transform.TransformPoint(firePosition);
    }

    public void StartCast() {
        if(attacking) {
            GameObjectPool magicEffect = objectPooler.SpawnObject(
                magic.GetComponent<EffectObjectPool>(),
                transform.TransformPoint(firePosition),
                Quaternion.LookRotation(dirAttack));
            
            magicEffect.GetComponent<AbsMagic>().Cast();
        }
    }

    public void EndCast() {
        if(attacking) {
            attacking = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1,0,0, 0.8f);
        Gizmos.DrawSphere(transform.TransformPoint(firePosition), 0.2f);
    }
#endif
}
