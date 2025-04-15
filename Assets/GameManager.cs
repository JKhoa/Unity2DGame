using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnRadius = 10f;
    public int maxEnemies = 10;
    public Transform player;

    private float nextSpawnTime;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isGameOver = false;
    private bool isPaused = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetupScene();
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void SetupScene()
    {
        // Thiết lập camera
        if (Camera.main != null)
        {
            Camera.main.orthographicSize = 5f; // Điều chỉnh zoom
            Camera.main.backgroundColor = new Color(0.2f, 0.3f, 0.5f); // Màu nền xanh đậm
        }

        // Thiết lập canvas
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
        }

        // Thiết lập player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Đặt vị trí player
            player.transform.position = new Vector3(0, -3, 0);
            
            // Đảm bảo scale đúng
            player.transform.localScale = new Vector3(1, 1, 1);

            // Kiểm tra và điều chỉnh các component
            SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = 5; // Đảm bảo hiển thị trên các object khác
            }
        }

        // Reset time scale
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Kiểm tra trạng thái pause trước khi xử lý logic game
        if (isPaused)
        {
            // Chỉ xử lý input pause/unpause khi đang pause
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ResumeGame();
            }
            return;
        }

        // Xử lý input pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
            return;
        }

        // Game logic
        if (isGameOver) return;

        // Spawn enemy
        if (Time.time >= nextSpawnTime && activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }

        // Dọn dẹp enemy đã bị hủy
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    void SpawnEnemy()
    {
        // Tìm vị trí spawn ngẫu nhiên xung quanh player
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = player.position + new Vector3(randomDirection.x, randomDirection.y, 0) * spawnRadius;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(enemy);
    }

    public void GameOver()
    {
        isGameOver = true;
        // Hiển thị menu game over
        Debug.Log("Game Over!");
    }

    public void RestartGame()
    {
        // Xóa tất cả enemy
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();

        // Reset player
        if (player != null)
        {
            player.position = Vector3.zero;
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.ResetStats();
            }
        }

        isGameOver = false;
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            Debug.Log("Game Paused - TimeScale set to 0");
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            Debug.Log("Game Resumed - TimeScale set to 1");
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}