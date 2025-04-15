using UnityEngine;
using System.Collections.Generic;

public class EnemyRangeSpawns : MonoBehaviour 
{
    public GameObject enemyPrefab;
    public float spawnDistance = 15f;
    public float minSpawnY = 2f;
    public int maxEnemies = 5;
    public float spawnInterval = 3f;
    public float moveSpeed = 4f;
    public float keepDistance = 8f;
    public float verticalOffset = 2f;
    public float followSpeed = 3f;
    public float shootingRange = 12f;
    public float shootInterval = 2f;

    GameObject player;
    List<GameObject> enemies = new List<GameObject>();
    float nextSpawnTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null) return;

        if (Time.time >= nextSpawnTime && enemies.Count < maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }

        UpdateEnemies();
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = player.transform.position + Vector3.right * spawnDistance;
        spawnPos.y = Mathf.Max(spawnPos.y + verticalOffset, minSpawnY);

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemies.Add(enemy);
    }

    void UpdateEnemies()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            MoveEnemy(enemies[i]);
        }
    }

    void MoveEnemy(GameObject enemy)
    {
        // Tính vị trí mục tiêu (bên phải player)
        Vector3 targetPos = player.transform.position + Vector3.right * keepDistance;
        targetPos.y += verticalOffset;

        // Di chuyển
        enemy.transform.position = Vector3.Lerp(
            enemy.transform.position,
            targetPos,
            followSpeed * Time.deltaTime
        );

        // Xoay về hướng player
        Vector3 direction = (player.transform.position - enemy.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Bắn khi đủ gần
        if (Vector3.Distance(enemy.transform.position, player.transform.position) <= shootingRange)
        {
            enemy.SendMessage("OnShoot", SendMessageOptions.DontRequireReceiver);
        }
    }
}
