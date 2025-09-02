using Assets.Scripts.State_Machine.Mount_State_Machine.States;
using Assets.Scripts.State_Machine.Mount_State_Machine;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerMountedState : PlayerBaseState
    {
        private GameObject mountInstance;

        private readonly GameObject mountPrefab = Resources.Load<GameObject>("Prefabs/MountHorse");

        public PlayerMountedState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Entering Player Mounted State");
            ResetAnimationSpeed();
            _playerStateMachine.CharacterController.enabled = false;
            mountInstance = Object.Instantiate(mountPrefab, _playerStateMachine.transform.position, Quaternion.identity);

            Transform seat = mountInstance.transform.Find("SeatSocket");
            if (seat != null)
            {
                _playerStateMachine.transform.SetParent(seat);
                _playerStateMachine.transform.SetLocalPositionAndRotation(new Vector3(0, 0, -.5f), Quaternion.identity);
            }

            _playerStateMachine.Animator.CrossFadeInFixedTime("Mount", 0.1f);
            _playerStateMachine.EquippedWeapon.SetActive(false);
        }

        public override void UpdateState(float deltaTime)
        {
            if (_playerStateMachine.InputManager.PlayerDismountInput())
            {
                _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
            }
        }

        public override void ExitState()
        {
            //TODO: change this to only affect player mesh
            ResetAnimationSpeed();
            _playerStateMachine.CharacterController.enabled = true;
            _playerStateMachine.EquippedWeapon.SetActive(true);
            if (mountInstance != null)
            {
                _playerStateMachine.transform.SetParent(null);
                Object.Destroy(mountInstance);
            }
        }
    }
}
