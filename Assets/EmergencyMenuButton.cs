using UnityEngine;

public class EmergencyMenuButton : MonoBehaviour
{
    private bool showMenu = false;
    private Rect buttonRect;
    private Rect menuRect;
    private GUIStyle buttonStyle;
    private GUIStyle menuStyle;
    private GUIStyle titleStyle;
    private GUIStyle menuButtonStyle;
    
    // Thêm biến để kiểm soát vị trí nút
    [Header("Button Settings")]
    public bool showInTopRight = true;
    public float buttonWidth = 100;
    public float buttonHeight = 40;
    public float margin = 10;
    public KeyCode menuKey = KeyCode.M;
    
    // Thêm biến để theo dõi nút được hover
    private int hoveredButton = -1;

    void Start()
    {
        // Vị trí và kích thước nút - đặt ở góc trên bên phải
        if (showInTopRight)
        {
            buttonRect = new Rect(Screen.width - buttonWidth - margin, margin, buttonWidth, buttonHeight);
        }
        else
        {
            buttonRect = new Rect(margin, margin, buttonWidth, buttonHeight);
        }
        
        // Vị trí và kích thước menu
        menuRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 200, 300, 400);
        
        // Tạo style cho nút
        buttonStyle = new GUIStyle();
        buttonStyle.normal.background = CreateColorTexture(new Color(1, 0, 0, 1));
        buttonStyle.hover.background = CreateColorTexture(new Color(1, 0.3f, 0.3f, 1));
        buttonStyle.active.background = CreateColorTexture(new Color(0.8f, 0, 0, 1));
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.hover.textColor = Color.white;
        buttonStyle.active.textColor = Color.white;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle.fontSize = 16;
        buttonStyle.fontStyle = FontStyle.Bold;
        
        // Tạo style cho menu
        menuStyle = new GUIStyle();
        menuStyle.normal.background = CreateColorTexture(new Color(0, 0, 0, 0.9f));
        
        // Tạo style cho tiêu đề
        titleStyle = new GUIStyle();
        titleStyle.normal.textColor = new Color(1f, 0.8f, 0f); // Màu vàng
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 24;
        titleStyle.fontStyle = FontStyle.Bold;
        
        // Tạo style cho nút menu
        menuButtonStyle = new GUIStyle();
        menuButtonStyle.normal.background = CreateColorTexture(new Color(0.2f, 0.2f, 0.3f, 0.8f));
        menuButtonStyle.hover.background = CreateColorTexture(new Color(0.3f, 0.3f, 0.4f, 0.8f));
        menuButtonStyle.active.background = CreateColorTexture(new Color(0.1f, 0.1f, 0.2f, 0.8f));
        menuButtonStyle.normal.textColor = Color.white;
        menuButtonStyle.hover.textColor = Color.white;
        menuButtonStyle.active.textColor = Color.white;
        menuButtonStyle.alignment = TextAnchor.MiddleCenter;
        menuButtonStyle.fontSize = 18;
        
        Debug.Log("EmergencyMenuButton đã khởi tạo ở " + (showInTopRight ? "góc trên bên phải" : "góc trên bên trái"));
    }
    
    void Update()
    {
        // Mở/đóng menu khi nhấn phím M
        if (Input.GetKeyDown(menuKey))
        {
            showMenu = !showMenu;
            Time.timeScale = showMenu ? 0f : 1f;
            Debug.Log("Menu đã được " + (showMenu ? "mở" : "đóng") + " bằng phím " + menuKey);
        }
        
        // Cập nhật vị trí nút khi kích thước màn hình thay đổi
        if (showInTopRight)
        {
            buttonRect = new Rect(Screen.width - buttonWidth - margin, margin, buttonWidth, buttonHeight);
        }
    }
    
    void OnGUI()
    {
        // Vẽ nút menu
        if (GUI.Button(buttonRect, "MENU", buttonStyle))
        {
            showMenu = !showMenu;
            
            // Tạm dừng/tiếp tục game
            Time.timeScale = showMenu ? 0f : 1f;
            
            Debug.Log("Nút menu được nhấn! showMenu = " + showMenu);
        }
        
        // Hiển thị menu nếu được yêu cầu
        if (showMenu)
        {
            // Vẽ nền menu
            GUI.Box(menuRect, "", menuStyle);
            
            // Vẽ tiêu đề
            GUI.Label(new Rect(menuRect.x, menuRect.y + 20, menuRect.width, 40), "GRAPPLER SHOOTING", titleStyle);
            
            // Danh sách các mục menu
            string[] menuItems = new string[] {
                "CONTINUE",
                "NEW GAME",
                "LOAD GAME",
                "SETTINGS",
                "CREDITS",
                "EXIT GAME"
            };
            
            // Vẽ các nút menu
            float buttonHeight = 50;
            float spacing = 10;
            float startY = 80;
            
            for (int i = 0; i < menuItems.Length; i++)
            {
                Rect itemRect = new Rect(
                    menuRect.x + 25,
                    menuRect.y + startY + i * (buttonHeight + spacing),
                    menuRect.width - 50,
                    buttonHeight
                );
                
                // Kiểm tra xem chuột có đang hover trên nút không
                if (itemRect.Contains(Event.current.mousePosition))
                {
                    hoveredButton = i;
                }
                
                // Tạo style tùy chỉnh cho từng nút
                GUIStyle currentStyle = new GUIStyle(menuButtonStyle);
                if (i == hoveredButton)
                {
                    // Thay đổi màu chữ khi hover
                    currentStyle.normal.textColor = new Color(0.9f, 0.2f, 0.2f); // Màu đỏ khi hover
                }
                
                if (GUI.Button(itemRect, menuItems[i], currentStyle))
                {
                    OnMenuItemClick(i);
                }
            }
            
            // Reset hover state khi chuột không trên menu
            if (!menuRect.Contains(Event.current.mousePosition))
            {
                hoveredButton = -1;
            }
        }
    }
    
    void OnMenuItemClick(int index)
    {
        Debug.Log("Menu item clicked: " + index);
        
        switch (index)
        {
            case 0: // CONTINUE
                showMenu = false;
                Time.timeScale = 1f;
                break;
                
            case 1: // NEW GAME
                showMenu = false;
                Time.timeScale = 1f;
                // Thêm code để bắt đầu game mới
                break;
                
            case 5: // EXIT GAME
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif
                break;
                
            default:
                // Các nút khác chưa có chức năng
                break;
        }
    }
    
    // Hàm tạo texture màu đơn giản
    private Texture2D CreateColorTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }
}
