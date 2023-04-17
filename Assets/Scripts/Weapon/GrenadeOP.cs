using UnityEngine;

public class GrenadeOP : GameObjectPool
{
    [SerializeField] private LayerMask layerTarget;
    [SerializeField] private AttackType attackType;
    [SerializeField] private EffectObjectPool hitEffect;
    public float damage;
    private ObjectPoolerManager objectPoolerManager;

    //Missile
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform target;

    [SerializeField] private float speed = 2;
    [SerializeField] private float rotateSpeed = 100;

    private GameManager gameManager;
    private bool isStart;


    private void Awake()
    {
        gameManager = GameManager.Instance;
        objectPoolerManager = ObjectPoolerManager.Instance;
    }

    private void OnEnable()
    {
        gameManager.OnInitUiDone += StartGame;
    }

    private void OnDisable() {
        gameManager.OnInitUiDone -= StartGame;
    }

    private void StartGame()
    {
        isStart = true;
    }

    private void FixedUpdate()
    {
        if (isStart)
        {
            target = FindObjectOfType<EnemyBehaviour>().transform;
        }
        Vector3 direction = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z) - rb.position;
        direction.Normalize();
        rb.angularVelocity = -Vector3.Cross(direction, transform.forward) * rotateSpeed;
        rb.velocity = transform.forward * speed;
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
    }
}
