using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogic;
    public void EnableWeapon() 
    {
        weaponLogic.SetActive(true);
    }

    public void DisableWeapon() 
    {
        weaponLogic.SetActive(false);
    }
}
