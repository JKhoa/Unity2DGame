using UnityEngine;

public class PointCollision : MonoBehaviour
{
    public float swingBoostForce = 20f; // Lực phóng mạnh khi đu dây

    [System.Obsolete]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)     
            {
                // Tính toán lực phóng dựa trên hướng chuyển động của player khi đu dây
                Vector2 boostDirection = new Vector2(rb.velocity.x * 0.5f, 1f).normalized;
                rb.velocity = Vector2.zero; // Reset vận tốc hiện tại
                rb.AddForce(boostDirection * swingBoostForce, ForceMode2D.Impulse);

                Debug.Log("Player boosted upwards after rope swing!");
            }
        }
    }
}
