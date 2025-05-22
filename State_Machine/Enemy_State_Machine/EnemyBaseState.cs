using Assets.Scripts.State_Machine.Player_State_Machine;
using static Assets.Scripts.State_Machine.Player_State_Machine.PlayerBaseState;
using UnityEngine;
using Assets.Scripts.Player;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine _enemyStateMachine;
        protected MeleeWeapon _meleeWeapon;
        protected readonly int activeLayer = 3;
        protected readonly int inactiveLayer = 7;
        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
             this._enemyStateMachine = stateMachine;
        }
        public override void EnterState() 
        {
            base.EnterState();
            InitializeWeapon();
            SetWeaponActive(false);
            _meleeWeapon.ClearHitEnemies();
        }
        protected void SetWeaponActive(bool isActive)
        {
            _meleeWeapon.gameObject.layer = isActive ? activeLayer : inactiveLayer;
        }
        protected void InitializeWeapon()
        {
            if (_enemyStateMachine.EquippedWeapon != null)
            {
                _meleeWeapon = _enemyStateMachine.EquippedWeapon.GetComponentInChildren<MeleeWeapon>();
            }

            if (_meleeWeapon == null)
            {
                Debug.LogError("No weapon equipped in state: " + this.GetType().Name);
            }
        }
        protected void RotateToPlayer(float deltaTime)
        {
            Vector3 direction = PlayerManager.Instance.PlayerStateMachine.transform.position - _enemyStateMachine.transform.position;
            direction.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _enemyStateMachine.transform.rotation = Quaternion.Slerp(_enemyStateMachine.transform.rotation, targetRotation, deltaTime * 10);
        }
        protected void ResetAnimationSpeed()
        {
            _enemyStateMachine.Animator.speed = 1f;
        }
        private bool CriticalStrikeSuccessfull()
        {
            float rollForCrit = Random.Range(0f, 1f);
            if (_enemyStateMachine.CriticalChance >= rollForCrit)
            {
                Debug.Log("Enemy Critical!");
                return true;
            }
            return false;
        }
        
        protected void SetEnemyDamage()
        {
            if (CriticalStrikeSuccessfull())
            {
                _meleeWeapon.MeleeWeaponDamage
                  (Random.Range(_meleeWeapon.EquippedWeaponDataSO.minDamage, _meleeWeapon.EquippedWeaponDataSO.maxDamage + 1),
                  _enemyStateMachine.CriticalModifier, 0);
            }
            else
            {
                _meleeWeapon.MeleeWeaponDamage
                     (Random.Range(_meleeWeapon.EquippedWeaponDataSO.minDamage, _meleeWeapon.EquippedWeaponDataSO.maxDamage + 1),
                     1, 0);
            }
        }
    }
}
