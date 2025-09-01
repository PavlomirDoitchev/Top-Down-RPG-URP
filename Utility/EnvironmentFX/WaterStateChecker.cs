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
        public Transform feetTransform;              
        public ForceReceiver forceReceiver;          

        [Header("Settings")]
        public float swimThreshold = 0.5f;           
        public float floatStrength = 9f;             
        public bool debugLog;
        private float smoothVelocity;
        private PlayerStateMachine _playerStateMachine;
        private bool isSwimming;

        void Start()
        {
            _playerStateMachine = PlayerManager.Instance.PlayerStateMachine;
        }

        void Update()
        {
            WaterObject water = WaterObject.Find(feetTransform.position, false);

            if (water != null)
            {
                float waterHeight = water.transform.position.y;  
                float feetY = feetTransform.position.y;
                float depth = waterHeight - feetY;

                if (debugLog)
                    Debug.Log($"WaterHeight={waterHeight:F2} | Feet={feetY:F2} | Depth={depth:F2}");

                if (depth >= swimThreshold && !isSwimming)
                {
                    isSwimming = true;
                    _playerStateMachine.ChangeState(new PlayerSwimmingState(_playerStateMachine));
                }
                else if (depth < swimThreshold && isSwimming)
                {
                    isSwimming = false;
                    _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
                }

                if (isSwimming)
                {
                    float verticalDelta = waterHeight - feetY;
                    float targetVelocity = verticalDelta * floatStrength;

                    forceReceiver.verticalVelocity = Mathf.SmoothDamp(
                        forceReceiver.verticalVelocity,
                        targetVelocity,
                        ref smoothVelocity,
                        0.1f 
                    );
                }
            }
            else
            {
                if (isSwimming)
                {
                    isSwimming = false;
                    _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
                }
            }
        }
    }
}
