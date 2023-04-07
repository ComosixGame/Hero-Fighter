using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserLine : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private Vector3 startPosition;
    private bool active;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if(active) {
            Vector3 startPoint = transform.TransformPoint(startPosition);

            lineRenderer.SetPosition(0, startPoint);
            RaycastHit hit;
            if (Physics.Raycast(startPoint, transform.forward, out hit, layer))
            {
                lineRenderer.SetPosition(1, hit.point);
            } else {
                lineRenderer.SetPosition(1, startPoint + transform.forward.normalized * 99999f);
            }
        }
    }

    public void SetActiveLaser(bool turnOn) {
        lineRenderer.enabled = turnOn;
        active = turnOn;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(startPosition), 0.03f);
    }
}
