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
        elapsedTime += deltaTime;

        // Apply movement from ForceReceiver
        Move(deltaTime);

        if (elapsedTime >= knockbackDuration)
        {
            // Snap to NavMesh and re-enable pathfinding
            _enemyStateMachine.Agent.Warp(_enemyStateMachine.transform.position);
            _enemyStateMachine.Agent.enabled = true;

            // Transition to previous or default state
            _enemyStateMachine.ChangeState(new EnemyIdleState(_enemyStateMachine)); // Or remember last state
        }
    }

    public override void ExitState()
    {
        ResetMovementSpeed();
    }
}
