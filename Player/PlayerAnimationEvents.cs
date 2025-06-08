using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public PlayerStateMachine stateMachine;
    [SerializeField] private MeleeWeapon meleeWeapon;
    private void Start()
    {
        if (stateMachine == null)
            stateMachine = GetComponent<PlayerStateMachine>();
        if (meleeWeapon == null)
            meleeWeapon = stateMachine.EquippedWeapon.GetComponent<MeleeWeapon>();
    }
    public void BackToLocomotion()
    {
        stateMachine.ChangeState(new FighterLocomotionState(stateMachine));
    }
    public void EnableDisableTrails() => meleeWeapon.TrailRenderSwitcher();
    public void EnableDisableKnockback() => meleeWeapon.ShouldKnockBackSwitcher();
}