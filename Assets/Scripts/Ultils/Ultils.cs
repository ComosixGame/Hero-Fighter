using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Ultils
{
    //return force for rigi add force
    public static Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 dis = target - origin;
        Vector3 disXZ = dis;
        disXZ.y = 0;

        float Sy = dis.y;
        float Sxz = disXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 reuslt = disXZ.normalized;
        reuslt *= Vxz;
        reuslt.y = Vy;

        return reuslt;
    }

    public static Vector3 RandomNavmeshLocation(Vector3 origin, float minRange = 5, float maxRange = 10)
    {
        //tính random điểm có thể đi trên nav mesh
        Vector3 finalPosition = Vector3.zero;
        float RandomDistance = Random.Range(minRange, maxRange);
        Vector3 randomDirection = Random.insideUnitSphere * RandomDistance;
        randomDirection += origin;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 4f, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public static Quaternion GetRotationLook(Vector3 directionMove, Vector3 forward)
    {
        if (directionMove.x > 0)
        {
            return Quaternion.LookRotation(Vector3.right);
        }
        else if (directionMove.x < 0)
        {
            return Quaternion.LookRotation(Vector3.left);
        }
        else
        {
            return Quaternion.LookRotation(forward);
        }
    }

    public static void ScrollTo(ScrollRect scrollRect, RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;
        Vector2 childLocalPosition = target.localPosition;
        Vector2 result = scrollRect.content.localPosition;
        result.x = 0 - (viewportLocalPosition.x + childLocalPosition.x);
        scrollRect.content.localPosition = result;
    }
}
