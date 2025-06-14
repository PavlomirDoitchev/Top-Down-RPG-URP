﻿using Assets.Scripts.Player.Abilities;
using Assets.Scripts.Player.Abilities.Fighter_Abilities;
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
        public ShockwaveAbility FighterAbilityTwo { get; private set; }

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
			if (PlayerManager.Instance.PlayerStateMachine.CharacterLevelDataSO[0].characterClass == CharacterLevelSO.CharacterClass.Fighter)
            {
				FighterAbilityTwo = GetComponent<ShockwaveAbility>();
				FighterAbilityOne = GetComponent<FighterAbilityOne>();
                FighterBasicAttack = GetComponent<FighterBasicAttack>();
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
    }
}
