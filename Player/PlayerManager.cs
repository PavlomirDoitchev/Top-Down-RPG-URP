using Assets.Scripts.State_Machine.Player_State_Machine;
using Assets.Scripts.Player.Inventory;  
using UnityEngine;
namespace Assets.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        public PlayerStateMachine playerStateMachine { get; private set; }
        public PlayerInventory inventory { get; private set; }
        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else
                Instance = this;
            playerStateMachine = FindFirstObjectByType<PlayerStateMachine>();
            inventory = FindFirstObjectByType<PlayerInventory>();
        }
    }
}