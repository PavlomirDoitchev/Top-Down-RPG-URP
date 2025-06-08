using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float dragTime = 0.1f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float inAirDrag = .05f;
    [SerializeField] private float maxFallSpeed = -50f;
    [SerializeField] private NavMeshAgent agent;
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
            verticalVelocity += Physics.gravity.y * fallMultiplier * Time.deltaTime;
            verticalVelocity *= (1 - inAirDrag);
            verticalVelocity = Mathf.Max(verticalVelocity, maxFallSpeed);
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, dragTime);
        if (impact == Vector3.zero && agent != null)
        {
            agent.enabled = true;
        }
    }
    public void AddForce(Vector3 force)
    {
        impact += force;
        if(agent != null)
            agent.enabled = false; 
    }
    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
        if (agent != null)
            agent.enabled = false; 
    }
}
