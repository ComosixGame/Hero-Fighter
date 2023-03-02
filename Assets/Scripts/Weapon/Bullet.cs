using UnityEngine;

public class Bullet : GameObjectPool
{
    [SerializeField] private AttackType attackType;
    [SerializeField] private LayerMask layerTarget;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    private bool fired, hit;
    private Vector3 direction;
    private float damage;
    private ObjectPoolerManager ObjectPoolerManager;

    private void Awake() 
    {
        ObjectPoolerManager = ObjectPoolerManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate() {
        if(fired) {
            rb.velocity = transform.forward.normalized * speed ;
        }
    }

    public void Fire(float damage, float speed, Vector3 direction) {
        fired = true;
        this.damage = damage; 
        this.speed = speed;
        this.direction = direction;
    }

    private void OnCollisionEnter(Collision other) {
        if((layerTarget & (1 << other.gameObject.layer)) != 0) {
            ContactPoint contact = other.contacts[0];
            Destroy();
            if(other.gameObject.TryGetComponent(out IDamageable damageable)) {
                damageable.TakeDamgae(contact.point, 0, attackType);
            }
        }
    }

    private void Destroy() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        fired = false;
        hit = false;
        ObjectPoolerManager.DeactiveObject(this);
    }
}
