using System;
using UnityEngine;

namespace Assets.Scripts.Combat_Logic
{
	[Serializable]
	public class Cooldown
	{
		private float timeRemaining = 0f;

		public bool IsReady => timeRemaining <= 0f;

		public void Start(float duration)
		{
			timeRemaining = duration;
		}

		public void Tick(float deltaTime)
		{
			if (timeRemaining > 0f)
				timeRemaining -= deltaTime;
			//Debug.Log($"Cooldown time remaining: {timeRemaining}");
		}
	}
}
