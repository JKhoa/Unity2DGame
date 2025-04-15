using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class EnemySpawns : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDistance = 15f;
    public float minSpawnY = 2f;
    public int maxEnemies = 10;
    public float spawnInterval = 2f;
    public float moveSpeed = 5f;

    private GameObject player;
    private Camera mainCamera;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private float nextSpawnTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main;
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (player == null) return;

        // Spawn enemies
        if (Time.time >= nextSpawnTime && activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }

        // Update enemies
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            MoveEnemy(activeEnemies[i]);
        }
    }

    void SpawnEnemy()
    {
        Vector3 playerPos = player.transform.position;
        float randomAngle = Random.Range(0f, 360f);
        Vector3 spawnPos = playerPos + (Quaternion.Euler(0, 0, randomAngle) * Vector3.right * spawnDistance);
        
        // Ensure minimum Y position
        if (spawnPos.y < minSpawnY)
        {
            spawnPos.y = minSpawnY;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        activeEnemies.Add(enemy);

        // Keep original scale
        enemy.transform.localScale = enemyPrefab.transform.localScale;
    }

    void MoveEnemy(GameObject enemy)
    {
        if (player == null) return;

        Vector3 direction = (player.transform.position - enemy.transform.position).normalized;
        enemy.transform.position += direction * moveSpeed * Time.deltaTime;

        // Optional: Face the movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.transform.position, spawnDistance);
        }
    }
}
