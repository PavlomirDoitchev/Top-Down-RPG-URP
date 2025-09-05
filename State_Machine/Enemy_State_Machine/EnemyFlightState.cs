//using System;

//namespace Assets.Scripts.State_Machine.Enemy_State_Machine
//{
//    public class EnemyFlightState : EnemyBaseState
//    {
//        public EnemyFlightState(EnemyStateMachine stateMachine) : base(stateMachine)
//        {
//        }
//        public override void EnterState()
//        {
//            base.EnterState();
//            ResetAnimationSpeed();
//            if (!_enemyStateMachine.CanBeCrowdControlled) //Only boss encounters cannot be CC'd
//                _enemyStateMachine.BossPhaseCooldown.Start(_enemyStateMachine.BossPhaseCooldownDuration);

//            _enemyStateMachine.IsFlying = true;
//            _enemyStateMachine.Agent.isStopped = false;
//        }
       
//        public override void UpdateState(float deltaTime)
//        {
//            if (CheckForGlobalTransitions()) return;
//            if (!_enemyStateMachine.CanBeCrowdControlled)
//                _enemyStateMachine.BossPhaseCooldown.Tick(deltaTime);

//            if (_enemyStateMachine.BossPhaseCooldown.IsReady)
//            {
//                _enemyStateMachine.ChangeState(new EnemyLandState(_enemyStateMachine));
//            }
//            //TODO: Add abilities while flying. Engulf in flames, bombardment
//        }
//        public override void ExitState()
//        {
//            ResetAnimationSpeed();
//        }
//    }
//}