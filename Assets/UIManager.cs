using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Canvas Settings")]
    public Canvas mainCanvas;
    private Camera mainCamera;
    public Vector3 healthBarOffset = new Vector3(0, 1.5f, 0);
    public Vector3 expBarOffset = new Vector3(0, 1.2f, 0);
    
    [Header("Health UI")]
    public GameObject healthBarPrefab;
    public Slider healthBar;
    public TextMeshProUGUI healthText;

    [Header("Experience UI")]
    public ExpBarUI expBarUI;

    [Header("Weapon UI")]
    public Image weaponIcon;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponStats;

    [Header("Enemy UI")]
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI waveText;

    private EventSystem eventSystem;
    private Transform playerTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupUI();
    }

    private void SetupUI()
    {
        // Lấy camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Không tìm thấy Main Camera!");
            return;
        }

        // Tìm player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Không tìm thấy Player!");
            return;
        }

        // Thiết lập Canvas
        if (mainCanvas != null)
        {
            mainCanvas.renderMode = RenderMode.WorldSpace;
            mainCanvas.worldCamera = mainCamera;
        }

        // Đảm bảo có EventSystem
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystem = eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
        }

        // Kiểm tra ExpBarUI
        if (expBarUI == null)
        {
            Debug.LogError("ExpBarUI reference is missing in UIManager!");
            return;
        }

        // Thiết lập Health Bar
        SetupHealthBar();

        // Thiết lập Exp Bar
        SetupExpBar();
    }

    private void SetupHealthBar()
    {
        if (healthBar == null && healthBarPrefab != null)
        {
            GameObject healthBarObj = Instantiate(healthBarPrefab, mainCanvas.transform);
            healthBar = healthBarObj.GetComponent<Slider>();
            healthText = healthBarObj.GetComponentInChildren<TextMeshProUGUI>();

            if (healthBar != null)
            {
                healthBar.maxValue = 100;
                healthBar.value = 100;
            }
        }
        else if (healthBarPrefab == null)
        {
            Debug.LogError("Health Bar Prefab chưa được gán trong UIManager!");
        }
    }

    private void SetupExpBar()
    {
        if (expBarUI != null)
        {
            expBarUI.transform.SetParent(mainCanvas.transform, false);
        }
    }

    private void LateUpdate()
    {
        if (playerTransform == null || mainCamera == null) return;

        // Cập nhật vị trí Health Bar
        if (healthBar != null)
        {
            healthBar.transform.position = playerTransform.position + healthBarOffset;
            healthBar.transform.forward = mainCamera.transform.forward;
        }

        // Cập nhật vị trí Exp Bar
        if (expBarUI != null)
        {
            expBarUI.transform.position = playerTransform.position + expBarOffset;
            expBarUI.transform.forward = mainCamera.transform.forward;
        }
    }

    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(currentHealth)}/{Mathf.CeilToInt(maxHealth)}";
        }
    }

    public void UpdateExpUI(float currentExp, float expToNextLevel, int currentLevel)
    {
        if (expBarUI != null)
        {
            expBarUI.UpdateExpUI(currentExp, expToNextLevel, currentLevel);
        }
    }

    public void ShowExpGain(float expAmount)
    {
        if (expBarUI != null)
        {
            expBarUI.ShowExpGain(expAmount);
        }
    }

    public void UpdateWeaponUI(string name, Sprite icon, string stats)
    {
        if (weaponIcon != null)
        {
            weaponIcon.sprite = icon;
        }

        if (weaponName != null)
        {
            weaponName.text = name;
        }

        if (weaponStats != null)
        {
            weaponStats.text = stats;
        }
    }

    public void UpdateEnemyUI(int enemyCount, int currentWave)
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"Enemies: {enemyCount}";
        }

        if (waveText != null)
        {
            waveText.text = $"Wave: {currentWave}";
        }
    }
}