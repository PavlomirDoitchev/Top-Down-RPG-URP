using UnityEngine;

public class HideRoof : MonoBehaviour
{
    [SerializeField] private GameObject roof;
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
