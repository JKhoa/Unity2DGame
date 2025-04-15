using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float minVelocityForDirection = 0.1f; // Ngưỡng tốc độ tối thiểu để xác định hướng
    
    [Header("References")]
    [SerializeField] private TrailRenderer dashTrail;
    [SerializeField] private LayerMask dashLayer;

    private Rigidbody2D rb;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float cooldownTimeLeft;
    private int originalLayer;
    private Vector2 dashDirection;
    private Vector2 originalVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D không tìm thấy trên Player!");
            enabled = false;
            return;
        }

        originalLayer = gameObject.layer;

        // Tạo TrailRenderer nếu chưa có
        if (dashTrail == null)
        {
            dashTrail = gameObject.AddComponent<TrailRenderer>();
            dashTrail.time = 0.2f;
            dashTrail.startWidth = 0.5f;
            dashTrail.endWidth = 0f;
            dashTrail.enabled = false;
        }
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
            // Kết hợp vận tốc gốc với vận tốc dash
            rb.linearVelocity = originalVelocity + (dashDirection * dashSpeed);
        }
    }

    void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
        cooldownTimeLeft = dashCooldown;

        // Lưu lại vận tốc gốc của player
        originalVelocity = rb.linearVelocity;

        // Xác định hướng dash dựa trên vận tốc hiện tại
        Vector2 currentVelocity = rb.linearVelocity;
        if (currentVelocity.magnitude > minVelocityForDirection)
        {
            dashDirection = currentVelocity.normalized;
        }
        else
        {
            dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        // Bật hiệu ứng trail
        if (dashTrail != null)
        {
            dashTrail.enabled = true;
        }

        // Chuyển layer để miễn nhiễm sát thương
        gameObject.layer = LayerMask.NameToLayer("Invincible");
    }

    void EndDash()
    {
        isDashing = false;
        
        // Khôi phục lại vận tốc gốc thay vì đặt về 0
        rb.linearVelocity = originalVelocity;

        // Tắt hiệu ứng trail
        if (dashTrail != null)
        {
            dashTrail.enabled = false;
        }

        // Trả về layer ban đầu
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
