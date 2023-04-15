using UnityEngine;

public class Missile : GameObjectPool
{
    [SerializeField] private LayerMask layerTarget;
    [SerializeField] private AttackType attackType;
    [SerializeField] private EffectObjectPool hitEffect;
    [SerializeField] private EffectObjectPool smokeEffect;
    [SerializeField] private Vector3 smokePosition;
    private ParticleSystem generatedMissileSmoke;

    public float damage;
    private ObjectPoolerManager objectPoolerManager;

    //Missile
    [Header("REFERENCES")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform target;

    [SerializeField] private float speed = 2;
    [SerializeField] private float rotateSpeed = 100;

    private bool flag;


    private void Awake()
    {
        objectPoolerManager = ObjectPoolerManager.Instance;
    }

    private void OnEnable()
    {

        target = FindObjectOfType<EnemyBehaviour>().transform;
        flag = true;
    }

    private void SpawnParticle()
    {
        flag = false;
        generatedMissileSmoke = objectPoolerManager.SpawnObject(smokeEffect, transform.TransformPoint(smokePosition)
        , transform.rotation).
        GetComponent<ParticleSystem>();
        generatedMissileSmoke.Play();
    }


    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z) - rb.position;
        direction.Normalize();
        rb.angularVelocity = -Vector3.Cross(direction, transform.forward) * rotateSpeed;
        rb.velocity = transform.forward * speed;

        if (flag)
        {
            SpawnParticle();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if ((layerTarget & (1 << other.gameObject.layer)) != 0)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                ContactPoint contact = other.contacts[0];
                objectPoolerManager.SpawnObject(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));
                Vector3 dirAttack = transform.position - other.transform.position;
                damageable.TakeDamgae(dirAttack.normalized, damage, attackType);
            }
            Destroy();
        }
    }

    private void Destroy()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        objectPoolerManager.DeactiveObject(this);
        generatedMissileSmoke.Stop();
    }
}
