using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    [Header ("Effect")]
    public GameObject hitEffect;

    public void ShowHitEffect(Vector3 pos)
    {
        // GameObject.Instantiate(hitEffect, transform.position + Vector3.up*1.5f, Quaternion.identity);
        GameObject.Instantiate(hitEffect, pos, Quaternion.identity);

    }
}
