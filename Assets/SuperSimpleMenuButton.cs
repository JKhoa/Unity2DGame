using UnityEngine;
using UnityEngine.SceneManagement;

public class SuperSimpleMenuButton : MonoBehaviour
{
    // Không cần khai báo biến phức tạp
    private bool showMenu = false;
    private bool isGamePaused = false;
    
    // Lưu giá trị timeScale ban đầu để khôi phục khi cần
    private float originalTimeScale;
    
    // Các texture cho menu
    private Texture2D logoTexture;
    private Texture2D backgroundTexture;
    private Texture2D buttonNormalTexture;
    private Texture2D buttonHoverTexture;
    private Texture2D redLineTexture;
    private Texture2D menuBackgroundTexture; // Texture nền menu gradient tím-hồng
    
    // Các style cho menu
    private GUIStyle logoStyle;
    private GUIStyle menuTitleStyle;
    private GUIStyle buttonStyle;
    private GUIStyle buttonHoverStyle;
    private GUIStyle menuButtonStyle;
    
    // Vị trí và kích thước menu
    private Rect menuRect;
    private float buttonHeight = 40f;
    private float buttonSpacing = 10f;
    
    // Chỉ số nút đang được hover
    private int hoveredButtonIndex = -1;
    
    // Danh sách các mục menu
    private string[] menuItems = new string[] {
        "CONTINUE",        // KONTYNUUJ
        "NEW GAME",        // NOWA GRA
        "LOAD GAME",       // WCZYTAJ STAN GRY
        "SETTINGS",        // USTAWIENIA
        "CREDITS",         // TWÓRCY GRY
        "EXIT GAME"        // WYJDŹ Z GRY
    };
    
    // Hình ảnh nền
    private Texture2D backgroundImage;

    void Start()
    {
        // Lưu giá trị timeScale ban đầu
        originalTimeScale = Time.timeScale;
        Debug.Log("SuperSimpleMenuButton đã khởi tạo với timeScale ban đầu = " + originalTimeScale);
        
        // Tạo các texture
        CreateTextures();
        
        // Tạo các style
        CreateStyles();
        
        // Thiết lập kích thước menu - chuyển qua bên trái màn hình
        float menuWidth = 300;
        float menuHeight = 500;
        menuRect = new Rect(Screen.width * 0.15f, Screen.height / 2 - menuHeight / 2, menuWidth, menuHeight);
        
        // Tạo logo GrapplerShooting
        logoTexture = CreateGrapplerShootingLogo();
        
        // Tạo hình nền gradient tím-hồng
        menuBackgroundTexture = CyberpunkBackground.CreateBackgroundTexture(Screen.width, Screen.height);
    }

