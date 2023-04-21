using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Wall : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.TryGetComponent(out Animator animator)) {
            Debug.Log("pk");
            animator.applyRootMotion = false;
        }
    }
}
