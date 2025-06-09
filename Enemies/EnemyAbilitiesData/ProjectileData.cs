using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Enemy Spell Data/Projectile Spell Data")]
public class ProjectileData : ScriptableObject
{
    public string spellName;    
    public int damage;
    public float speed;
    public float lifeTime;
    public LayerMask targetLayer;
    public LayerMask selfDestroyLayer;
}
