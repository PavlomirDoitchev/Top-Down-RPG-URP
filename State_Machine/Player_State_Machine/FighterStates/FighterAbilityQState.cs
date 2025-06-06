using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;
namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class FighterAbilityQState : PlayerBaseState
    {
        //private bool rotationLocked = false;
        private Vector3 force;
        private Coroutine qCoroutine;
        int cost = 10;
        public FighterAbilityQState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            //Debug.Log("Q Ability State");
            int rank = _playerStateMachine.QAbilityRank;
            _playerStateMachine.Animator.speed = _playerStateMachine.PlayerStats.AttackSpeed;
            _playerStateMachine.Animator.Play("ARPG_Dual_Wield_Attack_Heavy1");
            SetMeleeDamage(rank, AbilityType.AbilityQ, PlayerStatType.Strength);
            force = _playerStateMachine.transform.forward * _playerStateMachine.qAbilityData[rank].force;
            qCoroutine = _playerStateMachine.StartCoroutine(QAbilityRoutine());
        }
        public override void UpdateState(float deltaTime)
        {
            PlayerMove(deltaTime);
        }
        //public override void UpdateState(float deltaTime)
        //{
        //    Move(deltaTime);
        //    if (!rotationLocked)
        //    {
        //        RotateToMouse(deltaTime);

        //    }
        //    else
        //    {
        //        LockRotation();
        //    }
        //    if (!rotationLocked && _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
        //    {
        //        SetWeaponActive(true);
        //        _playerStateMachine.ForceReceiver.AddForce(force);
        //        rotationLocked = true;
        //        SetCurrentRotation();
        //    }
        //    
        //if (_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        //{
        //    _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
        //}
        // }



        public override void ExitState()
        {
            _playerStateMachine.Animator.StopPlayback();
            ResetAnimationSpeed();
        }
        private IEnumerator QAbilityRoutine()
        {
            while (_playerStateMachine.InputManager.IsUsingAbility_Q && _playerStateMachine.PlayerStats.GetCurrentResource() >= cost)
            {
                SetWeaponActive(true);
                _playerStateMachine.PlayerStats.UseResource(cost);
                meleeWeapon.ClearHitEnemies();

                yield return new WaitForSeconds(_playerStateMachine.PlayerStats.AttackSpeed);
            }
            _playerStateMachine.ChangeState(new FighterLocomotionState(_playerStateMachine));
        }
    }
}