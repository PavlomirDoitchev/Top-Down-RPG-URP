using UnityEngine;

public class HideRoof : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
            GetComponentInChildren<MeshRenderer>().enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("MyOutlines"))
            GetComponentInChildren<MeshRenderer>().enabled = false;
    }
}
