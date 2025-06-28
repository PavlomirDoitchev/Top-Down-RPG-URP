using UnityEngine;

namespace Assets.Scripts.Player.Abilities
{
    public class RotateHammer : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 100f;

        private void Update()
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, transform.localRotation.eulerAngles.z + rotationSpeed * Time.deltaTime);
        }
        
    }
}
