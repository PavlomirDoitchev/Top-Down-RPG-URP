using DamageNumbersPro;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Enemy Spell Data/Projectile Spell Data")]
public class ProjectileData : ScriptableObject
{
    public string spellName;    
    public int damage;
    public float speed;
    public float lifeTime;
    public float aimNoise_X = 0.5f;
    public float aimNoise_Y = 0.5f;
    public LayerMask targetLayer;
    public LayerMask selfDestroyLayer;
    public DamageNumber damageNumberPrefab;
}
