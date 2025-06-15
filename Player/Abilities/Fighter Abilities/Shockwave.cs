using Assets.Scripts.Combat_Logic;
using Assets.Scripts.Player;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.State_Machine.Enemy_State_Machine;
using Assets.Scripts.State_Machine.Player_State_Machine;

public class Shockwave : MonoBehaviour
{
	[SerializeField] bool shouldShakeCamera = true;

	private readonly List<Collider> enemyList = new();

	float knockBackForce;
	int damage;
	float multiplier;

	[Header("Cone Settings")]
	[SerializeField] float maxRadius = 5f;
	[SerializeField] float coneAngle = 60f;
	[SerializeField] LayerMask enemyLayer;
	[SerializeField] float damageCheckInterval = 0.1f;
	[SerializeField] float impactRadius = 2f;

	[Header("Spell Settings")]
	//[SerializeField] Collider spellCollider;  // You might still want this for visuals or triggers?
	[SerializeField] float maxDuration = 1f;
	float timer;

	private void OnEnable()
	{
		timer = maxDuration;
		enemyList.Clear();

		if (shouldShakeCamera)
			PlayerManager.Instance.PlayerStateMachine.CinemachineImpulseSource.GenerateImpulse();
	}

	private void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			gameObject.SetActive(false);
		}
		else
		{
			damageCheckInterval -= Time.deltaTime;
			if (damageCheckInterval <= 0f)
			{
				ImpactCheck();
				CheckConeCollision();
				damageCheckInterval = 0.1f; // resets interval
			}
		}
	}
	void ImpactCheck()
	{

		Collider[] hits = Physics.OverlapSphere(transform.position, impactRadius, enemyLayer);
		foreach (Collider enemy in hits)
		{
			if (enemyList.Contains(enemy))
				continue;
			KnockUp(enemy);
			ApplyDamageTo(enemy);
			enemyList.Add(enemy);
		}
	}
	private void CheckConeCollision()
	{
		Vector3 origin = transform.position;
		Vector3 forward = transform.forward;
		int radialSteps = 30;
		float stepAngle = coneAngle / radialSteps;
		float stepDistance = maxRadius / 10f;

		for (int i = 0; i <= radialSteps; i++)
		{
			float angle = -coneAngle / 2f + stepAngle * i;
			Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
			Vector3 direction = rotation * forward;

			for (int j = 1; j <= 10; j++)
			{
				Vector3 samplePoint = origin + direction * stepDistance * j;
				Vector3 rayStart = samplePoint + Vector3.up * 5f;

				if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 10f, enemyLayer | (1 << 6))) // Layer 6
				{
					Collider[] hits = Physics.OverlapSphere(hit.point, 0.5f, enemyLayer);
					foreach (Collider enemy in hits)
					{
						if (enemyList.Contains(enemy)) continue;
						enemyList.Add(enemy);
						KnockUp(enemy);
						ApplyDamageTo(enemy);
					}
				}
			}
		}
	}

	private void ApplyDamageTo(Collider other)
	{
		if (other.gameObject.TryGetComponent<IDamagable>(out var damagable))
		{
			multiplier = PlayerManager.Instance.PlayerStateMachine.Ability_Two_Data[PlayerManager.Instance.PlayerStateMachine.Ability_Two_Rank].damageMultiplier;

			if (PlayerManager.Instance.PlayerStateMachine.CriticalStrikeSuccess())
			{
				damage = Mathf.RoundToInt(PlayerManager.Instance.PlayerStateMachine.WeaponDamage(damage, multiplier)
					* (PlayerManager.Instance.PlayerStateMachine.PlayerStats.CriticalDamageModifier
					+ PlayerManager.Instance.PlayerStateMachine.Weapon.criticalDamageModifier));
				damagable.TakeDamage(damage, false);
				PlayerManager.Instance.PlayerStateMachine.DamageText[4].Spawn(other.transform.position + Vector3.up * 2f, damage);
			}
			else
			{
				damage = Mathf.RoundToInt(PlayerManager.Instance.PlayerStateMachine.WeaponDamage(damage, multiplier));
				damagable.TakeDamage(damage, false);
				PlayerManager.Instance.PlayerStateMachine.DamageText[3].Spawn(other.transform.position + Vector3.up * 2f, damage);
			}

			//Debug.Log($"Cone hit: {other.gameObject.name}");
			//Debug.Log(enemyList.Count + " enemies hit by cone.");
		}
	}

	private void KnockUp(Collider other)
	{
		if (other.TryGetComponent<ForceReceiver>(out var forceReceiver)
			&& other.TryGetComponent<EnemyStateMachine>(out var enemyStateMachine)
			&& !enemyStateMachine.IsEnraged)
		{
			if (!PlayerManager.Instance.PlayerStateMachine.Ability_Two_Data[PlayerManager.Instance.PlayerStateMachine.Ability_Two_Rank].canKnockback)
				return;

			knockBackForce = PlayerManager.Instance.PlayerStateMachine.Ability_Two_Data[PlayerManager.Instance.PlayerStateMachine.Ability_Two_Rank].knockbackForce;

			Vector3 pullDir = (PlayerManager.Instance.PlayerStateMachine.transform.position - other.transform.position).normalized;
			Vector3 knockDir = (PlayerManager.Instance.PlayerStateMachine.transform.position - other.transform.position).normalized;
			if (enemyStateMachine.IsKnockedUp)
				return;
			forceReceiver.AddForce(knockDir * knockBackForce);
			forceReceiver.KnockUp(knockBackForce * 0.2f);
			enemyStateMachine.ChangeState(new EnemyKnockedUpState(enemyStateMachine));
			//enemyStateMachine.ChangeState(new EnemyKnockbackState(enemyStateMachine, .5f));
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Vector3 origin = transform.position;
		Vector3 forward = transform.forward;

		// Get ground normal
		if (Physics.Raycast(origin + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 2f, 1 << 6))
		{
			forward = Vector3.ProjectOnPlane(forward, hit.normal).normalized;
		}

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(origin, maxRadius);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(origin, impactRadius);

		Gizmos.color = Color.cyan;
		int stepCount = 30;
		float angleStep = coneAngle / stepCount;
		Quaternion startRotation = Quaternion.AngleAxis(-coneAngle / 2f, Vector3.up);

		for (int i = 0; i <= stepCount; i++)
		{
			Quaternion rotation = startRotation * Quaternion.AngleAxis(i * angleStep, Vector3.up);
			Vector3 direction = rotation * forward;
			Gizmos.DrawLine(origin, origin + direction * maxRadius);
		}
	}
#endif
}
