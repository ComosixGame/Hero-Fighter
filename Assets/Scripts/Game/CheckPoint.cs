using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if ((layer & (1 << other.gameObject.layer)) != 0)
        {
            gameManager.CheckedPoint();   
            gameObject.SetActive(false);
        }
    }
}
