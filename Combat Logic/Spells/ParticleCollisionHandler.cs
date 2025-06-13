using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections.Generic;

public class ParticleCollisionHandler : MonoBehaviour
{
	[SerializeField] bool shouldShakeCamera = true;
	private readonly List<GameObject> enemyList = new();
	//float timer = 2f;
	//private void Update()
	//{
	//	//if (timer > 0)
	//	//{
	//	//	timer -= Time.deltaTime;
	//	//}
 // //      else
 // //      {
 // //          gameObject.SetActive(false);
	//	//}
 //   }
	private void OnEnable()
	{
		if (shouldShakeCamera)
			PlayerManager.Instance.PlayerStateMachine.CinemachineImpulseSource.GenerateImpulse();
	}
	private void OnDisable()
	{
		//timer = 2f;
		enemyList.Clear();
	}
	void OnParticleCollision(GameObject other)
	{
		if(enemyList.Contains(other))
			return;

		if (other.gameObject.layer == 7 && other.TryGetComponent<IDamagable>(out var damagable)) 
		{
			enemyList.Add(other);
			Debug.Log(enemyList.Count + " enemies hit by particle collision");
			int damage = Mathf.RoundToInt(10 *
				PlayerManager.Instance.PlayerStateMachine.BasicAttackData[PlayerManager.Instance.PlayerStateMachine.BasicAttackRank].damageMultiplier);
			damagable.TakeDamage(damage, false);
			Debug.Log($"Dealt {damage} damage to {other.name}");
			PlayerManager.Instance.PlayerStateMachine.DamageText.Spawn(other.transform.position, damage);
		}
	}
}
