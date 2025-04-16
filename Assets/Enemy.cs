using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float damage = 10f;
    public float expValue = 10f;
    public float damageInterval = 0.5f; // Thời gian giữa các lần gây sát thương
    private float nextDamageTime;

    [Header("References")]
    public GameObject healthBarPrefab;
    private HealthBar healthBar;
    private Canvas mainCanvas;

    private void Start()
    {
        currentHealth = maxHealth;
        
        // Tìm Canvas chính trong scene
        mainCanvas = Object.FindFirstObjectByType<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError($"[{gameObject.name}] Main Canvas not found in scene!");
            return;
        }

        InitializeHealthBar();
        
        // Đảm bảo tag được set đúng
        if (gameObject.tag != "Enemy")
        {
            Debug.LogWarning($"[{gameObject.name}] Enemy tag not set! Setting it now...");
            gameObject.tag = "Enemy";
        }
    }

    private void InitializeHealthBar()
    {
        if (healthBarPrefab == null)
        {
            Debug.LogError($"[{gameObject.name}] HealthBar Prefab not assigned!");
            return;
        }

        if (mainCanvas != null)
        {
            // Tạo health bar và gán vào canvas
            GameObject healthBarObj = Instantiate(healthBarPrefab, mainCanvas.transform);
            healthBar = healthBarObj.GetComponent<HealthBar>();
            
            if (healthBar != null)
            {
                healthBar.SetTarget(transform);
                healthBar.UpdateHealth(currentHealth, maxHealth);
                Debug.Log($"[{gameObject.name}] HealthBar initialized. Current Health: {currentHealth}/{maxHealth}");
            }
            else
            {
                Debug.LogError($"[{gameObject.name}] HealthBar component not found on prefab!");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"[{gameObject.name}] Taking {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] HealthBar is null when taking damage!");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"[{gameObject.name}] Died!");
        
        // Thêm exp cho player
        PlayerStats playerStats = Object.FindFirstObjectByType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.GainExp(expValue);
            Debug.Log($"[{gameObject.name}] Giving {expValue} exp to player");
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] PlayerStats not found when trying to give exp!");
        }

        // Hiển thị hiệu ứng exp
        ShowExpGainEffect();

        // Xóa health bar
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        // Xóa enemy
        Destroy(gameObject);
    }
    
    // Hiển thị hiệu ứng nhận EXP
    private void ShowExpGainEffect()
    {
        // Tìm ExpBarUI
        ExpBarUI expBarUI = Object.FindFirstObjectByType<ExpBarUI>();
        if (expBarUI != null)
        {
            // Hiển thị animation EXP tăng
            expBarUI.ShowExpGain(expValue);
        }
        
        // Tạo text hiệu ứng tại vị trí enemy
        GameObject expTextObj = new GameObject("ExpGainWorldText");
        expTextObj.transform.position = transform.position + Vector3.up * 0.5f;
        
        // Thêm TextMesh để hiển thị trong world space
        TextMesh textMesh = expTextObj.AddComponent<TextMesh>();
        textMesh.text = $"+{expValue} EXP";
        textMesh.fontSize = 14;
        textMesh.color = Color.yellow;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.characterSize = 0.1f;
        
        // Thêm script để animation text bay lên và mờ dần
        StartCoroutine(AnimateExpText(expTextObj));
    }
    
    // Animation cho text EXP bay lên và mờ dần
    private System.Collections.IEnumerator AnimateExpText(GameObject textObj)
    {
        TextMesh textMesh = textObj.GetComponent<TextMesh>();
        Vector3 startPos = textObj.transform.position;
        float duration = 1.5f;
        float startTime = Time.time;
        
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            
            // Di chuyển lên trên
            textObj.transform.position = startPos + Vector3.up * t * 1.5f;
            
            // Mờ dần
            Color color = textMesh.color;
            color.a = 1 - t;
            textMesh.color = color;
            
            yield return null;
        }
        
        Destroy(textObj);
    }

    private void OnDestroy()
    {
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    // Thêm hàm này để debug collision với đạn
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[{gameObject.name}] Trigger entered by: {other.gameObject.name} with tag: {other.gameObject.tag}");
        
        if (other.CompareTag("Bullet"))
        {
            BulletBehavior bullet = other.GetComponent<BulletBehavior>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
                Debug.Log($"[{gameObject.name}] Hit by bullet with damage: {bullet.damage}");
            }
        }
    }
} 