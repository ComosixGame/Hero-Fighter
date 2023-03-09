using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [Header ("Effects")]
    public GameObject dustEffectLand;
    public GameObject hitEffect;
    [HideInInspector]

    //Show Hit effect
    public void ShowHitEffect()
    {
        GameObject.Instantiate(hitEffect, transform.position +Vector3.up* 0.2f, Quaternion.identity);
    }

    //Show Land effect
    public void ShowDustEffect()
    {
        GameObject.Instantiate(dustEffectLand, transform.position + Vector3.down*1.2f, Quaternion.identity);
    }

}
