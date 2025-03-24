using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;
namespace Assets.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        public PlayerStateMachine playerStateMachine { get; private set; }
        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else
                Instance = this;
            playerStateMachine = FindFirstObjectByType<PlayerStateMachine>();
        }
    }
}