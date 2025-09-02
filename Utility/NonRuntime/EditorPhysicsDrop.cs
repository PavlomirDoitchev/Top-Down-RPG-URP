using UnityEngine;

[ExecuteInEditMode]
public class EditorPhysicsDrop : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasSimulated = false;

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            rb = gameObject.GetComponent<Rigidbody>();
            if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
        }
    }

    private void Update()
    {
        if (!Application.isPlaying && !hasSimulated)
        {
            // Run one physics step per editor frame
            Physics.Simulate(Time.fixedDeltaTime);
        }
    }

    public void Freeze()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            hasSimulated = true;
        }
    }
}
