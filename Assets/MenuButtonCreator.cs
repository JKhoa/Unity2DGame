using UnityEngine;
using UnityEngine.UI;

public class MenuButtonCreator : MonoBehaviour
{
    // Tự động tạo nút khi script được thêm vào scene
    void Awake()
    {
        CreateMenuButton();
    }

    // Tạo nút menu
    void CreateMenuButton()
    {
        Debug.Log("Đang tạo nút menu...");

        // Tạo canvas mới để đảm bảo nút luôn hiển thị
        GameObject canvasObj = new GameObject("MenuButtonCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Thiết lập canvas
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100; // Đảm bảo hiển thị trên cùng
        
        // Tạo nút
        GameObject buttonObj = new GameObject("OpenMenuButton");
        buttonObj.transform.SetParent(canvas.transform, false);
        
        // Thêm hình ảnh cho nút
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.8f, 0.2f, 0.2f, 1f); // Màu đỏ
        
        // Thiết lập vị trí và kích thước nút
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0);
        buttonRect.anchorMax = new Vector2(0.5f, 0);
        buttonRect.pivot = new Vector2(0.5f, 0);
        buttonRect.sizeDelta = new Vector2(200, 60);
        buttonRect.anchoredPosition = new Vector2(0, 100);
        
        // Thêm text cho nút
        GameObject textObj = new GameObject("ButtonText");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        Text buttonText = textObj.AddComponent<Text>();
        buttonText.text = "OPEN MENU";
        buttonText.color = Color.white;
        buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonText.fontSize = 24;
        buttonText.alignment = TextAnchor.MiddleCenter;
        
        // Thiết lập vị trí text
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        // Thêm button component
        Button button = buttonObj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.8f, 0.2f, 0.2f, 1f);
        colors.highlightedColor = new Color(1f, 0.3f, 0.3f, 1f);
        colors.pressedColor = new Color(0.7f, 0.1f, 0.1f, 1f);
        button.colors = colors;
        
        // Thêm sự kiện click
        button.onClick.AddListener(OpenMenu);
        
        // Thêm EventSystem nếu chưa có
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
        
        Debug.Log("Đã tạo nút menu thành công!");
        
        // Đảm bảo nút không bị hủy khi chuyển scene
        DontDestroyOnLoad(canvasObj);
    }
    
    // Mở menu
    void OpenMenu()
    {
        Debug.Log("Nút menu được nhấn!");
        
        // Tạo menu nếu chưa có
        CreateCyberpunkMenu();
    }
    
    // Tạo menu Cyberpunk
    void CreateCyberpunkMenu()
    {
        Debug.Log("Đang tạo menu Cyberpunk...");
        
        // Kiểm tra xem menu đã tồn tại chưa
        GameObject existingMenu = GameObject.Find("CyberpunkMenu");
        if (existingMenu != null)
        {
            existingMenu.SetActive(true);
            Time.timeScale = 0f; // Tạm dừng game
            return;
        }
        
        // Tạo canvas cho menu
        GameObject canvasObj = new GameObject("CyberpunkMenuCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Thiết lập canvas
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 200; // Đảm bảo hiển thị trên cùng
        
        // Tạo panel nền
        GameObject menuObj = new GameObject("CyberpunkMenu");
        menuObj.transform.SetParent(canvas.transform, false);
        
        Image menuImage = menuObj.AddComponent<Image>();
        menuImage.color = new Color(0, 0, 0, 0.9f); // Màu đen trong suốt
        
        RectTransform menuRect = menuObj.GetComponent<RectTransform>();
        menuRect.anchorMin = Vector2.zero;
        menuRect.anchorMax = Vector2.one;
        menuRect.sizeDelta = Vector2.zero;
        
        // Tạo tiêu đề
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(menuObj.transform, false);
        
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "GRAPPLER SHOOTING";
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.fontSize = 36;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(1f, 0.8f, 0f); // Màu vàng
        
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.9f);
        titleRect.anchorMax = new Vector2(0.5f, 0.9f);
        titleRect.sizeDelta = new Vector2(500, 50);
        titleRect.anchoredPosition = Vector2.zero;
        
        // Tạo các nút menu
        string[] menuItems = new string[] {
            "CONTINUE",
            "NEW GAME",
            "LOAD GAME",
            "SETTINGS",
            "CREDITS",
            "EXIT GAME"
        };
        
        float buttonHeight = 50;
        float spacing = 20;
        float startY = 100;
        
        for (int i = 0; i < menuItems.Length; i++)
        {
            GameObject buttonObj = new GameObject(menuItems[i] + "Button");
            buttonObj.transform.SetParent(menuObj.transform, false);
            
            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.2f, 0.3f, 0.8f);
            
            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.sizeDelta = new Vector2(300, buttonHeight);
            buttonRect.anchoredPosition = new Vector2(0, startY - i * (buttonHeight + spacing));
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            
            Text buttonText = textObj.AddComponent<Text>();
            buttonText.text = menuItems[i];
            buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonText.fontSize = 24;
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.color = Color.white;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            Button button = buttonObj.AddComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = new Color(0.2f, 0.2f, 0.3f, 0.8f);
            colors.highlightedColor = new Color(0.3f, 0.3f, 0.4f, 0.8f);
            colors.pressedColor = new Color(0.1f, 0.1f, 0.2f, 0.8f);
            button.colors = colors;
            
            int index = i;
            button.onClick.AddListener(() => OnMenuButtonClick(index, menuObj));
        }
        
        // Tạm dừng game
        Time.timeScale = 0f;
        
        Debug.Log("Đã tạo menu Cyberpunk thành công!");
    }
    
    // Xử lý sự kiện click nút menu
    void OnMenuButtonClick(int index, GameObject menuObj)
    {
        Debug.Log("Menu button clicked: " + index);
        
        switch (index)
        {
            case 0: // CONTINUE
                menuObj.SetActive(false);
                Time.timeScale = 1f; // Tiếp tục game
                break;
                
            case 1: // NEW GAME
                menuObj.SetActive(false);
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
}
