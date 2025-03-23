using Assets.Scripts.State_Machine.Player;
using Unity.Cinemachine;
using UnityEngine;

public class AssignPlayerToCamera : MonoBehaviour
{
    CinemachineCamera cam;

    void Start()
    {
        cam = GetComponent<CinemachineCamera>();
        Transform target = FindFirstVisibleObject();
        cam.Target.TrackingTarget = target;
    }
    Transform FindFirstVisibleObject() 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 100f); // Adjust radius
        foreach (var col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                return col.transform;
            }
        }
        return null;
    }

}
