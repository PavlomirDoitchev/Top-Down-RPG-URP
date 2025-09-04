using UnityEngine;
using NHance.Assets.Scripts.Items;
using NHance.Assets.Scripts.Enums;
using System.Collections.Generic;
using NHance.Assets.Scripts;

public class SimpleEquipmentManager : MonoBehaviour
{
    [Header("References")]
    public Transform rootBone;              // Skeleton root
    public List<BodypartSlot> bodySlots;    // Naked meshes (torso, legs, etc.)
    public List<SocketSlot> socketSlots;    // Socket mapping (hand_r, hand_l, head, etc.)
    public List<ItemTypeToSocket> itemToSocket; // Maps item types to sockets

    private Dictionary<ItemTypeEnum, GameObject> equippedItems = new Dictionary<ItemTypeEnum, GameObject>();

    /// <summary>
    /// Equip an NHItem prefab
    /// </summary>
    public void Equip(NHItem item)
    {
        if (item == null) return;

        // Remove existing item of same type
        Unequip(item.Type);

        // Find parent socket
        Transform parent = rootBone;
        var socketType = itemToSocket.Find(m => m.itemType == item.Type);
        if (socketType != null)
        {
            var socket = socketSlots.Find(s => s.type == socketType.socketType);
            if (socket != null && socket.transform != null)
                parent = socket.transform;
        }

        // Instantiate
        GameObject instance = Instantiate(item.gameObject, parent);
        var equipment = instance.AddComponent<Equipment>();
        equipment.Target = rootBone; // Handles bone remapping

        // Apply socket offsets (position/rotation)
        if (socketType != null)
        {
            var socket = socketSlots.Find(s => s.type == socketType.socketType);
            if (socket != null)
            {
                instance.transform.localPosition = socket.localPosition;
                instance.transform.localRotation = Quaternion.Euler(socket.localRotation);
            }
        }

        // Hide covered body parts
        foreach (var part in item.GetPartByType())
        {
            var slot = bodySlots.Find(s => s.type == part);
            if (slot != null && slot.renderer != null)
                slot.renderer.enabled = false;
        }

        equippedItems[item.Type] = instance;
    }

    /// <summary>
    /// Unequip by type
    /// </summary>
    public void Unequip(ItemTypeEnum type)
    {
        if (equippedItems.TryGetValue(type, out GameObject itemGO))
        {
            var nhItem = itemGO.GetComponent<NHItem>();
            if (nhItem != null)
            {
                // Restore hidden body parts
                foreach (var part in nhItem.GetPartByType())
                {
                    var slot = bodySlots.Find(s => s.type == part);
                    if (slot != null && slot.renderer != null)
                        slot.renderer.enabled = true;
                }
            }

            DestroyImmediate(itemGO);
            equippedItems.Remove(type);
        }
    }

    /// <summary>
    /// Unequip everything
    /// </summary>
    public void ClearAll()
    {
        foreach (var kv in equippedItems)
        {
            if (kv.Value != null)
                DestroyImmediate(kv.Value);
        }
        equippedItems.Clear();

        // Re-enable all body parts
        foreach (var slot in bodySlots)
        {
            if (slot.renderer != null)
                slot.renderer.enabled = true;
        }
    }
}

[System.Serializable]
public class BodypartSlot
{
    public TargetBodyparts type;
    public SkinnedMeshRenderer renderer;
}

[System.Serializable]
public class SocketSlot
{
    public BoneType type;         // e.g. HandR, HandL, Head, Spine
    public Transform transform;   // Reference in the skeleton
    public Vector3 localPosition; // Offset relative to socket
    public Vector3 localRotation; // Euler offset relative to socket
}

[System.Serializable]
public class ItemTypeToSocket
{
    public ItemTypeEnum itemType; // e.g. WeaponR, Helmet
    public BoneType socketType;   // Which bone/socket it belongs to
}
