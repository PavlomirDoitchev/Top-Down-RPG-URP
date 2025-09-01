using Assets.Scripts.Player;
using Assets.Scripts.State_Machine;
using Assets.Scripts.State_Machine.Player_State_Machine;
using StylizedWater3;
using UnityEngine;

namespace Assets.Scripts.Utility.EnvironmentFX
{

    public class WaterStateChecker : MonoBehaviour
    {
        [Header("References")]
        public Transform characterRoot;   // e.g., hips or chest bone
        public Transform feetTransform;   // to check when water is above feet

        [Header("Settings")]
        public float swimThreshold = 0.5f; 
        public bool debugLog;
        private PlayerStateMachine _playerStateMachine;

        void Start()
        {
            _playerStateMachine = PlayerManager.Instance.PlayerStateMachine;

        }

        void Update()
        {



            // 2. Get the aligned Y position
            float waterHeight = characterRoot.position.y;

            // 3. Calculate depth
            float feetY = feetTransform.position.y;
            float depth = waterHeight - feetY;

            if (debugLog)
            {
                Debug.Log($"WaterHeight={waterHeight:F2} | Feet={feetY:F2} | Depth={depth:F2}");
            }

            // 4. Transition condition
            if (depth >= swimThreshold)
            {
                // Transition to swimming state
                // animator.SetBool("Swimming", true);
                _playerStateMachine.ChangeState(new PlayerSwimmingState(_playerStateMachine));
            }
            else
            {
                // Transition to walking/wading state
                // animator.SetBool("Swimming", false);
                _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
            }

        }
    }
}

