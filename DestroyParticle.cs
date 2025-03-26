using System.Collections;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField] float timer;
    public void Start()
    {
        StartCoroutine(DestroyThis());
    }
    IEnumerator DestroyThis() 
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }
}
