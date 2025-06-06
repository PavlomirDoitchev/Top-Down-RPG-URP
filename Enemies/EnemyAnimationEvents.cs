using Assets.Scripts.Enemies;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] private EnemyMelee enemyMelee;

    public void EnableWeaponDamage() => enemyMelee.SetEnemyLayerDuringAttack();
    public void DisableWeaponDamage() => enemyMelee.CantHarmPlayer();
}
