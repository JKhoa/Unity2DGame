using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DirectMenuButton : MonoBehaviour
{
    // Vị trí và kích thước nút
    public Vector2 buttonPosition = new Vector2(10, 10);
    public Vector2 buttonSize = new Vector2(200, 60);
    
    // Màu sắc và text
    public Color buttonColor = Color.red;
    public string buttonText = "MENU";
    public int fontSize = 24;
    
    // Tham chiếu đến các component
    private Canvas canvas;
    private GameObject menuPanel;
    private bool menuCreated = false;
    
    void Start()
    {
        // Đợi 1 frame để đảm bảo scene đã được tải hoàn toàn
        StartCoroutine(CreateButtonDelayed());
    }
    
    IEnumerator CreateButtonDelayed()
    {
        yield return null; // Đợi 1 frame
        
        Debug.Log("Bắt đầu tạo nút menu...");
        
        // Tạo nút menu
        CreateMenuButton();
    }
    
    void CreateMenuButton()
    {
        // Tìm canvas hiện có trong scene
        canvas = FindObjectOfType<Canvas>();
        
        // Nếu không tìm thấy canvas, tạo mới
        if (canvas == null)
        {
            Debug.Log("Không tìm thấy Canvas, đang tạo mới...");
            GameObject canvasObj = new GameObject("MenuButtonCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            
            // Thêm EventSystem nếu chưa có
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }
        else
        {
            Debug.Log("Đã tìm thấy Canvas: " + canvas.name);
        }
        
        // Tạo nút menu
        GameObject buttonObj = new GameObject("MenuButton");
        buttonObj.transform.SetParent(canvas.transform, false);
        
        // Thêm RectTransform
        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.sizeDelta = buttonSize;
        rectTransform.anchoredPosition = buttonPosition;
        
        // Thêm Image component
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = buttonColor;
        
        // Tạo text cho nút
        GameObject textObj = new GameObject("ButtonText");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        // Thêm RectTransform cho text
        RectTransform textRectTransform = textObj.AddComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.offsetMin = Vector2.zero;
        textRectTransform.offsetMax = Vector2.zero;
        
        // Thêm Text component
        Text buttonTextComponent = textObj.AddComponent<Text>();
        buttonTextComponent.text = buttonText;
        buttonTextComponent.alignment = TextAnchor.MiddleCenter;
        buttonTextComponent.fontSize = fontSize;
        buttonTextComponent.color = Color.white;
        
        // Tìm font mặc định
        buttonTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        
        // Thêm Button component
        Button button = buttonObj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = buttonColor;
        colors.highlightedColor = new Color(buttonColor.r * 1.2f, buttonColor.g * 1.2f, buttonColor.b * 1.2f, buttonColor.a);
        colors.pressedColor = new Color(buttonColor.r * 0.8f, buttonColor.g * 0.8f, buttonColor.b * 0.8f, buttonColor.a);
        button.colors = colors;
        
        // Thêm sự kiện click
        button.onClick.AddListener(OnMenuButtonClick);
        
        Debug.Log("Đã tạo nút menu thành công!");
    }
    
    void OnMenuButtonClick()
    {
        Debug.Log("Nút menu được nhấn!");
        
        // Nếu menu chưa được tạo, tạo mới
        if (!menuCreated)
        {
            CreateMenu();
            menuCreated = true;
        }
        // Nếu menu đã được tạo, hiển thị/ẩn
        else
        {
            if (menuPanel != null)
            {
                menuPanel.SetActive(!menuPanel.activeSelf);
                
                // Tạm dừng/tiếp tục game
                Time.timeScale = menuPanel.activeSelf ? 0f : 1f;
            }
            else
            {
                Debug.LogWarning("Menu panel không tồn tại!");
                CreateMenu();
            }
        }
    }
    
    void CreateMenu()
    {
        Debug.Log("Đang tạo menu...");
        
        // Tạo panel cho menu
        menuPanel = new GameObject("MenuPanel");
        menuPanel.transform.SetParent(canvas.transform, false);
        
        // Thêm RectTransform
        RectTransform panelRectTransform = menuPanel.AddComponent<RectTransform>();
        panelRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        panelRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
        panelRectTransform.sizeDelta = new Vector2(400, 500);
        panelRectTransform.anchoredPosition = Vector2.zero;
        
        // Thêm Image component
        Image panelImage = menuPanel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.9f);
        
        // Tạo tiêu đề
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(menuPanel.transform, false);
        
        // Thêm RectTransform cho tiêu đề
        RectTransform titleRectTransform = titleObj.AddComponent<RectTransform>();
        titleRectTransform.anchorMin = new Vector2(0.5f, 1);
        titleRectTransform.anchorMax = new Vector2(0.5f, 1);
        titleRectTransform.pivot = new Vector2(0.5f, 1);
        titleRectTransform.sizeDelta = new Vector2(380, 60);
        titleRectTransform.anchoredPosition = new Vector2(0, -20);
        
        // Thêm Text component cho tiêu đề
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "GRAPPLER SHOOTING";
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.fontSize = 30;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(1f, 0.8f, 0f); // Màu vàng
        
        // Tạo các nút menu
        string[] menuItems = new string[] {
            "CONTINUE",
            "NEW GAME",
            "LOAD GAME",
            "SETTINGS",
            "CREDITS",
            "EXIT GAME"
        };
        
        // Khoảng cách giữa các nút
        float buttonHeight = 60;
        float spacing = 10;
        float startY = -100;
        
        // Tạo các nút
        for (int i = 0; i < menuItems.Length; i++)
        {
            // Tạo nút
            GameObject menuButtonObj = new GameObject(menuItems[i] + "Button");
            menuButtonObj.transform.SetParent(menuPanel.transform, false);
            
            // Thêm RectTransform
            RectTransform menuButtonRect = menuButtonObj.AddComponent<RectTransform>();
            menuButtonRect.anchorMin = new Vector2(0.5f, 1);
            menuButtonRect.anchorMax = new Vector2(0.5f, 1);
            menuButtonRect.pivot = new Vector2(0.5f, 1);
            menuButtonRect.sizeDelta = new Vector2(300, buttonHeight);
            menuButtonRect.anchoredPosition = new Vector2(0, startY - i * (buttonHeight + spacing));
            
            // Thêm Image component
            Image menuButtonImage = menuButtonObj.AddComponent<Image>();
            menuButtonImage.color = new Color(0.2f, 0.2f, 0.3f, 0.8f);
            
            // Tạo text cho nút
            GameObject menuButtonTextObj = new GameObject("Text");
            menuButtonTextObj.transform.SetParent(menuButtonObj.transform, false);
            
            // Thêm RectTransform cho text
            RectTransform menuButtonTextRect = menuButtonTextObj.AddComponent<RectTransform>();
            menuButtonTextRect.anchorMin = Vector2.zero;
            menuButtonTextRect.anchorMax = Vector2.one;
            menuButtonTextRect.offsetMin = Vector2.zero;
            menuButtonTextRect.offsetMax = Vector2.zero;
            
            // Thêm Text component
            Text menuButtonText = menuButtonTextObj.AddComponent<Text>();
            menuButtonText.text = menuItems[i];
            menuButtonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            menuButtonText.fontSize = 24;
            menuButtonText.alignment = TextAnchor.MiddleCenter;
            menuButtonText.color = Color.white;
            
            // Thêm Button component
            Button menuButton = menuButtonObj.AddComponent<Button>();
            ColorBlock menuButtonColors = menuButton.colors;
            menuButtonColors.normalColor = new Color(0.2f, 0.2f, 0.3f, 0.8f);
            menuButtonColors.highlightedColor = new Color(0.3f, 0.3f, 0.4f, 0.8f);
            menuButtonColors.pressedColor = new Color(0.1f, 0.1f, 0.2f, 0.8f);
            menuButton.colors = menuButtonColors;
            
            // Thêm sự kiện click
            int buttonIndex = i;
            menuButton.onClick.AddListener(() => OnMenuItemClick(buttonIndex));
        }
        
        // Tạm dừng game
        Time.timeScale = 0f;
        
        Debug.Log("Đã tạo menu thành công!");
    }
    
    void OnMenuItemClick(int index)
    {
        Debug.Log("Nút menu " + index + " được nhấn!");
        
        switch (index)
        {
            case 0: // CONTINUE
                menuPanel.SetActive(false);
                Time.timeScale = 1f; // Tiếp tục game
                break;
                
            case 1: // NEW GAME
                // Thêm code để bắt đầu game mới
                menuPanel.SetActive(false);
                Time.timeScale = 1f;
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
