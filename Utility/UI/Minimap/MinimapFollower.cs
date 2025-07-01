using UnityEngine;

public class MinimapFollower : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float heightOffset = 15f;

    //[SerializeField] Transform playerArrow;

    void LateUpdate()
    {
        if(player == null)
            return;
        transform.position = new Vector3(player.position.x, player.position.y + heightOffset, player.position.z);
       // playerArrow.rotation = Quaternion.Euler(0, 0, -player.eulerAngles.y);
    }
}
