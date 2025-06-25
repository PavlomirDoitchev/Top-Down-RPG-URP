namespace Assets.Scripts.State_Machine.Enemy_State_Machine
{
    public class EnemyStunnedState : EnemyBaseState
    {
        float stunDuration = 0f;
        float elapsedTime;
        public EnemyStunnedState(EnemyStateMachine stateMachine, float duration) : base(stateMachine)
        {
            stunDuration = duration;
            _enemyStateMachine.IsStunned = true;
        }
        public override void EnterState()
        {
            base.EnterState();
            _enemyStateMachine.IsKnockedBack = false;
            if (_enemyStateMachine.CurrentHealth <= 0) 
            {
                _enemyStateMachine.ChangeState(new EnemyDeathState(_enemyStateMachine));
                return;
            }
            _enemyStateMachine.Agent.enabled = false;
            elapsedTime = 0f;
            _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.StunnedAnimationName, .1f);
        }

        public override void UpdateState(float deltaTime)
        {
            //if (CheckForGlobalTransitions()) return;
            Move(deltaTime);
            elapsedTime += deltaTime;
            if (elapsedTime >= stunDuration)
            {
                _enemyStateMachine.Agent.Warp(_enemyStateMachine.transform.position);
                _enemyStateMachine.Agent.enabled = true;


                if (_enemyStateMachine.PreviousState is EnemyPatrolState
                    || _enemyStateMachine.PreviousState is EnemyWanderState
                    || _enemyStateMachine.PreviousState is EnemyIdleState)

                {
                    if (_enemyStateMachine.EnemyType == EnemyType.Melee)
                        _enemyStateMachine.ChangeState(new EnemyMeleeAttackState(_enemyStateMachine));
                    else if (_enemyStateMachine.EnemyType == EnemyType.Ranged || _enemyStateMachine.EnemyType == EnemyType.MeleeRanged)
                        _enemyStateMachine.ChangeState(new EnemyRangedAttackState(_enemyStateMachine));
                    else
                        _enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine));
                }
                else
                    _enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine));
            }
        }
        public override void ExitState()
        {
            _enemyStateMachine.Agent.enabled = true;
            _enemyStateMachine.IsStunned = false;
        }
    }
}
