using System.Collections;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField] float timer;
	private void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			Destroy(gameObject);
		}
	}
}

