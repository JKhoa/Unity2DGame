using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public static PlayerShooting Instance { get; private set; }

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFireTime;

    [Header("Shield")]
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float shieldDamage = 10f;
    [SerializeField] private float shieldRadius = 2f;
    [SerializeField] private float shieldRotationSpeed = 90f;
    [SerializeField] private KeyCode shieldToggleKey = KeyCode.Q;
    private bool hasShield;
    private GameObject activeShield;

    [Header("Triple Shot Settings")]
    [SerializeField] private float defaultTripleShotDamageMultiplier = 1f;
    [SerializeField] private float defaultTripleShotAngle = 15f;
    private bool hasTripleShot = false;
    private float tripleShotDamageMultiplier;
    private float tripleShotAngle;

    [Header("Double Shot Settings")]
    [SerializeField] private float defaultDoubleShotDamageMultiplier = 1f;
    [SerializeField] private float doubleShotOffset = 0.2f;
    private bool hasDoubleShot = false;
    private float doubleShotDamageMultiplier;

    [Header("Piercing Shot Settings")]
    [SerializeField] private int defaultMaxPierceCount = 1;
    private bool hasPiercingShot = false;
    private int maxPierceCount;

    private void Awake()
    {
        SetupSingleton();
        InitializeDefaultValues();
    }

    private void SetupSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDefaultValues()
    {
        tripleShotDamageMultiplier = defaultTripleShotDamageMultiplier;
        tripleShotAngle = defaultTripleShotAngle;
        doubleShotDamageMultiplier = defaultDoubleShotDamageMultiplier;
        maxPierceCount = defaultMaxPierceCount;
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            Debug.Log("Game is paused! TimeScale = 0");
            return;
        }
        
        HandleInput();
    }

    void HandleInput()
    {
        HandleShootingInput();
        HandleShieldInput();
    }

    void HandleShootingInput()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        UpdateWeaponRotation();
    }

    void HandleShieldInput()
    {
        if (Input.GetKeyDown(shieldToggleKey))
        {
            ToggleShield();
        }
    }

    void UpdateWeaponRotation()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        if (hasTripleShot)
        {
            ShootTriple();
        }
        else if (hasDoubleShot)
        {
            ShootDouble();
        }
        else
        {
            ShootSingle();
        }
    }

    void ShootSingle()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        if (bulletBehavior != null)
        {
            bulletBehavior.direction = firePoint.right;
            bulletBehavior.damage = GetDamage();
            if (hasPiercingShot)
            {
                bulletBehavior.isPiercing = true;
                bulletBehavior.pierceCount = maxPierceCount;
            }
        }
    }

    void ShootDouble()
    {
        Vector3 offset = Vector3.Cross(firePoint.right, Vector3.forward) * doubleShotOffset;
        
        GameObject bullet1 = Instantiate(bulletPrefab, firePoint.position + offset, firePoint.rotation);
        SetupBullet(bullet1, firePoint.right, doubleShotDamageMultiplier);

        GameObject bullet2 = Instantiate(bulletPrefab, firePoint.position - offset, firePoint.rotation);
        SetupBullet(bullet2, firePoint.right, doubleShotDamageMultiplier);
    }

    void ShootTriple()
    {
        // Viên đạn giữa
        GameObject bulletMiddle = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        SetupBullet(bulletMiddle, firePoint.right, tripleShotDamageMultiplier);

        // Tính góc cho các viên đạn bên
        Quaternion leftRot = Quaternion.Euler(0, 0, -tripleShotAngle);
        Quaternion rightRot = Quaternion.Euler(0, 0, tripleShotAngle);

        // Viên đạn trái
        GameObject bulletLeft = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * leftRot);
        SetupBullet(bulletLeft, bulletLeft.transform.right, tripleShotDamageMultiplier);

        // Viên đạn phải
        GameObject bulletRight = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * rightRot);
        SetupBullet(bulletRight, bulletRight.transform.right, tripleShotDamageMultiplier);
    }

    void SetupBullet(GameObject bullet, Vector3 direction, float damageMultiplier)
    {
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        if (bulletBehavior != null)
        {
            bulletBehavior.direction = direction;
            bulletBehavior.damage = GetDamage() * damageMultiplier;
            if (hasPiercingShot)
            {
                bulletBehavior.isPiercing = true;
                bulletBehavior.pierceCount = maxPierceCount;
            }
        }
    }

    float GetDamage()
    {
        PlayerStats playerStats = GetComponent<PlayerStats>();
        return playerStats != null ? playerStats.damage : 10f;
    }

    void HandleShieldControl()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleShield();
        }
    }

    // Shield methods
    public void EnableShield(float duration)
    {
        if (activeShield == null && shieldPrefab != null)
        {
            activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            activeShield.transform.parent = transform;
            hasShield = true;

            ShieldBehavior shieldBehavior = activeShield.GetComponent<ShieldBehavior>();
            if (shieldBehavior != null)
            {
                shieldBehavior.damage = shieldDamage;
                shieldBehavior.radius = shieldRadius;
                shieldBehavior.rotationSpeed = shieldRotationSpeed;
            }

            Invoke("DisableShield", duration);
        }
    }

    public void SetShieldDamage(float damage)
    {
        shieldDamage = damage;
        if (activeShield != null)
        {
            ShieldBehavior shieldBehavior = activeShield.GetComponent<ShieldBehavior>();
            if (shieldBehavior != null)
            {
                shieldBehavior.damage = damage;
            }
        }
    }

    public void SetShieldRadius(float radius)
    {
        shieldRadius = radius;
        if (activeShield != null)
        {
            ShieldBehavior shieldBehavior = activeShield.GetComponent<ShieldBehavior>();
            if (shieldBehavior != null)
            {
                shieldBehavior.radius = radius;
            }
        }
    }

    public void SetShieldRotationSpeed(float speed)
    {
        shieldRotationSpeed = speed;
        if (activeShield != null)
        {
            ShieldBehavior shieldBehavior = activeShield.GetComponent<ShieldBehavior>();
            if (shieldBehavior != null)
            {
                shieldBehavior.rotationSpeed = speed;
            }
        }
    }

    void DisableShield()
    {
        if (activeShield != null)
        {
            Destroy(activeShield);
            activeShield = null;
            hasShield = false;
        }
    }

    void ToggleShield()
    {
        if (hasShield)
        {
            DisableShield();
        }
        else
        {
            EnableShield(5f);
        }
    }

    public bool HasActiveShield()
    {
        return hasShield;
    }

    // Upgrade methods
    public void EnableTripleShot(float damageMultiplier, float angle)
    {
        hasTripleShot = true;
        tripleShotDamageMultiplier = damageMultiplier;
        tripleShotAngle = angle;
        hasDoubleShot = false;
    }

    public void EnableDoubleShot(float damageMultiplier)
    {
        hasDoubleShot = true;
        doubleShotDamageMultiplier = damageMultiplier;
        hasTripleShot = false;
    }

    public void EnablePiercingShot(int pierceCount)
    {
        hasPiercingShot = true;
        maxPierceCount = pierceCount;
    }
} 