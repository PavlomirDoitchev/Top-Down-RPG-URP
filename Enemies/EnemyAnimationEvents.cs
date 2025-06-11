using Assets.Scripts.Enemies;
using Assets.Scripts.Enemies.EnemyType;
using Assets.Scripts.Player;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] private EnemyMelee[] enemyMelee;
    [SerializeField] private EnemyStateMachine enemyStateMachine;
    [SerializeField] private EnemyProjectileAbility enemySpell;
    [SerializeField] ArcherHideArrow archerArrow;
    PlayerManager playerManager;
    private void Start()
    {
        playerManager = PlayerManager.Instance;
    }
    #region Common Animation Events 
    public void EnableMainHandWeaponDamage() => enemyMelee[0].SetEnemyLayerDuringAttack();
    public void DisableMainHandWeaponDamage() => enemyMelee[0].CantHarmPlayer();
    public void EnableOffHandWeaponDamage() => enemyMelee[1].SetEnemyLayerDuringAttack();
    public void DisableOffHandWeaponDamage() => enemyMelee[1].CantHarmPlayer();
    public void KnockBack() => enemyStateMachine.ShouldKnockBackPlayer = true;
    public void DontKnockBack() => enemyStateMachine.ShouldKnockBackPlayer = false;

    #endregion

    #region Melee Combo Attacks  
    public void DragonBruteTwoHitCombo() => enemyStateMachine.TwoHitCombo();
    #endregion

    #region Spell Casting
    public void CastSpell() => enemySpell.Cast(PlayerManager.Instance.PlayerStateMachine.transform);
    public void ChangeAnimationAfterCast() => enemyStateMachine.Animator.CrossFadeInFixedTime(enemyStateMachine.IdleAnimationName, .1f);
    #endregion

    #region Skeleton Archer Animation Events
    public void HideArrow() => archerArrow.HideArrow();
    public void ShowArrow() => archerArrow.ShowArrow();
    #endregion
}
