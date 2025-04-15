using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float fireRate = 0.5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float bulletDamage = 10f;
    public bool isTripleShot = false;
    public bool isDoubleShot = false;
    public bool isPiercingShot = false;

    private float nextFireTime;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Di chuyển
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection.Normalize();

        // Bắn
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    void Shoot()
    {
        if (isTripleShot)
        {
            // Bắn 3 viên đạn
            CreateBullet(0f);
            CreateBullet(15f);
            CreateBullet(-15f);
        }
        else if (isDoubleShot)
        {
            // Bắn 2 viên đạn
            CreateBullet(5f);
            CreateBullet(-5f);
        }
        else
        {
            // Bắn 1 viên đạn
            CreateBullet(0f);
        }
    }

    void CreateBullet(float angle)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.Rotate(0, 0, angle);
        
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        if (bulletBehavior != null)
        {
            bulletBehavior.damage = bulletDamage;
            bulletBehavior.isPiercing = isPiercingShot;
        }
    }

    public void TakeDamage(float damage)
    {
        // Xử lý nhận sát thương
        PlayerStats playerStats = GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);
        }
    }
} 