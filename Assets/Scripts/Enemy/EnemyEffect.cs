using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    [Header ("Effect")]
    public GameObject hitEffect;

    public void ShowHitEffect()
    {
        GameObject.Instantiate(hitEffect, transform.position + Vector3.up*1.5f, Quaternion.identity);
    }
}
