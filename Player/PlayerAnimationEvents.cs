using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public PlayerStateMachine _stateMachine;
    [SerializeField] private PlayerMelee meleeWeapon;
    private void Start()
    {
        if (_stateMachine == null)
            _stateMachine = GetComponent<PlayerStateMachine>();

    }
    public void BackToLocomotion() => _stateMachine.ChangeState(new FighterLocomotionState(_stateMachine));
    public void EnableFrontMeleeCollider() => meleeWeapon.SetWeaponActive(true, 0);
    //public void EnableDisableTrails() => meleeWeapon.TrailRenderSwitcher();
    //public void EnableDisableKnockback() => meleeWeapon.ShouldKnockBackSwitcher();


    //public void EnableFrontMeleeCollider() => meleeWeapon.SetWeaponActive(true, 0);
    //public void DisableFrontMeleeCollider() => meleeWeapon.SetWeaponActive(false, 0);
    //public void EnableAoEMeleeCollider() => meleeWeapon.SetWeaponActive(true, 1);
    //public void DisableAoEMeleeCollider() => meleeWeapon.SetWeaponActive(false, 1);
}