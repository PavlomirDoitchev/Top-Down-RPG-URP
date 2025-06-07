using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public PlayerStateMachine stateMachine;

    public void OnHitAnimationComplete()
    {
        if (stateMachine != null)
        {
            stateMachine.ChangeState(new FighterLocomotionState(stateMachine));
        }
    }
}