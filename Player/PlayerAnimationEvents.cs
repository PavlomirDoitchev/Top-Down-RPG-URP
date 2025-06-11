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
    public void BackToLocomotion() => stateMachine.ChangeState(new FighterLocomotionState(stateMachine));
    
    public void EnableDisableTrails() => meleeWeapon.TrailRenderSwitcher();
    public void EnableDisableKnockback() => meleeWeapon.ShouldKnockBackSwitcher();


    public void EnableFrontMeleeCollider() => meleeWeapon.SetWeaponActive(true, 0);
    public void DisableFrontMeleeCollider() => meleeWeapon.SetWeaponActive(false, 0);
    public void EnableAoEMeleeCollider() => meleeWeapon.SetWeaponActive(true, 1);
    public void DisableAoEMeleeCollider() => meleeWeapon.SetWeaponActive(false, 1);
}