    void CreateTextures()
    {
        // Tạo texture nền menu - làm mờ hơn
        backgroundTexture = new Texture2D(1, 1);
        backgroundTexture.SetPixel(0, 0, new Color(0.05f, 0.05f, 0.1f, 0.6f)); // Giảm alpha xuống 0.6
        backgroundTexture.Apply();
        
        // Tạo texture nút bình thường
        buttonNormalTexture = new Texture2D(1, 1);
        buttonNormalTexture.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.15f, 0.4f)); // Giảm alpha xuống 0.4
        buttonNormalTexture.Apply();
        
        // Tạo texture nút hover
        buttonHoverTexture = new Texture2D(1, 1);
        buttonHoverTexture.SetPixel(0, 0, new Color(0.2f, 0.1f, 0.1f, 0.6f)); // Giảm alpha xuống 0.6
        buttonHoverTexture.Apply();
        
        // Tạo texture đường kẻ đỏ
        redLineTexture = new Texture2D(1, 1);
        redLineTexture.SetPixel(0, 0, new Color(0.9f, 0.2f, 0.2f, 1f));
        redLineTexture.Apply();
        
        // Tạo texture nền
        backgroundImage = new Texture2D(1, 1);
        backgroundImage.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.2f, 0.5f));
        backgroundImage.Apply();
    }
    
    void CreateStyles()
    {
        // Style cho logo
        logoStyle = new GUIStyle();
        logoStyle.normal.background = null;
        logoStyle.normal.textColor = new Color(1f, 0.8f, 0f); // Màu vàng Cyberpunk
        logoStyle.fontSize = 28; // Giảm kích thước font
        logoStyle.fontStyle = FontStyle.Bold;
        logoStyle.alignment = TextAnchor.MiddleCenter; // Căn giữa
        
        // Style cho tiêu đề menu
        menuTitleStyle = new GUIStyle();
        menuTitleStyle.normal.textColor = Color.white;
        menuTitleStyle.fontSize = 24;
        menuTitleStyle.alignment = TextAnchor.MiddleCenter;
        
        // Style cho nút menu bình thường
        buttonStyle = new GUIStyle();
        buttonStyle.normal.background = buttonNormalTexture;
        buttonStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
        buttonStyle.fontSize = 18;
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.alignment = TextAnchor.MiddleLeft;
        buttonStyle.padding = new RectOffset(20, 0, 0, 0);
        
        // Style cho nút menu hover
        buttonHoverStyle = new GUIStyle(buttonStyle);
        buttonHoverStyle.normal.background = buttonHoverTexture;
        buttonHoverStyle.normal.textColor = new Color(0.9f, 0.2f, 0.2f); // Màu đỏ Cyberpunk
        
        // Style cho nút MENU
        menuButtonStyle = new GUIStyle();
        menuButtonStyle.normal.background = buttonNormalTexture;
        menuButtonStyle.normal.textColor = Color.white;
        menuButtonStyle.fontSize = 16;
        menuButtonStyle.fontStyle = FontStyle.Bold;
        menuButtonStyle.alignment = TextAnchor.MiddleCenter;
    }
    
    // Tạo logo GrapplerShooting
    private Texture2D CreateGrapplerShootingLogo()
    {
        Texture2D logoTexture = new Texture2D(300, 60, TextureFormat.RGBA32, false);
        
        // Tạo nền trong suốt
        Color transparent = new Color(0, 0, 0, 0);
        for (int x = 0; x < logoTexture.width; x++)
        {
            for (int y = 0; y < logoTexture.height; y++)
            {
                logoTexture.SetPixel(x, y, transparent);
            }
        }
        
        // Vẽ chữ "GRAPPLERSHOOTING" bằng các hình chữ nhật màu vàng
        Color cyberpunkYellow = new Color(1f, 0.8f, 0f);
        
        // Vẽ các chữ cái bằng các hình chữ nhật đơn giản
        // G
        DrawRect(logoTexture, 10, 25, 10, 50, cyberpunkYellow);
        DrawRect(logoTexture, 10, 40, 10, 15, cyberpunkYellow);
        DrawRect(logoTexture, 10, 40, 45, 50, cyberpunkYellow);
        DrawRect(logoTexture, 30, 40, 30, 50, cyberpunkYellow);
        
        // R
        DrawRect(logoTexture, 45, 55, 10, 50, cyberpunkYellow);
        DrawRect(logoTexture, 55, 70, 10, 15, cyberpunkYellow);
        DrawRect(logoTexture, 55, 70, 25, 30, cyberpunkYellow);
        DrawRect(logoTexture, 55, 70, 30, 50, cyberpunkYellow);
        
        // A
        DrawRect(logoTexture, 75, 85, 10, 50, cyberpunkYellow);
        DrawRect(logoTexture, 95, 105, 10, 50, cyberpunkYellow);
        DrawRect(logoTexture, 85, 95, 10, 15, cyberpunkYellow);
        DrawRect(logoTexture, 85, 95, 30, 35, cyberpunkYellow);
        
        // P
        DrawRect(logoTexture, 110, 120, 10, 50, cyberpunkYellow);
        DrawRect(logoTexture, 120, 135, 10, 15, cyberpunkYellow);
        DrawRect(logoTexture, 120, 135, 25, 30, cyberpunkYellow);
        
        // P
        DrawRect(logoTexture, 140, 150, 10, 50, cyberpunkYellow);
        DrawRect(logoTexture, 150, 165, 10, 15, cyberpunkYellow);
        DrawRect(logoTexture, 150, 165, 25, 30, cyberpunkYellow);
        
        // L
        DrawRect(logoTexture, 170, 180, 10, 50, cyberpunkYellow);
        DrawRect(logoTexture, 180, 195, 45, 50, cyberpunkYellow);
        
        // E
        DrawRect(logoTexture, 200, 210, 10, 50, cyberpunkYellow);
        DrawRect(logoTexture, 210, 225, 10, 15, cyberpunkYellow);
        DrawRect(logoTexture, 210, 225, 30, 35, cyberpunkYellow);
        DrawRect(logoTexture, 210, 225, 45, 50, cyberpunkYellow);
        
        // R
        DrawRect(logoTexture, 230, 240, 10, 50, cyberpunkYellow);
        DrawRect(logoTexture, 240, 255, 10, 15, cyberpunkYellow);
        DrawRect(logoTexture, 240, 255, 25, 30, cyberpunkYellow);
        DrawRect(logoTexture, 240, 255, 30, 50, cyberpunkYellow);
        
        logoTexture.Apply();
        return logoTexture;
    }
    
    // Vẽ hình chữ nhật
    private void DrawRect(Texture2D texture, int x1, int x2, int y1, int y2, Color color)
    {
        for (int x = x1; x < x2 && x < texture.width; x++)
        {
            for (int y = y1; y < y2 && y < texture.height; y++)
            {
                texture.SetPixel(x, y, color);
            }
        }
    }

    void OnGUI()
    {
        // Vẽ nút MENU ở góc trên bên phải
        if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 40), "MENU", menuButtonStyle))
        {
            ToggleMenu();
        }
        
        // Nếu menu đang được hiển thị
        if (showMenu)
        {
            // Vẽ hình nền gradient tím-hồng cho toàn màn hình
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), menuBackgroundTexture);
            
            // Vẽ nền menu với độ trong suốt
            GUI.DrawTexture(menuRect, backgroundTexture);
            
            // Vẽ logo GrapplerShooting ở trên cùng
            GUI.Label(new Rect(menuRect.x, menuRect.y + 20, menuRect.width, 40), "GRAPPLERSHOOTING", logoStyle);
            
            // Vẽ các nút menu
            float startY = menuRect.y + 80; // Giảm khoảng cách từ logo xuống các nút
            
            for (int i = 0; i < menuItems.Length; i++)
            {
                Rect buttonRect = new Rect(
                    menuRect.x + 10, // Giảm lề trái để các nút gần mép trái hơn
                    startY + i * (buttonHeight + buttonSpacing),
                    menuRect.width - 20,
                    buttonHeight
                );
                
                // Kiểm tra xem chuột có đang hover trên nút không
                bool isHovered = buttonRect.Contains(Event.current.mousePosition);
                if (isHovered)
                {
                    hoveredButtonIndex = i;
                }
                
                // Chọn style dựa trên trạng thái hover
                GUIStyle currentStyle = (i == hoveredButtonIndex) ? buttonHoverStyle : buttonStyle;
                
                // Vẽ nút menu
                if (GUI.Button(buttonRect, menuItems[i], currentStyle))
                {
                    OnMenuItemClick(i);
                }
                
                // Vẽ đường kẻ đỏ bên dưới nút được hover
                if (i == hoveredButtonIndex)
                {
                    GUI.DrawTexture(new Rect(buttonRect.x, buttonRect.y + buttonRect.height + 2, buttonRect.width, 2), redLineTexture);
                }
            }
            
            // Reset hover state khi chuột không trên menu
            if (!menuRect.Contains(Event.current.mousePosition))
            {
                hoveredButtonIndex = -1;
            }
        }
    }
    
    // Phím tắt M để mở menu
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMenu();
        }
    }
    
    // Bật/tắt menu và tạm dừng/tiếp tục game
    void ToggleMenu()
    {
        showMenu = !showMenu;
        
        if (showMenu)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
        
        Debug.Log("Menu " + (showMenu ? "mở" : "đóng") + ", Game " + (isGamePaused ? "tạm dừng" : "tiếp tục"));
    }
    
    // Tạm dừng game
    void PauseGame()
    {
        if (!isGamePaused)
        {
            originalTimeScale = Time.timeScale; // Lưu timeScale hiện tại
            Time.timeScale = 0f; // Đặt timeScale = 0 để tạm dừng game
            isGamePaused = true;
            Debug.Log("Game đã tạm dừng, timeScale = " + Time.timeScale);
        }
    }
    
    // Tiếp tục game
    void ResumeGame()
    {
        if (isGamePaused)
        {
            Time.timeScale = originalTimeScale; // Khôi phục timeScale ban đầu
            isGamePaused = false;
            Debug.Log("Game đã tiếp tục, timeScale = " + Time.timeScale);
        }
    }
    
    // Tiếp tục game khi nhấn Continue
    void ContinueGame()
    {
        showMenu = false;
        ResumeGame();
        Debug.Log("Nhấn CONTINUE, game tiếp tục");
    }
    
    // Bắt đầu game mới
    void NewGame()
    {
        // Đóng menu trước khi bắt đầu game mới
        showMenu = false;
        
        // Lấy tên scene hiện tại và tải lại
        string currentScene = SceneManager.GetActiveScene().name;
        Time.timeScale = originalTimeScale; // Đảm bảo timeScale bình thường trước khi reset
        SceneManager.LoadScene(currentScene);
        Debug.Log("Nhấn NEW GAME, đóng menu và tải lại scene: " + currentScene);
    }
    
    // Thoát game
    void ExitGame()
    {
        Debug.Log("Nhấn EXIT");
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    // Xử lý khi nhấn vào mục menu
    void OnMenuItemClick(int index)
    {
        switch (index)
        {
            case 0:
                ContinueGame();
                break;
            case 1:
                NewGame();
                break;
            case 2:
                // Load game
                break;
            case 3:
                // Settings
                break;
            case 4:
                // Credits
                break;
            case 5:
                ExitGame();
                break;
        }
    }
}
