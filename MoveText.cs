using Assets.Scripts.Player;
using UnityEngine;

public class MoveText : MonoBehaviour
{
    
    private void Update()
    {
        gameObject.transform.Translate(Vector3.up * Time.deltaTime);
    }
}
