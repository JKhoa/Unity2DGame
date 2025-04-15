using UnityEngine;

public class ShieldBehavior : MonoBehaviour
{
    public float damage = 10f;
    public float radius = 2f;
    public float rotationSpeed = 90f;
    public LayerMask enemyLayer;

    void Start()
    {
        transform.localScale = Vector3.one * radius;
    }

    void Update()
    {
        // Xoay shield
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Kiểm tra va chạm với enemy
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage * Time.deltaTime);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
} 