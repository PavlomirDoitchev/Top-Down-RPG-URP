using Assets.Scripts.Combat_Logic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject endPoint;
    [SerializeField] GameObject platform;
    [SerializeField] float speed = 1f;
    //bool hasTakenDamage = false;
   
    private float lerpTime = 0f;
    private bool goingForward = true;

    void Update()
    {
        lerpTime += Time.deltaTime * speed;
        if (lerpTime > 1f)
        {
            lerpTime = 0f;
            goingForward = !goingForward;
        }

        if (goingForward)
            platform.transform.position = Vector3.Lerp(startPoint.transform.position, endPoint.transform.position, lerpTime);
        else
            platform.transform.position = Vector3.Lerp(endPoint.transform.position, startPoint.transform.position, lerpTime);
    }
    private void OnDrawGizmos()
    {
        if (startPoint != null && endPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPoint.transform.position, endPoint.transform.position);
            Gizmos.DrawSphere(startPoint.transform.position, 0.1f);
            Gizmos.DrawSphere(endPoint.transform.position, 0.1f);
        }
    }
   
}
