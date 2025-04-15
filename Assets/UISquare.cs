using UnityEngine;
using System.IO;

public class UISquareGenerator : MonoBehaviour
{
    void Start()
    {
        // Tạo texture 100x100 pixel màu trắng
        Texture2D texture = new Texture2D(100, 100);
        Color[] colors = new Color[100 * 100];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        texture.SetPixels(colors);
        texture.Apply();

        // Chuyển texture thành PNG và lưu vào file
        byte[] bytes = texture.EncodeToPNG();
        string filePath = Path.Combine(Application.dataPath, "UISquare.png");
        File.WriteAllBytes(filePath, bytes);

        // Thông báo hoàn thành
        Debug.Log("Đã tạo sprite hình vuông tại: " + filePath);

        // Tự hủy script sau khi hoàn thành
        Destroy(this);
    }
} 