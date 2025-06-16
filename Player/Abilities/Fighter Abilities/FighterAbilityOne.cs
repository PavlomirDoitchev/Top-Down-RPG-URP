using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class FighterAbilityOne : Skills
    {
        [field: SerializeField] public ParticleSystem particle { get; private set; }

		

		public override void UseSkill()
        {
            base.UseSkill();
            if (particle.isPlaying)
            {
                particle.Stop();
            }
            particle.Play();
        }

    }
}
