using Assets.Scripts.State_Machine.Player_State_Machine;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Player
{
    public class SkillManager : MonoBehaviour
    {
        public static SkillManager Instance;

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
            fighterQ = GetComponent<FighterQ>();
        }
    }
}
