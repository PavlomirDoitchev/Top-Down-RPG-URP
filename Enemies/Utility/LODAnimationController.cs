using UnityEngine;

public class LODAnimationController : MonoBehaviour
{
	public LODGroup lodGroup;
	public Animator animator;
	public int animationCutoffLOD = 2; // Disable animation at LOD 2 or higher

	void Update()
	{
		if (lodGroup == null || animator == null) return;

		LOD[] lods = lodGroup.GetLODs();
		float screenRelativeHeight = GetScreenRelativeHeight();
		int currentLOD = GetCurrentLODIndex(lods, screenRelativeHeight);

		animator.enabled = currentLOD < animationCutoffLOD;
	}

	float GetScreenRelativeHeight()
	{
		Camera cam = Camera.main;
		if (cam == null) return 1f;

		// Calculate combined bounds from all renderers
		Bounds bounds = new Bounds(transform.position, Vector3.zero);
		foreach (var renderer in lodGroup.GetComponentsInChildren<Renderer>())
		{
			if (renderer.enabled)
				bounds.Encapsulate(renderer.bounds);
		}

		float distance = Vector3.Distance(cam.transform.position, bounds.center);
		float size = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
		float height = size / distance;

		return height;
	}

	int GetCurrentLODIndex(LOD[] lods, float screenHeight)
	{
		for (int i = 0; i < lods.Length; i++)
		{
			if (screenHeight >= lods[i].screenRelativeTransitionHeight)
				return i;
		}
		return lods.Length - 1;
	}
}
