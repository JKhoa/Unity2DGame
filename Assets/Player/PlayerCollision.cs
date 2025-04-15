using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public int playerScore = 0; // Biến để lưu trữ điểm số của người chơi
    public float boostForceY = 15f; // Lực phóng theo trục Y
    public float boostForceX = 15f;  // Lực phóng theo trục X

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    [System.Obsolete]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(collision.gameObject);
            playerScore++;
            Debug.Log("Player Score: " + playerScore);
        }

        if (collision.gameObject.CompareTag("Point"))

        {
            Destroy(collision.gameObject);
            playerScore++;

            // Phóng mạnh lên trên theo trục X và Y, tính từ vị trí hiện tại
            rb.velocity = Vector2.zero; // Reset vận tốc trước khi áp dụng
            rb.AddForce(new Vector2(boostForceX, boostForceY), ForceMode2D.Impulse);

            Debug.Log("Player Score: " + playerScore);
        }
    }
}