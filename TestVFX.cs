using UnityEngine;
using UnityEngine.VFX;

public class TestVFX : MonoBehaviour
{
    VisualEffect effect;
    private void Start()
    {
        effect = GetComponent<VisualEffect>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            effect.Play();
        }
    }
}
