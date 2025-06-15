using UnityEngine;

public class AutoDisableParticle : MonoBehaviour
{
	private ParticleSystem ps;

	void Start()
	{
		ps = GetComponent<ParticleSystem>();
	}

	void Update()
	{
		if (ps != null && !ps.IsAlive())
		{
			gameObject.SetActive(false);
		}
	}
}
