using UnityEngine;
using System.Collections.Generic;

public class EnemyShtSpawn : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public float spawnDistance = 15f;
    public float minSpawnY = 2f;
    public int maxEnemies = 5;
    public float spawnInterval = 3f;
    public float moveSpeed = 4f;

    [Header("Range Enemy Settings")]
    public float keepDistance = 8f;
    public float verticalOffset = 2f;
    public float followSpeed = 3f;
    public float shootingRange = 12f;
    public float shootInterval = 2f;

    private GameObject player;
    private Camera mainCamera;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private float nextSpawnTime;
    private Dictionary<GameObject, float> nextShootTimes = new Dictionary<GameObject, float>();

    private void Awake()
    {
        // Đảm bảo các giá trị không âm
        spawnDistance = Mathf.Max(0, spawnDistance);
        minSpawnY = Mathf.Max(0, minSpawnY);
        maxEnemies = Mathf.Max(0, maxEnemies);
        spawnInterval = Mathf.Max(0.1f, spawnInterval);
        moveSpeed = Mathf.Max(0, moveSpeed);
        keepDistance = Mathf.Max(0, keepDistance);
        followSpeed = Mathf.Max(0, followSpeed);
        shootingRange = Mathf.Max(0, shootingRange);
        shootInterval = Mathf.Max(0.1f, shootInterval);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure player has tag 'Player'");
            enabled = false;
            return;
        }

        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab not assigned!");
            enabled = false;
            return;
        }

        mainCamera = Camera.main;
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (!enabled || player == null) return;

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

            MoveRangeEnemy(activeEnemies[i]);
        }
    }

    private void SpawnEnemy()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 spawnPos = playerPos + (Vector3.right * spawnDistance) + (Vector3.up * verticalOffset);

        if (spawnPos.y < minSpawnY)
        {
            spawnPos.y = minSpawnY;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        activeEnemies.Add(enemy);
        nextShootTimes[enemy] = Time.time + shootInterval;

        // Keep original scale
        enemy.transform.localScale = enemyPrefab.transform.localScale;
    }

    private void MoveRangeEnemy(GameObject enemy)
    {
        Vector3 playerPos = player.transform.position;
        Vector3 targetPos = playerPos + (Vector3.right * keepDistance) + (Vector3.up * verticalOffset);

        // Di chuyển mượt đến vị trí mục tiêu
        enemy.transform.position = Vector3.Lerp(
            enemy.transform.position,
            targetPos,
            followSpeed * Time.deltaTime
        );

        // Luôn quay mặt về phía player
        Vector3 direction = (playerPos - enemy.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Kiểm tra khoảng cách và thời gian để bắn
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, playerPos);
        if (distanceToPlayer <= shootingRange && Time.time >= nextShootTimes[enemy])
        {
            enemy.SendMessage("OnShoot", SendMessageOptions.DontRequireReceiver);
            nextShootTimes[enemy] = Time.time + shootInterval;
        }
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            // Vẽ vòng tròn spawn
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.transform.position, spawnDistance);

            // Vẽ vòng tròn khoảng cách giữ
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, keepDistance);

            // Vẽ vòng tròn tầm bắn
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.transform.position, shootingRange);
        }
    }

    // Public methods để thay đổi giá trị từ bên ngoài
    public void SetSpawnDistance(float distance) => spawnDistance = Mathf.Max(0, distance);
    public void SetMaxEnemies(int max) => maxEnemies = Mathf.Max(0, max);
    public void SetSpawnInterval(float interval) => spawnInterval = Mathf.Max(0.1f, interval);
    public void SetKeepDistance(float distance) => keepDistance = Mathf.Max(0, distance);
    public void SetShootingRange(float range) => shootingRange = Mathf.Max(0, range);
}
