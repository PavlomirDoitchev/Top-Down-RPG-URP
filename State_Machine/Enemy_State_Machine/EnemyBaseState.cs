using Assets.Scripts.State_Machine.Player_State_Machine;
using static Assets.Scripts.State_Machine.Player_State_Machine.PlayerBaseState;
using UnityEngine;
using Assets.Scripts.Player;
using Assets.Scripts.Enemies;

namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine _enemyStateMachine;
        //protected MeleeWeapon _meleeWeapon;
        protected EnemyMelee _enemyMelee;
        //protected readonly int activeLayer = 3;
        //protected readonly int inactiveLayer = 7;
        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
             this._enemyStateMachine = stateMachine;
        }
        public override void EnterState() 
        {
            base.EnterState();
            InitializeWeapon();
            //SetWeaponActive(false);
            SetEnemyLayerDuringAttack("Enemy");
            _enemyMelee.EnemyClearHitEnemies();
        }
        //protected void SetWeaponActive(bool isActive)
        //{
        //    _enemyMelee.gameObject.layer = isActive ? activeLayer : inactiveLayer;
        //}
        protected void InitializeWeapon()
        {
            if (_enemyStateMachine.EquippedWeapon != null)
            {
                _enemyMelee = _enemyStateMachine.EquippedWeapon.GetComponentInChildren<EnemyMelee>();
            }

            if (_enemyMelee == null)
            {
                Debug.LogError("No weapon equipped in state: " + this.GetType().Name);
            }
        }
        protected void RotateToPlayer(float deltaTime)
        {
            Vector3 direction = PlayerManager.Instance.PlayerStateMachine.transform.position - _enemyStateMachine.transform.position;
            direction.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _enemyStateMachine.transform.rotation = Quaternion.Slerp(_enemyStateMachine.transform.rotation, targetRotation, deltaTime * _enemyStateMachine.RotationSpeed);
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
                _enemyMelee.EnemyWeaponDamage
                  (Random.Range(_enemyMelee.EquippedWeaponDataSO.minDamage, _enemyMelee.EquippedWeaponDataSO.maxDamage + 1),
                  _enemyStateMachine.CriticalModifier);
            }
            else
            {
                _enemyMelee.EnemyWeaponDamage
                     (Random.Range(_enemyMelee.EquippedWeaponDataSO.minDamage, _enemyMelee.EquippedWeaponDataSO.maxDamage + 1),
                     1);
            }
        }
        protected void SetEnemyLayerDuringAttack(string layer)
        {
            _enemyMelee.gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }
}
