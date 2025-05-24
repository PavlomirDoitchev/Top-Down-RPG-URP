using UnityEngine;

public class EnemyForceReceiver : MonoBehaviour
{
    [SerializeField] private float dragTime = 0.1f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float inAirDrag = .05f;
    [SerializeField] private float maxFallSpeed = -50f;
    private float verticalVelocity;
    private Vector3 impact;
    private Vector3 dampingVelocity;
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;
    public bool isGrounded;
    uint layerMask = 1 << 6; // the ground is on layer 6
    private void OnCollisionEnter(Collision collision)
    {
        if ((layerMask & (1 << collision.gameObject.layer)) != 0)
        {
            isGrounded = true;
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if ((layerMask & (1 << other.gameObject.layer)) != 0)
        {
            isGrounded = true;
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (verticalVelocity < 0f && isGrounded)
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
    }
    public void AddForce(Vector3 force)
    {
        impact += force;
    }
    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}
