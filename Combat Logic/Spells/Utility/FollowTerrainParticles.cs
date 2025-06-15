using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class FollowTerrainParticles : MonoBehaviour
{
	private ParticleSystem ps;
	private ParticleSystem.Particle[] particles;
	public LayerMask groundLayer;
	public float raycastDistance = 100f;
	public float offsetAboveGround = 0.1f;


	void Start()
	{
		ps = GetComponent<ParticleSystem>();
		particles = new ParticleSystem.Particle[ps.main.maxParticles];
	}
	
	void LateUpdate()
	{
		int count = ps.GetParticles(particles);
		for (int i = 0; i < count; i++)
		{
			//spellCollider.transform.position = particles[i].position;
			Vector3 pos = particles[i].position;
			Ray ray = new Ray(pos + Vector3.up * raycastDistance * 0.5f, Vector3.down);

			if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, groundLayer))
			{
				pos.y = hit.point.y + offsetAboveGround;
				particles[i].velocity = Vector3.ProjectOnPlane(particles[i].velocity, hit.normal);

				particles[i].position = pos;
			}
		}

		ps.SetParticles(particles, count);
	}
}
