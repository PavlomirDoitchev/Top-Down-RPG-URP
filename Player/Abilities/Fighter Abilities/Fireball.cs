using UnityEngine;
namespace Assets.Scripts.Player.Abilities.Fighter_Abilities
{
	public class Fireball : MonoBehaviour
	{
		Vector3 direction;

		[SerializeField] Rigidbody rb;
		private void OnEnable()
		{
			direction = SkillManager.Instance.AimSpell();
		}
		private void FixedUpdate()
		{
			rb.MovePosition(transform.position + direction *1 * Time.fixedDeltaTime);
		}
		private void OnDisable()
		{
			transform.position = Vector3.zero;
		}
	}
}
