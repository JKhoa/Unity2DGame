using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            // Khi người chơi chạm vào lava, tải lại scene để kết thúc trò chơi
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
