using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Base Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float moveSpeed = 5f;
    public float damage = 10f;

    [Header("Multipliers")]
    public float damageMultiplier = 1f;
    public float moveSpeedMultiplier = 1f;
    public float maxHealthMultiplier = 1f;

    [Header("Combat Stats")]
    public float attackSpeed = 1f;
    public float attackRange = 5f;

    [Header("References")]
    public GameObject healthBarPrefab;
    private HealthBar healthBar;
    private Canvas mainCanvas;
    public Slider healthSlider;
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

    void Start()
    {
        // Luôn đặt máu hiện tại bằng máu tối đa khi bắt đầu
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
        
        Debug.Log($"[PlayerStats] Bắt đầu với máu: {currentHealth}/{maxHealth}");
    }
    
    private void InitializeHealthBar()
    {
        // Xóa thanh máu cũ nếu có để tránh trùng lặp
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
            healthBar = null;
        }
        
        // Kiểm tra các tham chiếu cần thiết
        if (healthBarPrefab == null)
        {
            Debug.LogError("[PlayerStats] HealthBarPrefab là null! Không thể tạo thanh máu.");
            return;
        }
        
        if (mainCanvas == null)
        {
            Debug.LogError("[PlayerStats] Main Canvas là null! Không thể tạo thanh máu.");
            return;
        }
        
        // Tạo thanh máu mới
        GameObject healthBarObj = Instantiate(healthBarPrefab, mainCanvas.transform);
        healthBar = healthBarObj.GetComponent<HealthBar>();
        
        if (healthBar != null)
        {
            // Thiết lập thanh máu
            healthBar.SetTarget(transform);
            healthBar.UpdateHealth(currentHealth, maxHealth);
            
            // Cập nhật slider UI nếu có
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }
            
            Debug.Log($"[PlayerStats] Đã khởi tạo thanh máu. Máu: {currentHealth}/{maxHealth}");
        }
        else
        {
            Debug.LogError("[PlayerStats] Không tìm thấy component HealthBar trên prefab!");
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

    // Thêm hàm để kiểm tra va chạm với lava
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lava"))
        {
            Die();
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
}