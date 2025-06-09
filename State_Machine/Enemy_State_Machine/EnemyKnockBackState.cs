using Assets.Scripts.State_Machine.Enemy_State_Machine;

public class EnemyKnockbackState : EnemyBaseState
{
    private float knockbackDuration;
    private float elapsedTime;

    public EnemyKnockbackState(EnemyStateMachine stateMachine, float duration) : base(stateMachine)
    {
        knockbackDuration = duration;
    }

    public override void EnterState()
    {
        base.EnterState();
        _enemyStateMachine.Agent.enabled = false;
        _enemyStateMachine.Animator.CrossFadeInFixedTime(_enemyStateMachine.HitAnimationName, 0.1f);
    }

    public override void UpdateState(float deltaTime)
    {
       // CheckForGlobalTransitions();
        elapsedTime += deltaTime;
        Move(deltaTime);

        if (elapsedTime >= knockbackDuration)
        {
            _enemyStateMachine.Agent.Warp(_enemyStateMachine.transform.position);
            _enemyStateMachine.Agent.enabled = true;

            // Default state
            if (_enemyStateMachine.PreviousState is EnemyPatrolState)
            {
                _enemyStateMachine.ChangeState(new EnemyMeleeAttackState(_enemyStateMachine));
                return;
            }
            _enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine)); 
        }
    }

    public override void ExitState()
    {
        MovementSpeedRunning();
    }
}
