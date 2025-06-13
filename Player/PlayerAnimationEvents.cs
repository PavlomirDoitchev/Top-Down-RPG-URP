using Assets.Scripts.State_Machine.Player_State_Machine;
using Assets.Scripts.State_Machine.Player_State_Machine.FighterStates;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public PlayerStateMachine _stateMachine;
    [SerializeField] private PlayerMelee _playerMelee;
    [SerializeField] private MeleeWeapon[] _meleeWeapon;
    [SerializeField] private TrailRenderer _trailRenderer;
    public ParticleSystem[] spellVFX;
    private void Start()
    {
        if (_stateMachine == null)
            _stateMachine = GetComponent<PlayerStateMachine>();
		if (_playerMelee == null)
            _playerMelee = GetComponentInChildren<PlayerMelee>();   


	}
	
	public void BackToLocomotion() => _stateMachine.ChangeState(new FighterLocomotionState(_stateMachine));
    public void EnableFrontMeleeCollider() => _playerMelee.SetWeaponActive(true, 0);
	public void DisableFrontMeleeCollider() => _playerMelee.SetWeaponActive(false, 0);
    public void EnableAoEMeleeCollider() => _playerMelee.SetWeaponActive(true, 1);
	public void DisableAoEMeleeCollider() => _playerMelee.SetWeaponActive(false, 1);
    public void ClearAllEnemiesFrontCollider() => _meleeWeapon[0].enemyColliders.Clear();
    public void ClearAllEnemiesAoECollider() => _meleeWeapon[1].enemyColliders.Clear();
    public void EnableDisableKnockback() 
    {
        _stateMachine.PlayerStats.ShouldKnockback = !_stateMachine.PlayerStats.ShouldKnockback;
	}
	public void EnableDisableTrails() => _trailRenderer.emitting = !_trailRenderer.emitting;
    public void EnableSpellVFX()
    {
        if(_stateMachine.PlayerCurrentState is FighterAbilityTwoState)
            spellVFX[0].gameObject.SetActive(true);
    }



}