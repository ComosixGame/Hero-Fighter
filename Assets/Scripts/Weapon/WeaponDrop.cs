using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WeaponDrop : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originRotation;
    private Transform parent;
    private Rigidbody rb;

    private void Awake() {
        parent = transform.parent;
        originPosition = transform.localPosition;
        originRotation = transform.localRotation;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void Drop() {
        rb.isKinematic = false;
        transform.parent = null;
        Invoke("Hide", 5f);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        transform.position = originPosition;
        transform.rotation = originRotation;
        transform.SetParent(parent, false);
        rb.isKinematic = true;
    }
}
