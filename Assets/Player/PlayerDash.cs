using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float minVelocityForDirection = 0.1f;
    
    [Header("Ghost Effect Settings")]
    [SerializeField] private float ghostSpawnTime = 0.016f; // 60fps để mượt hơn
    [SerializeField] private float ghostDuration = 0.15f; // Thời gian ngắn hơn để ghost biến mất nhanh hơn
    [SerializeField] private Color ghostColor = new Color(0.3f, 0.7f, 1f, 0.7f); // Màu xanh trong suốt
    private float ghostSpawnTimer;
    private GameObject ghostPrefab;
    
    [Header("References")]
    [SerializeField] private LayerMask dashLayer;

    private Rigidbody2D rb;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float cooldownTimeLeft;
    private int originalLayer;
    private Vector2 dashDirection;
    private Vector2 originalVelocity;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D không tìm thấy trên Player!");
            enabled = false;
            return;
        }

        originalLayer = gameObject.layer;
        CreateGhostPrefab();
    }

    private void CreateGhostPrefab()
    {
        ghostPrefab = new GameObject("PlayerGhost");
        SpriteRenderer ghostSprite = ghostPrefab.AddComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            ghostSprite.sprite = spriteRenderer.sprite;
            ghostSprite.sortingOrder = spriteRenderer.sortingOrder - 1;
        }
        
        GhostEffect ghostEffect = ghostPrefab.AddComponent<GhostEffect>();
        ghostEffect.duration = ghostDuration;
        ghostEffect.initialColor = ghostColor;
        
        ghostPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Xử lý cooldown
        if (!canDash)
        {
            cooldownTimeLeft -= Time.deltaTime;
            if (cooldownTimeLeft <= 0)
            {
                canDash = true;
            }
        }

        // Kiểm tra input dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartDash();
        }

        // Xử lý thời gian dash
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            
            ghostSpawnTimer -= Time.deltaTime;
            if (ghostSpawnTimer <= 0)
            {
                SpawnGhost();
                ghostSpawnTimer = ghostSpawnTime;
            }

            if (dashTimeLeft <= 0)
            {
                EndDash();
            }
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
        }
    }

    private void SpawnGhost()
    {
        if (ghostPrefab != null)
        {
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            ghost.SetActive(true);
            
            ghost.transform.localScale = transform.localScale;
            
            SpriteRenderer ghostSprite = ghost.GetComponent<SpriteRenderer>();
            if (ghostSprite != null && spriteRenderer != null)
            {
                ghostSprite.sprite = spriteRenderer.sprite;
                ghostSprite.flipX = spriteRenderer.flipX;
                ghostSprite.flipY = spriteRenderer.flipY;
            }
        }
    }

    void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
        cooldownTimeLeft = dashCooldown;
        ghostSpawnTimer = 0;

        Vector2 currentVelocity = rb.linearVelocity;
        if (currentVelocity.magnitude > minVelocityForDirection)
        {
            dashDirection = currentVelocity.normalized;
        }
        else
        {
            dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        gameObject.layer = LayerMask.NameToLayer("Invincible");
    }

    void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector2.zero;
        gameObject.layer = originalLayer;
    }

    // Hàm public để các script khác có thể kiểm tra trạng thái dash
    public bool IsDashing()
    {
        return isDashing;
    }

    // Hàm public để giảm thời gian hồi dash (có thể dùng cho power-up)
    public void ReduceDashCooldown(float reduction)
    {
        dashCooldown = Mathf.Max(0.1f, dashCooldown - reduction);
    }
}

// Thêm script mới cho hiệu ứng ghost
public class GhostEffect : MonoBehaviour
{
    public float duration = 0.15f;
    public Color initialColor = new Color(0.3f, 0.7f, 1f, 0.7f);
    private float timeLeft;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timeLeft = duration;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = initialColor;
        }
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(0, initialColor.a, timeLeft / duration);
            spriteRenderer.color = color;
        }
    }
}
