using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public Vector3 direction;
    public bool isPiercing = false;
    public int pierceCount = 1;
    private int currentPierceCount = 0;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }

        // Tự hủy sau 5 giây
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (!isPiercing || ++currentPierceCount >= pierceCount)
            {
                Destroy(gameObject);
            }
        }
    }
} 