using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Player
{
    public class SkillManager : MonoBehaviour
    {
        public static SkillManager Instance;
        public FighterBasicAttack fighterBasicAttack { get; private set; }
        public FighterQ fighterQ { get; private set; }
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
            if (PlayerManager.Instance.playerStateMachine.CharacterLevelDataSO[0].characterClass == CharacterLevelSO.CharacterClass.Fighter)
            {
                fighterQ = GetComponent<FighterQ>();
                fighterBasicAttack = GetComponent<FighterBasicAttack>();
            }
            else if (PlayerManager.Instance.playerStateMachine.CharacterLevelDataSO[0].characterClass == CharacterLevelSO.CharacterClass.Mage)
            {
                //TODO:Add Mage skills

            }
            else if (PlayerManager.Instance.playerStateMachine.CharacterLevelDataSO[0].characterClass == CharacterLevelSO.CharacterClass.Rogue) 
            {
                //Add Rogue Skills
            
            }
        }
    }
}
