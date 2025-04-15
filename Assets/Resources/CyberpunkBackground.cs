using UnityEngine;

// Script để tạo hình nền gradient tím-hồng với hiệu ứng mạng lưới
public static class CyberpunkBackground
{
    // Tạo texture nền gradient tím-hồng
    public static Texture2D CreateBackgroundTexture(int width, int height)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        
        // Màu gradient
        Color purpleColor = new Color(0.3f, 0.0f, 0.5f, 0.9f); // Tím đậm
        Color pinkColor = new Color(0.8f, 0.2f, 0.6f, 0.9f);   // Hồng
        
        // Tạo gradient từ tím sang hồng
        for (int y = 0; y < height; y++)
        {
            float t = (float)y / height;
            Color gradientColor = Color.Lerp(purpleColor, pinkColor, t);
            
            for (int x = 0; x < width; x++)
            {
                texture.SetPixel(x, y, gradientColor);
            }
        }
        
        // Vẽ các đường mạng lưới
        DrawNetworkLines(texture, width, height);
        
        // Vẽ các điểm nút
        DrawNetworkNodes(texture, width, height);
        
        texture.Apply();
        return texture;
    }
    
    // Vẽ các đường mạng lưới
    private static void DrawNetworkLines(Texture2D texture, int width, int height)
    {
        // Tạo màu cho đường mạng lưới
        Color lineColor = new Color(1f, 1f, 1f, 0.2f);
        
        // Số lượng đường
        int numLines = 30;
        
        // Tạo các điểm ngẫu nhiên
        Vector2[] points = new Vector2[numLines];
        for (int i = 0; i < numLines; i++)
        {
            points[i] = new Vector2(Random.Range(0, width), Random.Range(0, height));
        }
        
        // Vẽ các đường nối giữa các điểm
        for (int i = 0; i < numLines; i++)
        {
            for (int j = i + 1; j < numLines; j++)
            {
                // Chỉ vẽ một số đường, không vẽ tất cả các kết nối có thể
                if (Random.Range(0, 10) < 3)
                {
                    DrawLine(texture, (int)points[i].x, (int)points[i].y, (int)points[j].x, (int)points[j].y, lineColor);
                }
            }
        }
    }
    
    // Vẽ các điểm nút
    private static void DrawNetworkNodes(Texture2D texture, int width, int height)
    {
        // Tạo màu cho điểm nút
        Color nodeColor = new Color(1f, 1f, 1f, 0.8f);
        
        // Số lượng điểm nút
        int numNodes = 50;
        
        // Vẽ các điểm nút ngẫu nhiên
        for (int i = 0; i < numNodes; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            
            // Vẽ điểm nút (một điểm trắng nhỏ)
            DrawCircle(texture, x, y, Random.Range(1, 3), nodeColor);
        }
    }
    
    // Vẽ đường thẳng giữa hai điểm
    private static void DrawLine(Texture2D texture, int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;
        
        while (true)
        {
            if (x0 >= 0 && x0 < texture.width && y0 >= 0 && y0 < texture.height)
            {
                texture.SetPixel(x0, y0, color);
            }
            
            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
    
    // Vẽ hình tròn
    private static void DrawCircle(Texture2D texture, int x0, int y0, int radius, Color color)
    {
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * x + y * y <= radius * radius)
                {
                    int drawX = x0 + x;
                    int drawY = y0 + y;
                    if (drawX >= 0 && drawX < texture.width && drawY >= 0 && drawY < texture.height)
                    {
                        texture.SetPixel(drawX, drawY, color);
                    }
                }
            }
        }
    }
}
