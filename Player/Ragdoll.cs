using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    //[SerializeField] private CharacterController controller;
    private Collider[] allColliders;
    private Rigidbody[] allRigidbodies;
    private void Awake()
    {
        allColliders = GetComponentsInChildren<Collider>(true);
        allRigidbodies = GetComponentsInChildren<Rigidbody>(true);
        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagdoll) 
    {
        foreach (var collider in allColliders) 
        {
            if(collider.gameObject.CompareTag("Ragdoll"))
                collider.enabled = isRagdoll;
        }
        foreach (var rb in allRigidbodies)
        {
            if (rb.gameObject.CompareTag("Ragdoll")) 
            {
                rb.isKinematic = isRagdoll;
            rb.useGravity = isRagdoll;
            }
        }
        animator.enabled = isRagdoll;
        //controller.enabled = isRagdoll;
    }
}
