using Assets.Scripts.State_Machine.Enemy_State_Machine;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies.Utility
{
    public class Spawner : MonoBehaviour
    {
        [Header("Enemy Setup")]
        [SerializeField] private GameObject[] enemyPrefabs; 
        [SerializeField] private Transform[] spawnPoints;

        [Header("Wave Settings")]
        [SerializeField] private float spawnInterval = 1.5f;
        [SerializeField] private int maxWaves = 5;
        [SerializeField] private float spawnRadius = 2f;

        private readonly List<EnemyStateMachine> aliveEnemies = new List<EnemyStateMachine>();

        private int currentWave = 0;
        private int enemiesToSpawn;
        private int enemiesSpawned;
        private float timer;
        private bool spawningWave;

        private Queue<GameObject> spawnQueue = new Queue<GameObject>();

        private void Update()
        {
            if (!spawningWave && aliveEnemies.Count == 0 && currentWave < maxWaves)
            {
                StartNextWave();
            }

            if (spawningWave)
            {
                timer += Time.deltaTime;
                if (timer >= spawnInterval && enemiesSpawned < enemiesToSpawn)
                {
                    SpawnEnemy();
                    timer = 0f;
                }

                if (enemiesSpawned >= enemiesToSpawn)
                {
                    spawningWave = false;
                }
            }
        }

        private void StartNextWave()
        {
            currentWave++;
            enemiesToSpawn = 2 + currentWave;
            enemiesSpawned = 0;
            spawningWave = true;

            PrepareSpawnQueue();

            Debug.Log($"Wave {currentWave} started with {enemiesToSpawn} enemies.");
        }

        private void PrepareSpawnQueue()
        {
            spawnQueue.Clear();

            int uniqueCount = Mathf.Min(enemiesToSpawn, enemyPrefabs.Length);
            for (int i = 0; i < uniqueCount; i++)
            {
                spawnQueue.Enqueue(enemyPrefabs[i]);
            }

            for (int i = uniqueCount; i < enemiesToSpawn; i++)
            {
                GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                spawnQueue.Enqueue(randomEnemy);
            }
        }

        private void SpawnEnemy()
        {
            if (spawnQueue.Count == 0 || spawnPoints.Length == 0)
            {
                Debug.LogWarning("No enemies in spawn queue or no spawn points set!");
                return;
            }

            GameObject prefab = spawnQueue.Dequeue();
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Vector2 offset2D = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = spawnPoint.position + new Vector3(offset2D.x, 0f, offset2D.y);

            GameObject enemyObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
            EnemyStateMachine enemy = enemyObj.GetComponent<EnemyStateMachine>();

            if (enemy != null)
            {
                aliveEnemies.Add(enemy);
                enemy.OnDeath += () => aliveEnemies.Remove(enemy);
            }

            enemiesSpawned++;
        }
    }
}
