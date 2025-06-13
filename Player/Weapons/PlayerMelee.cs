using UnityEngine;
using DamageNumbersPro;
using Assets.Scripts.Player;
public class PlayerMelee : MonoBehaviour
{
    [Header("-----References-----")]
    public GameObject[] damageColliders;
    PlayerManager _playerManager;    
    private void Start()
    {
        _playerManager = PlayerManager.Instance;
	}

	#region Animation Events Functions
	/// <summary>
	/// Specify which collider to activate or deactivate based on the index. 
	/// </summary>
	/// <param name="isActive"></param>
	/// <param name="index"></param>
	public void SetWeaponActive(bool isActive, int index)
    {
        if (isActive)
            damageColliders[index].gameObject.layer = 13;
        else if (!isActive)
            damageColliders[index].gameObject.layer = 3;
    }
   
    
	#endregion
}
