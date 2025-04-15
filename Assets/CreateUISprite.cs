using UnityEngine;
using UnityEngine.UI;

public class CreateUISprite : MonoBehaviour
{
    void Start()
    {
        // Tạo texture trắng
        Texture2D tex = new Texture2D(100, 100);
        Color[] colors = new Color[100 * 100];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        tex.SetPixels(colors);
        tex.Apply();

        // Tạo sprite từ texture
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 100, 100), new Vector2(0.5f, 0.5f), 100);

        // Gán sprite vào Image component
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
        }

        // Tự hủy script sau khi hoàn thành
        Destroy(this);
    }
} 