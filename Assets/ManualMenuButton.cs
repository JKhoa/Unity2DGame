using UnityEngine;
using UnityEngine.UI;

public class ManualMenuButton : MonoBehaviour
{
    [Header("Menu Button Settings")]
    public Vector2 buttonPosition = new Vector2(50, 50); // Vị trí nút (góc trên bên trái)
    public Vector2 buttonSize = new Vector2(150, 50);    // Kích thước nút
    public string buttonText = "OPEN MENU";              // Chữ trên nút
    public Color buttonColor = new Color(0.2f, 0.2f, 0.2f, 0.8f); // Màu nút
    public Color textColor = Color.white;                // Màu chữ
    
    [Header("Menu Reference")]
    public GameObject menuToOpen;                        // Menu cần mở
    
    private SimpleMenuOpener menuOpener;                 // Tham chiếu đến SimpleMenuOpener nếu có
    
    private void Start()
    {
        // Tìm SimpleMenuOpener nếu có
        menuOpener = FindObjectOfType<SimpleMenuOpener>();
        
        // Tạo nút menu
        CreateMenuButton();
    }
    
    private void CreateMenuButton()
    {
        // Tạo canvas nếu chưa có
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("ButtonCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;
        }
        
        // Tạo nút
        GameObject buttonObj = new GameObject("OpenMenuButton");
        buttonObj.transform.SetParent(canvas.transform, false);
        
        // Thêm hình ảnh cho nút
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = buttonColor;
        
        // Thiết lập vị trí và kích thước nút
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0, 1);
        buttonRect.anchorMax = new Vector2(0, 1);
        buttonRect.pivot = new Vector2(0, 1);
        buttonRect.sizeDelta = buttonSize;
        buttonRect.anchoredPosition = buttonPosition;
        
        // Thêm text cho nút
        GameObject textObj = new GameObject("ButtonText");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        Text buttonTextComponent = textObj.AddComponent<Text>();
        buttonTextComponent.text = buttonText;
        buttonTextComponent.color = textColor;
        buttonTextComponent.alignment = TextAnchor.MiddleCenter;
        buttonTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonTextComponent.fontSize = 20;
        
        // Thiết lập vị trí text
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        // Thêm button component và sự kiện click
        Button button = buttonObj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = buttonColor;
        colors.highlightedColor = new Color(
            buttonColor.r + 0.1f, 
            buttonColor.g + 0.1f, 
            buttonColor.b + 0.1f, 
            buttonColor.a
        );
        colors.pressedColor = new Color(
            buttonColor.r - 0.1f, 
            buttonColor.g - 0.1f, 
            buttonColor.b - 0.1f, 
            buttonColor.a
        );
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
    }
    
    public void OpenMenu()
    {
        // Nếu có menu cụ thể được chỉ định
        if (menuToOpen != null)
        {
            menuToOpen.SetActive(true);
            Time.timeScale = 0f; // Tạm dừng game
        }
        // Nếu có SimpleMenuOpener
        else if (menuOpener != null)
        {
            menuOpener.ToggleMenu();
        }
        // Nếu không có cả hai, thử tìm menu theo tên
        else
        {
            GameObject menu = GameObject.Find("MenuPanel");
            if (menu != null)
            {
                menu.SetActive(true);
                Time.timeScale = 0f; // Tạm dừng game
            }
            else
            {
                Debug.LogWarning("Không tìm thấy menu để mở!");
                
                // Tìm SimpleMenuOpener một lần nữa
                menuOpener = FindObjectOfType<SimpleMenuOpener>();
                if (menuOpener != null)
                {
                    menuOpener.ToggleMenu();
                }
            }
        }
    }
}
