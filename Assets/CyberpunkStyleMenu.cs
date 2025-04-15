using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CyberpunkStyleMenu : MonoBehaviour
{
    [Header("Menu Settings")]
    public KeyCode menuKey = KeyCode.M;
    public bool showMenuOnStart = true;
    
    [Header("Menu References")]
    public GameObject menuPanel;
    public Transform menuItemsContainer;
    
    [Header("Menu Styling")]
    public Color selectedColor = new Color(0.9f, 0.2f, 0.2f, 1f); // Màu đỏ khi được chọn
    public Color normalColor = new Color(0.7f, 0.7f, 0.7f, 1f);   // Màu xám bình thường
    
    // Các tùy chọn menu bằng tiếng Anh
    private string[] menuItems = new string[] {
        "CONTINUE",        // KONTYNUUJ
        "NEW GAME",        // NOWA GRA
        "LOAD GAME",       // WCZYTAJ STAN GRY
        "SETTINGS",        // USTAWIENIA
        "CREDITS",         // TWÓRCY GRY
        "EXIT GAME"        // WYJDŹ Z GRY
    };
    
    private bool menuCreated = false;
    private Canvas menuCanvas;
    private GameObject logoObject;
    
    void Awake()
    {
        // Tạo menu ngay khi component được khởi tạo
        if (menuPanel == null)
        {
            CreateCyberpunkMenu();
        }
    }
    
    void Start()
    {
        // Hiển thị hoặc ẩn menu tùy theo cài đặt
        if (menuPanel != null)
        {
            menuPanel.SetActive(showMenuOnStart);
            if (showMenuOnStart)
            {
                Time.timeScale = 0f;
            }
        }
        
        // Debug log để kiểm tra
        Debug.Log("CyberpunkStyleMenu started. Menu panel: " + (menuPanel != null ? "exists" : "null"));
    }
    
    void OnGUI()
    {
        // Hiển thị nút menu nhỏ ở góc trên bên phải nếu menu đang ẩn
        if (menuPanel == null || !menuPanel.activeSelf)
        {
            if (GUI.Button(new Rect(Screen.width - 100, 10, 90, 30), "MENU (M)"))
            {
                ToggleMenu();
            }
        }
    }
    
    void Update()
    {
        // Mở/đóng menu khi nhấn phím M
        if (Input.GetKeyDown(menuKey))
        {
            ToggleMenu();
        }
    }
    
    public void ToggleMenu()
    {
        if (menuPanel != null)
        {
            bool isActive = !menuPanel.activeSelf;
            menuPanel.SetActive(isActive);
            
            // Tạm dừng/tiếp tục game
            Time.timeScale = isActive ? 0f : 1f;
            
            Debug.Log("Menu toggled: " + (isActive ? "shown" : "hidden"));
        }
        else if (!menuCreated)
        {
            CreateCyberpunkMenu();
            menuPanel.SetActive(true);
            Time.timeScale = 0f;
            
            Debug.Log("Menu created and shown");
        }
    }
    
    void CreateCyberpunkMenu()
    {
        Debug.Log("Đang tạo menu Cyberpunk style...");
        
        // Tìm canvas hiện có hoặc tạo mới
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        if (canvases.Length > 0)
        {
            foreach (Canvas c in canvases)
            {
                if (c.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    menuCanvas = c;
                    Debug.Log("Found existing canvas: " + c.name);
                    break;
                }
            }
        }
        
        if (menuCanvas == null)
        {
            // Tạo canvas mới
            GameObject canvasObj = new GameObject("CyberpunkMenuCanvas");
            menuCanvas = canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            menuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            menuCanvas.sortingOrder = 100;
            
            Debug.Log("Created new canvas: " + canvasObj.name);
            
            // Thêm EventSystem nếu chưa có
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                Debug.Log("Created new EventSystem");
            }
        }
        
        // Tạo panel nền cho menu
        GameObject panel = new GameObject("CyberpunkMenuPanel");
        panel.transform.SetParent(menuCanvas.transform, false);
        menuPanel = panel;
        
        // Thêm hình ảnh nền
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f); // Nền đen mờ
        
        // Thiết lập kích thước panel
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Tạo logo Cyberpunk
        CreateLogo();
        
        // Tạo container cho các mục menu
        GameObject container = new GameObject("MenuItems");
        container.transform.SetParent(panel.transform, false);
        menuItemsContainer = container.transform;
        
        // Thiết lập vị trí container
        RectTransform containerRect = container.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.05f, 0.2f);
        containerRect.anchorMax = new Vector2(0.4f, 0.8f);
        containerRect.offsetMin = Vector2.zero;
        containerRect.offsetMax = Vector2.zero;
        
        // Tạo các mục menu
        for (int i = 0; i < menuItems.Length; i++)
        {
            CreateMenuItem(menuItems[i], i);
        }
        
        // Đánh dấu menu đã được tạo
        menuCreated = true;
        
        Debug.Log("Đã tạo menu Cyberpunk style thành công!");
    }
    
    void CreateLogo()
    {
        // Tạo đối tượng logo
        logoObject = new GameObject("CyberpunkLogo");
        logoObject.transform.SetParent(menuPanel.transform, false);
        
        // Thiết lập vị trí logo
        RectTransform logoRect = logoObject.AddComponent<RectTransform>();
        logoRect.anchorMin = new Vector2(0.05f, 0.85f);
        logoRect.anchorMax = new Vector2(0.4f, 0.95f);
        logoRect.offsetMin = Vector2.zero;
        logoRect.offsetMax = Vector2.zero;
        
        // Thêm text component cho logo
        Text logoText = logoObject.AddComponent<Text>();
        logoText.text = "GRAPPLER SHOOTING";
        logoText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        logoText.fontSize = 36;
        logoText.fontStyle = FontStyle.Bold;
        logoText.color = new Color(1f, 0.8f, 0f); // Màu vàng
        logoText.alignment = TextAnchor.MiddleLeft;
    }
    
    void CreateMenuItem(string itemText, int index)
    {
        // Tạo đối tượng cho mục menu
        GameObject item = new GameObject(itemText + "Button");
        item.transform.SetParent(menuItemsContainer, false);
        
        // Thiết lập vị trí
        RectTransform itemRect = item.AddComponent<RectTransform>();
        float height = 1f / (menuItems.Length + 1);
        float yPos = 1f - (index + 1) * height;
        itemRect.anchorMin = new Vector2(0, yPos - height * 0.3f);
        itemRect.anchorMax = new Vector2(1, yPos + height * 0.3f);
        itemRect.offsetMin = Vector2.zero;
        itemRect.offsetMax = Vector2.zero;
        
        // Thêm text component
        Text itemText2 = item.AddComponent<Text>();
        itemText2.text = itemText;
        itemText2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        itemText2.fontSize = 24;
        itemText2.alignment = TextAnchor.MiddleLeft;
        itemText2.color = normalColor;
        
        // Thêm button component
        Button button = item.AddComponent<Button>();
        
        // Thiết lập màu sắc cho button
        ColorBlock colors = button.colors;
        colors.normalColor = Color.clear;
        colors.highlightedColor = Color.clear;
        colors.pressedColor = Color.clear;
        colors.selectedColor = Color.clear;
        button.colors = colors;
        
        // Thêm sự kiện khi hover bằng cách sử dụng các phương thức có sẵn
        button.onClick.AddListener(() => OnMenuItemClick(index));
        
        // Thêm component để xử lý hover
        ButtonHoverHandler hoverHandler = item.AddComponent<ButtonHoverHandler>();
        hoverHandler.Initialize(itemText2, normalColor, selectedColor);
    }
    
    void OnMenuItemClick(int index)
    {
        Debug.Log("Menu item clicked: " + menuItems[index]);
        
        switch (index)
        {
            case 0: // CONTINUE
                menuPanel.SetActive(false);
                Time.timeScale = 1f;
                break;
                
            case 1: // NEW GAME
                menuPanel.SetActive(false);
                Time.timeScale = 1f;
                // Thêm code để bắt đầu game mới
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
                
            case 2: // LOAD GAME
                // Thêm code để load game
                break;
                
            case 3: // SETTINGS
                // Thêm code để mở settings
                break;
                
            case 4: // CREDITS
                // Thêm code để hiển thị credits
                break;
                
            case 5: // EXIT GAME
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif
                break;
        }
    }
}

// Class phụ trợ để xử lý hover
public class ButtonHoverHandler : MonoBehaviour
{
    private Text buttonText;
    private Color normalColor;
    private Color hoverColor;
    
    public void Initialize(Text text, Color normal, Color hover)
    {
        buttonText = text;
        normalColor = normal;
        hoverColor = hover;
    }
    
    public void OnPointerEnter()
    {
        if (buttonText != null)
        {
            buttonText.color = hoverColor;
        }
    }
    
    public void OnPointerExit()
    {
        if (buttonText != null)
        {
            buttonText.color = normalColor;
        }
    }
    
    void OnMouseEnter()
    {
        OnPointerEnter();
    }
    
    void OnMouseExit()
    {
        OnPointerExit();
    }
}
