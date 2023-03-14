using UnityEngine;

public class AreaColliderTrigger : MonoBehaviour
{
    [SerializeField] private MapGeneration mapGeneration;
    [SerializeField] private LayerMask playerLayer;

    private void Awake() {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer & 1 << other.gameObject.layer) != 0)
        {
            if(mapGeneration != null)
            {
                mapGeneration.NextWave();
                gameObject.SetActive(false);
            }
            
        }
    }
}
