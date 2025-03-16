using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float dragTime = 0.1f;
    private float verticalVelocity;
    private Vector3 impact;
    private Vector3 dampingVelocity;
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;
    private void Update()
    {
        if (verticalVelocity < 0f && characterController.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, dragTime);
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }
}
