using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : AbsPlayerSkill
{
    [SerializeField] private Vector3 laserPosition;
    [SerializeField] private EffectObjectPool laserObject;
    [SerializeField] private float damage;
    private bool casting;
    private ParticleSystem generatedLaserObject;
    private ObjectPoolerManager objectPooler;

    override protected void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Action()
    {
        animator.SetTrigger(PlayerSkillHash);
    }

    public void CastLaser()
    {
        casting =  true;
        generatedLaserObject = objectPooler.SpawnObject(
                laserObject,
                transform.TransformPoint(laserPosition),
                transform.rotation
            ).GetComponent<ParticleSystem>();
        generatedLaserObject.Play();
    }

    public void CastLaserDone()
    {
        casting = false;
        generatedLaserObject.Stop();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.TransformPoint(laserPosition), 0.1f);
    }
}
