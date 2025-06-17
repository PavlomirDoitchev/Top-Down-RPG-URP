using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolManager : MonoBehaviour
{
	public static ProjectilePoolManager Instance { get; private set; }

	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	public List<Pool> pools;
	private Dictionary<string, Queue<GameObject>> poolDictionary;

	void Awake()
	{
		if (Instance != null && Instance != this) Destroy(gameObject);
		Instance = this;

		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach (var pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();
			for (int i = 0; i < pool.size; i++)
			{
				GameObject obj = Instantiate(pool.prefab, this.transform);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			poolDictionary.Add(pool.tag, objectPool);
		}
	}

	public GameObject GetProjectile(string tag)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning($"Projectile tag {tag} not found!");
			return null;
		}

		var obj = poolDictionary[tag].Dequeue();
		poolDictionary[tag].Enqueue(obj);
		return obj;
	}
}
