using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float damage;
    [SerializeField] private Rigidbody rb;
    private GameObjectPool gameObjectPool;
    private ObjectPoolerManager ObjectPoolerManager;

    private void Awake() 
    {
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        gameObjectPool = GetComponent<GameObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
