using UnityEngine;
using NHance.Assets.Scripts;
using NHance.Assets.Scripts.Items;
using NHance.Assets.Scripts.Enums;

public class EquipmentDemo : MonoBehaviour
{
    [Header("References")]
    public NHAvatar character;
    public NHItem helmetItem; 
    public NHItem swordItem;  

    void Start()
    {
        if (character == null)
        {
            Debug.LogError("NHAvatar reference is missing!");
            return;
        }

        
        //character.ClearItems();
        character.Compile(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            EquipItem(helmetItem);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipItem(ItemTypeEnum.Helmet);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            EquipItem(swordItem);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            UnequipItem(ItemTypeEnum.WeaponR);
        }
    }

    void EquipItem(NHItem item)
    {
        if (item == null) return;

        character.SetItem(item);   
        character.Compile();      
        Debug.Log($"Equipped {item.name}");
    }

    void UnequipItem(ItemTypeEnum type)
    {
        character.ClearItems(type);  
        character.Compile();         
        Debug.Log($"Unequipped {type}");
    }
}
