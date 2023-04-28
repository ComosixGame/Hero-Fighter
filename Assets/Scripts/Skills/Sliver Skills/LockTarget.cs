using System.Linq;
using UnityEngine;

public class LockTarget : PlayerSkill
{
    [SerializeField] private Vector3 grenadePosition;
    [SerializeField] private GameObjectPool grenadeObjectPool;
    // [SerializeField] private SkillState skillState;
    private Transform target;
    private ObjectPoolerManager objectPooler;
    private GameObjectPool gameObjectPool;

    override protected void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
        currentLevel = skillState.level;
    }

    // Start is called before the first frame update
    void Start()
    {
        energy = skillLevels[currentLevel].energy;
        maxCoolDownTime = skillLevels[currentLevel].maxCoolDownTime;
        // grenadeObjectPool.GetComponent<GrenadeOP>().damage = skillLevels[currentLevel].damage;
    }

    public void CastGrenade()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f, LayerMask.NameToLayer("Enemy"));
        Collider closest = hitColliders.OrderBy(collider => Vector3.Distance(collider.transform.position, transform.position)).First();
        target = closest.transform;
        Debug.Log(closest);
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if(target != null) {
            Gizmos.DrawSphere(target.position, 1f);
        }
    }
}
