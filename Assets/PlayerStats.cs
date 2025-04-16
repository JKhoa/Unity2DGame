using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Base Stats")]
    public float maxHealth = 100f;
    private float currentHealth;
    public float moveSpeed = 5f;
    public float damage = 10f;

    [Header("Level System")]
    public int currentLevel = 1;
    public float currentExp = 0f;
    public float expToNextLevel = 100f;
    public float expMultiplier = 1.5f; // Hệ số tăng exp cần thiết mỗi level
    public ExpBarUI expBarUI;

    [Header("Multipliers")]
    public float damageMultiplier = 1f;
    public float moveSpeedMultiplier = 1f;
    public float maxHealthMultiplier = 1f;

    [Header("Combat Stats")]
    public float attackSpeed = 1f;
    public float attackRange = 5f;

    [Header("References")]
    [SerializeField] private GameObject healthBarPrefab;
    private HealthBar healthBar;
    private Canvas mainCanvas;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent<float> onHealthChanged;
    public UnityEvent<float> onMaxHealthChanged;

    [Header("Death Effects")]
    [SerializeField] private float deathDelay = 1f; // Thời gian delay trước khi reset
    [SerializeField] private GameObject deathEffectPrefab; // Optional: hiệu ứng khi chết
    private bool isDead = false;

    private void Awake()
    {
        // Đặt instance cho singleton
        Instance = this;
        
        // Khởi tạo slider nếu có
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
        }
        
        // Đăng ký sự kiện khi scene được tải
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    // Được gọi khi scene được tải
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[PlayerStats] Scene đã được tải, khởi tạo lại thanh máu");
        
        // Đặt lại các giá trị
        isDead = false;
        currentHealth = maxHealth;
        
        // Đảm bảo các component được bật lại
        if (GetComponent<PlayerShooting>() != null)
            GetComponent<PlayerShooting>().enabled = true;
        
        if (GetComponent<Rigidbody2D>() != null)
            GetComponent<Rigidbody2D>().simulated = true;
        
        // Tìm Canvas trong scene mới và đợi 1 frame để đảm bảo tất cả các object đã được tải
        StartCoroutine(InitializeHealthBarAfterDelay());
    }

    private IEnumerator InitializeHealthBarAfterDelay()
    {
        yield return null; // Đợi 1 frame

        // Tìm Canvas
        mainCanvas = Object.FindFirstObjectByType<Canvas>();
        
        // Khởi tạo lại thanh máu
        if (mainCanvas != null)
        {
            InitializeHealthBar();
        }
        else
        {
            Debug.LogError("[PlayerStats] Không tìm thấy Canvas sau khi load scene!");
        }
    }
    
    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện khi đối tượng bị hủy
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
        
        // Cập nhật các thành phần UI
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        
        onMaxHealthChanged?.Invoke(maxHealth);
        onHealthChanged?.Invoke(currentHealth);
        
        // Tìm Canvas chính
        mainCanvas = Object.FindFirstObjectByType<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError("[PlayerStats] Không tìm thấy Canvas trong scene!");
            return;
        }

        // Khởi tạo thanh máu
        InitializeHealthBar();

        // Khởi tạo ExpBarUI
        if (expBarUI == null)
        {
            expBarUI = FindObjectOfType<ExpBarUI>();
            if (expBarUI == null)
            {
                Debug.LogError("[PlayerStats] Không tìm thấy ExpBarUI!");
            }
        }
        
        // Cập nhật UI exp ban đầu
        UpdateExpUI();
        
        Debug.Log($"[PlayerStats] Bắt đầu với máu: {currentHealth}/{maxHealth}, Level: {currentLevel}, Exp: {currentExp}/{expToNextLevel}");
    }
    
    private void InitializeHealthBar()
    {
        // Kiểm tra nếu đã có reference đến healthSlider
        if (healthSlider == null)
        {
            // Nếu có prefab thì tạo mới
            if (healthBarPrefab != null)
            {
                GameObject healthBarObj = Instantiate(healthBarPrefab, transform);
                healthBarObj.transform.localPosition = new Vector3(0, 1.5f, 0);
                healthBarObj.transform.localScale = new Vector3(0.01f, 0.01f, 1);
                
                healthSlider = healthBarObj.GetComponent<Slider>();
                healthText = healthBarObj.GetComponentInChildren<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("[PlayerStats] HealthBarPrefab là null! Không thể tạo thanh máu.");
                return;
            }
        }

        // Thiết lập giá trị ban đầu
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        UpdateHealthUI();
    }

    private void LateUpdate()
    {
        if (healthSlider != null)
        {
            // Làm cho health bar luôn quay về phía camera
            healthSlider.transform.forward = Camera.main.transform.forward;
        }
    }

    public void TakeDamage(float damage)
    {
        // Không xử lý sát thương nếu đã chết
        if (isDead) 
        {
            Debug.Log("[PlayerStats] Không thể nhận sát thương vì đã chết");
            return;
        }
        
        // Tính toán giá trị máu mới
        float oldHealth = currentHealth;
        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"[PlayerStats] Nhân vật nhận {damage} sát thương. Máu: {currentHealth}/{maxHealth}");
        
        // Kiểm tra xem máu có thay đổi không
        if (Mathf.Approximately(oldHealth, currentHealth))
        {
            Debug.Log("[PlayerStats] Máu không thay đổi, bỏ qua cập nhật UI");
            return;
        }
        
        // Cập nhật thanh máu UI
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogWarning("[PlayerStats] Thanh máu là null khi nhận sát thương! Thử khởi tạo lại...");
            InitializeHealthBar();
            
            // Thử cập nhật lại sau khi khởi tạo
            if (healthBar != null)
            {
                healthBar.UpdateHealth(currentHealth, maxHealth);
            }
        }
        
        // Cập nhật slider UI
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        
        // Kích hoạt sự kiện máu thay đổi
        onHealthChanged?.Invoke(currentHealth);

        // Kiểm tra tử vong
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
        
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        
        onHealthChanged?.Invoke(currentHealth);
    }

    void Die()
    {
        if (isDead) return; // Tránh gọi Die nhiều lần
        isDead = true;

        // Thông báo
        Debug.Log("[PlayerStats] Player died!");

        // Vô hiệu hóa player
        DisablePlayer();

        // Hiệu ứng chết (nếu có)
        PlayDeathEffect();

        // Gọi event
        onDeath?.Invoke();

        // Reset game sau delay
        Invoke("ResetGame", deathDelay);
    }

    private void DisablePlayer()
    {
        // Vô hiệu hóa các component
        if (GetComponent<PlayerShooting>() != null)
            GetComponent<PlayerShooting>().enabled = false;
        
        if (GetComponent<Rigidbody2D>() != null)
            GetComponent<Rigidbody2D>().simulated = false;
        
        if (GetComponent<Collider2D>() != null)
            GetComponent<Collider2D>().enabled = false;

        // Ẩn sprite
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().enabled = false;
    }

    private void PlayDeathEffect()
    {
        if (deathEffectPrefab != null)
        {
            GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, deathDelay);
        }
    }

    private void ResetGame()
    {
        // Đảm bảo hủy thanh máu cũ trước khi reload scene
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
            healthBar = null;
        }

        // Reset các giá trị
        isDead = false;
        currentHealth = maxHealth;

        // Reload scene hiện tại
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra va chạm với Lava
        if (other.CompareTag("Lava"))
        {
            Die();
        }
        // Kiểm tra va chạm với Enemy
        else if (other.CompareTag("Enemy"))
        {
            // Nhận exp khi tiêu diệt enemy
            GainExp(20f);
        }
    }

    public void ResetStats()
    {
        currentHealth = maxHealth;
        damageMultiplier = 1f;
        moveSpeedMultiplier = 1f;
        maxHealthMultiplier = 1f;
        
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
        
        onHealthChanged?.Invoke(currentHealth);
        onMaxHealthChanged?.Invoke(maxHealth);
    }

    public void UpdateMaxHealth()
    {
        float oldMaxHealth = maxHealth;
        maxHealth = 100f * maxHealthMultiplier;
        
        // Điều chỉnh máu hiện tại theo tỷ lệ
        if (oldMaxHealth > 0)
        {
            currentHealth = (currentHealth / oldMaxHealth) * maxHealth;
        }
        
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
        
        onMaxHealthChanged?.Invoke(maxHealth);
    }

    public void IncreaseMaxHealth(float amount)
    {
        float healthPercent = currentHealth / maxHealth;
        maxHealth += amount;
        currentHealth = maxHealth * healthPercent;
        
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }

        onHealthChanged?.Invoke(currentHealth);
        onMaxHealthChanged?.Invoke(maxHealth);
    }

    public void IncreaseDamage(float amount)
    {
        damage += amount;
    }

    public void IncreaseAttackSpeed(float amount)
    {
        attackSpeed += amount;
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void IncreaseAttackRange(float amount)
    {
        attackRange += amount;
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(currentHealth)}/{Mathf.CeilToInt(maxHealth)}";
        }
    }

    public void GainExp(float amount)
    {
        if (isDead) return;

        float expGained = amount;
        currentExp += expGained;

        // Hiển thị exp gain
        if (expBarUI != null)
        {
            expBarUI.ShowExpGain(expGained);
        }

        // Kiểm tra level up
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
            expToNextLevel *= expMultiplier;
        }

        UpdateExpUI();
        Debug.Log($"[PlayerStats] Nhận {expGained} exp. Hiện tại: {currentExp}/{expToNextLevel}");
    }

    private void LevelUp()
    {
        currentLevel++;
        
        // Tăng chỉ số khi lên level
        maxHealth += 10f;
        damage += 5f;
        
        // Cập nhật máu hiện tại
        float healthPercent = currentHealth / maxHealth;
        maxHealth *= 1.1f; // Tăng 10% máu tối đa
        currentHealth = maxHealth * healthPercent;
        
        UpdateHealthUI();
        UpdateExpUI();
        
        Debug.Log($"[PlayerStats] Lên level {currentLevel}! Máu tối đa mới: {maxHealth}");
    }

    private void UpdateExpUI()
    {
        if (expBarUI != null)
        {
            expBarUI.UpdateExpUI(currentExp, expToNextLevel, currentLevel);
        }
    }
}