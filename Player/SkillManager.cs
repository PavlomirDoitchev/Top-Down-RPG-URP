using Assets.Scripts.Player.Abilities;
using Assets.Scripts.Player.Abilities.Fighter_Abilities;
using Assets.Scripts.Player.Abilities.Mage_Abilities;
using Assets.Scripts.Player.Weapons;
using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Player
{
    public class SkillManager : MonoBehaviour
    {
        public static SkillManager Instance;

        public Dodge Dodge { get; private set; }
		public FighterBasicAttack FighterBasicAttack { get; private set; }
        public FighterAbilityOne FighterAbilityOne { get; private set; }
        public ShockwaveAbility ShockwaveAbility { get; private set; }
        public RainOfFireAbility RainOfFireAbility { get; private set; }
        public LightningShieldAbility LightningShieldAbility { get; private set; }
        public CenterPullAbility CenterPullAbility { get; private set; }
        public PlayerProjectileAbility[] ProjectileAbility { get; private set; }
        private void Awake()
        {
            if (Instance != null)            
                Destroy(Instance.gameObject);
            else
                Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        private void Start()
        {
			Dodge = GetComponent<Dodge>();
            ProjectileAbility = GetComponents<PlayerProjectileAbility>();
            if (PlayerManager.Instance.PlayerStateMachine.CharacterLevelDataSO[0].characterClass == CharacterLevelSO.CharacterClass.Fighter)
            {
				ShockwaveAbility = GetComponent<ShockwaveAbility>();
				FighterAbilityOne = GetComponent<FighterAbilityOne>();
                FighterBasicAttack = GetComponent<FighterBasicAttack>();
                LightningShieldAbility = GetComponent<LightningShieldAbility>();
                RainOfFireAbility = GetComponent<RainOfFireAbility>();
                CenterPullAbility = GetComponent<CenterPullAbility>();
            }
            else if (PlayerManager.Instance.PlayerStateMachine.CharacterLevelDataSO[0].characterClass == CharacterLevelSO.CharacterClass.Mage)
            {
                //TODO:Add Mage skills

            }
            else if (PlayerManager.Instance.PlayerStateMachine.CharacterLevelDataSO[0].characterClass == CharacterLevelSO.CharacterClass.Rogue) 
            {
                //Add Rogue Skills
            
            }
        }
        /// <summary>
        /// Aims the spell based on the mouse position in the world.
        /// </summary>
        /// <returns></returns>
		public virtual Vector3 AimSpell()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			int groundMask = LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
            {
                Vector3 aimPoint = hit.point + Vector3.up;
                return aimPoint;
            }
            else if (Physics.Raycast(ray, out RaycastHit enemyHit, Mathf.Infinity, LayerMask.GetMask("Enemy"))) 
            {
                Vector3 aimPoint = enemyHit.point;
                return aimPoint;
            }

            else return Vector3.zero;
		}
	}
}
