using UnityEngine;

public class PlayerTestScript : MonoBehaviour
{
    [SerializeField] float movementSpeed = 25f;

    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
        }
    }
}
