using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class AbsBullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    
    public abstract void Fire(Vector3 direction);
}
