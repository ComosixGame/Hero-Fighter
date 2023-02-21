using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public LayerMask targetLayer;
    private void OnTriggerEnter(Collider other) 
    {
        if ((targetLayer & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("Hit Player");
        }
    }
}
