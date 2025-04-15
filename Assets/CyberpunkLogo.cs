using UnityEngine;

// Script để tạo texture logo Cyberpunk
public static class CyberpunkLogo
{
    // Tạo texture logo Cyberpunk
    public static Texture2D CreateLogo()
    {
        // Tạo texture mới với kích thước 300x100
        Texture2D logoTexture = new Texture2D(300, 100, TextureFormat.RGBA32, false);
        
        // Màu vàng Cyberpunk
        Color cyberpunkYellow = new Color(1f, 0.8f, 0f);
        
        // Tạo nền trong suốt
        Color transparent = new Color(0, 0, 0, 0);
        for (int x = 0; x < logoTexture.width; x++)
        {
            for (int y = 0; y < logoTexture.height; y++)
            {
                logoTexture.SetPixel(x, y, transparent);
            }
        }
        
        // Vẽ chữ "CYBERPUNK" bằng các hình chữ nhật
        DrawCyberpunkText(logoTexture, cyberpunkYellow);
        
        // Áp dụng các thay đổi
        logoTexture.Apply();
        
        return logoTexture;
    }
    
    // Vẽ chữ CYBERPUNK bằng các hình chữ nhật
    private static void DrawCyberpunkText(Texture2D texture, Color color)
    {
        // Vẽ chữ C
        DrawRect(texture, 10, 30, 20, 10, color);
        DrawRect(texture, 10, 30, 10, 40, color);
        DrawRect(texture, 10, 30, 20, 70, color);
        
        // Vẽ chữ Y
        DrawRect(texture, 40, 50, 10, 40, color);
        DrawRect(texture, 60, 70, 10, 40, color);
        DrawRect(texture, 40, 70, 10, 70, color);
        
        // Vẽ chữ B
        DrawRect(texture, 80, 90, 10, 70, color);
        DrawRect(texture, 90, 100, 10, 30, color);
        DrawRect(texture, 90, 100, 10, 70, color);
        DrawRect(texture, 90, 100, 10, 50, color);
        
        // Vẽ chữ E
        DrawRect(texture, 110, 120, 10, 70, color);
        DrawRect(texture, 120, 130, 10, 30, color);
        DrawRect(texture, 120, 130, 10, 50, color);
        DrawRect(texture, 120, 130, 10, 70, color);
        
        // Vẽ chữ R
        DrawRect(texture, 140, 150, 10, 70, color);
        DrawRect(texture, 150, 160, 10, 30, color);
        DrawRect(texture, 150, 160, 10, 50, color);
        DrawRect(texture, 160, 170, 10, 60, color);
        
        // Vẽ chữ P
        DrawRect(texture, 180, 190, 10, 70, color);
        DrawRect(texture, 190, 200, 10, 30, color);
        DrawRect(texture, 190, 200, 10, 50, color);
        
        // Vẽ chữ U
        DrawRect(texture, 210, 220, 10, 70, color);
        DrawRect(texture, 230, 240, 10, 70, color);
        DrawRect(texture, 220, 230, 10, 30, color);
        
        // Vẽ chữ N
        DrawRect(texture, 250, 260, 10, 70, color);
        DrawRect(texture, 260, 270, 10, 60, color);
        DrawRect(texture, 270, 280, 10, 50, color);
        DrawRect(texture, 280, 290, 10, 70, color);
        
        // Vẽ chữ K
        DrawRect(texture, 300, 310, 10, 70, color);
        DrawRect(texture, 310, 320, 10, 50, color);
        DrawRect(texture, 320, 330, 10, 30, color);
        DrawRect(texture, 320, 330, 10, 70, color);
    }
    
    // Vẽ hình chữ nhật
    private static void DrawRect(Texture2D texture, int x1, int x2, int y1, int y2, Color color)
    {
        for (int x = x1; x < x2 && x < texture.width; x++)
        {
            for (int y = y1; y < y2 && y < texture.height; y++)
            {
                texture.SetPixel(x, y, color);
            }
        }
    }
}
