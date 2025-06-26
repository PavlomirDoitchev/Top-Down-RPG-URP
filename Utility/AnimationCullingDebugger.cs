using UnityEngine;

public class AnimatorCullingDebugger : MonoBehaviour
{
	private Animator animator;
	private Renderer[] renderers;
	private bool wasVisible;

	void Start()
	{
		animator = GetComponent<Animator>();
		renderers = GetComponentsInChildren<Renderer>();
		wasVisible = IsVisible();
	}

	void Update()
	{
		bool currentlyVisible = IsVisible();

		if (currentlyVisible != wasVisible)
		{
			wasVisible = currentlyVisible;

			//if (currentlyVisible)
			//	Debug.Log($"{name} is now visible — Animator should be updating.");
			//else
			//	Debug.Log($"{name} is now culled — Animator may stop updating depending on culling mode.");
		}
	}

	bool IsVisible()
	{
		foreach (Renderer r in renderers)
		{
			if (r.isVisible)
				return true;
		}
		return false;
	}
}
