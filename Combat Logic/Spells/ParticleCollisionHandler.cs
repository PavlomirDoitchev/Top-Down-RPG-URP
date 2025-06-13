using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections.Generic;

public class ParticleCollisionHandler : MonoBehaviour
{
	[SerializeField] bool shouldShakeCamera = true;
	private readonly List<GameObject> enemyList = new();

	private void OnEnable()
	{
		if (shouldShakeCamera)
			PlayerManager.Instance.PlayerStateMachine.CinemachineImpulseSource.GenerateImpulse();
	}
	private void OnDisable()
	{
		enemyList.Clear();
	}
	void OnParticleCollision(GameObject other)
	{
		if(enemyList.Contains(other))
			return;

		if (other.gameObject.layer == 7 && other.TryGetComponent<IDamagable>(out var damagable)) 
		{
			enemyList.Add(other);
			//Debug.Log(enemyList.Count + " enemies hit by particle collision");
			int damage = Mathf.RoundToInt(10 *
				PlayerManager.Instance.PlayerStateMachine.BasicAttackData[PlayerManager.Instance.PlayerStateMachine.BasicAttackRank].damageMultiplier);
			damagable.TakeDamage(damage, false);
			PlayerManager.Instance.PlayerStateMachine.DamageText[3].Spawn(other.transform.position + Vector3.up * 2f, damage);
		}
	}
}
