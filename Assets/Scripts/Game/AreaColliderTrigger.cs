using UnityEngine;

public class AreaColliderTrigger : MonoBehaviour
{
    public MapGeneration mapGeneration;
    public LayerMask playerLayer;
    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer & 1 << other.gameObject.layer) != 0)
        {
            if(mapGeneration != null)
            {
                mapGeneration.NextWave();
                Destroy(gameObject);
            }
            
        }
    }
}
