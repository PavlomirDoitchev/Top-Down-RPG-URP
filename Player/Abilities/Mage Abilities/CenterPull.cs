using UnityEngine;
using System.Collections.Generic;
namespace Assets.Scripts.Player.Abilities.Mage_Abilities
{
    public class CenterPull : MonoBehaviour
    {
        [SerializeField] bool shouldShakeCamera = true;
        private readonly List<Collider> enemyList = new();
        float pullForce;
        int damage;
        float multiplier;
        [SerializeField] LayerMask enemyLayer;
        [SerializeField] float maxRadius = 5f;
        [SerializeField] float impactRadius = 2f;   
        [SerializeField] float damageCheckInterval = 0.1f;
        [SerializeField] float maxDuration = 1f;
    }
}
