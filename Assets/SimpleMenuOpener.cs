using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleMenuOpener : MonoBehaviour
{
    [Header("Menu Settings")]
    public KeyCode menuKey = KeyCode.M; // Sử dụng phím M thay vì Escape
    
    [Header("Menu References")]
    public Canvas menuCanvas;
    public GameObject menuPanel;
    
    // Các button trong menu
    public Button continueButton;
    public Button newGameButton;
    public Button loadGameButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button exitButton;
    
    private bool menuCreated = false;
    
    void Start()
    {
        // Tự động tạo menu nếu chưa có
        if (menuCanvas == null)
        {
            CreateMenuSystem();
        }
        
        // Ẩn menu khi bắt đầu
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }
    
    void Update()
    {
        // Kiểm tra phím để mở/đóng menu
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
        }
        else
        {
            Debug.LogError("Menu panel is not assigned!");
            
            // Thử tạo menu nếu chưa có
            if (!menuCreated)
            {
                CreateMenuSystem();
                if (menuPanel != null)
                {
                    menuPanel.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
        }
    }
    
    void CreateMenuSystem()
    {
        Debug.Log("Creating menu system from scratch");
        
        // Tạo canvas nếu chưa có
        GameObject canvasObj = new GameObject("MenuCanvas");
        menuCanvas = canvasObj.AddComponent<Canvas>();
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        menuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        menuCanvas.sortingOrder = 100;
        
        // Tạo panel nền
        GameObject panelObj = new GameObject("MenuPanel");
        panelObj.transform.SetParent(menuCanvas.transform, false);
        menuPanel = panelObj;
        
        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);
        
        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Tạo tiêu đề
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(panelObj.transform, false);
        
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "GRAPPLER SHOOTING";
        titleText.fontSize = 48;
        titleText.color = new Color(1f, 0.8f, 0f); // Màu vàng
        titleText.alignment = TextAlignmentOptions.Center;
        
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.85f);
        titleRect.anchorMax = new Vector2(0.5f, 0.95f);
        titleRect.sizeDelta = new Vector2(500, 100);
        titleRect.anchoredPosition = Vector2.zero;
        
        // Tạo các button menu
        string[] menuItems = new string[] {
            "CONTINUE",
            "NEW GAME",
            "LOAD GAME",
            "SETTINGS",
            "CREDITS",
            "EXIT GAME"
        };
        
        float buttonHeight = 60;
        float spacing = 20;
        float startY = 150;
        
        for (int i = 0; i < menuItems.Length; i++)
        {
            GameObject buttonObj = new GameObject(menuItems[i]);
            buttonObj.transform.SetParent(panelObj.transform, false);
            
            Button button = buttonObj.AddComponent<Button>();
            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            
            ColorBlock colors = button.colors;
            colors.normalColor = new Color(1, 1, 1, 0.8f);
            colors.highlightedColor = new Color(1, 0.8f, 0, 1);
            colors.pressedColor = new Color(0.9f, 0.5f, 0, 1);
            button.colors = colors;
            
            // Thiết lập vị trí button
            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.sizeDelta = new Vector2(300, buttonHeight);
            buttonRect.anchoredPosition = new Vector2(0, startY - i * (buttonHeight + spacing));
            
            // Thêm text cho button
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            
            TextMeshProUGUI buttonText = textObj.AddComponent<TextMeshProUGUI>();
            buttonText.text = menuItems[i];
            buttonText.fontSize = 24;
            buttonText.color = Color.white;
            buttonText.alignment = TextAlignmentOptions.Center;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            // Gán các button vào biến tương ứng
            switch (i)
            {
                case 0: continueButton = button; break;
                case 1: newGameButton = button; break;
                case 2: loadGameButton = button; break;
                case 3: settingsButton = button; break;
                case 4: creditsButton = button; break;
                case 5: exitButton = button; break;
            }
            
            // Thêm listener cho button
            int index = i;
            button.onClick.AddListener(() => OnButtonClick(index));
        }
        
        // Đánh dấu menu đã được tạo
        menuCreated = true;
        
        // Thêm EventSystem nếu chưa có
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }
    
    void OnButtonClick(int buttonIndex)
    {
        Debug.Log("Button clicked: " + buttonIndex);
        
        switch (buttonIndex)
        {
            case 0: // CONTINUE
                ToggleMenu();
                break;
                
            case 1: // NEW GAME
                ToggleMenu();
                // Thêm code để bắt đầu game mới
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
