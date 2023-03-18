using UnityEngine;

public class Bullet : GameObjectPool
{
    [SerializeField] private AttackType attackType;
    [SerializeField] private LayerMask layerTarget;
    private float speed;
    [SerializeField] private Rigidbody rb;
    private bool fired, hit;
    private float damage;
    private ObjectPoolerManager ObjectPoolerManager;

    private void Awake() 
    {
        ObjectPoolerManager = ObjectPoolerManager.Instance;
    }

    private void FixedUpdate() {
        if(fired) {
            rb.velocity = transform.forward.normalized * speed ;
        }
    }

    public void Fire(float damage, float speed) {
        fired = true;
        this.damage = damage; 
        this.speed = speed;
    }

    private void OnCollisionEnter(Collision other) {
        if((layerTarget & (1 << other.gameObject.layer)) != 0) {
            ContactPoint contact = other.contacts[0];
            if(other.gameObject.TryGetComponent(out IDamageable damageable)) {
                damageable.TakeDamgae(contact.point, damage, attackType);
            }
            Destroy();
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
