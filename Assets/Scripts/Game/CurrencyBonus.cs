using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class CurrencyBonus : MonoBehaviour
{
    public bool useMagnet = true, addForceOnAwake = true;

    public LayerMask layer;
    [SerializeField] private int point;
    public AudioClip audioClip;
    public float volumeScale = 1;
    private Rigidbody rb;
    private GameManager gameManager;
    private GameObjectPool gameObjectPool;
    private ObjectPoolerManager ObjectPoolerManager;
    private SoundManager soundManager;

    private void Awake() 
    {
        gameManager = GameManager.Instance;
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        soundManager = SoundManager.Instance;
        gameObjectPool = GetComponent<GameObjectPool>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() 
    {
        if(addForceOnAwake) {
            Vector3 dir = Random.insideUnitSphere.normalized;
            rb.AddForce(dir * 8f, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if((layer & (1 << other.gameObject.layer)) != 0) {
            ObjectPoolerManager.DeactiveObject(gameObjectPool);
            // soundManager.PlayOneShot(audioClip, volumeScale);
        }   
    }

    // private void OnDisable() 
    // {
    //     gameManager.UpdateMoney(point);
    // }
}
