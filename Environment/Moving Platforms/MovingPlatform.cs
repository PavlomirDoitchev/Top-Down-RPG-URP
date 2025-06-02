using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject endPoint;
    [SerializeField] GameObject platform;
    [SerializeField] float speed = 1f;  
    void Update()
    {
        platform.transform.localPosition = Vector3.Lerp(
            startPoint.transform.position,
            endPoint.transform.position,
            Mathf.PingPong(Time.time, speed)
        );
    }
   
}
