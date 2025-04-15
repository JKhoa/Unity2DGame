using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBarSetup : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject expBarPrefab; // Nếu bạn đã có prefab sẵn
    
    [Header("UI Settings")]
    public Color expBarColor = new Color(0.2f, 0.6f, 1f); // Màu xanh dương
    public Vector2 expBarSize = new Vector2(300, 20);
    public Vector2 expBarPosition = new Vector2(0, 40);
    
    private Canvas mainCanvas;
    private GameObject expBarObject;
    private ExpBarUI expBarUI;
    
    void Awake()
    {
        // Tìm Canvas chính
        mainCanvas = Object.FindFirstObjectByType<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError("[ExpBarSetup] Không tìm thấy Canvas trong scene!");
            return;
        }
    }
    
    void Start()
    {
        // Tạo thanh EXP nếu chưa có
        if (UIManager.Instance != null && UIManager.Instance.expBar == null)
        {
            CreateExpBar();
        }
    }
    
    public void CreateExpBar()
    {
        if (mainCanvas == null) return;
        
        // Tạo container cho thanh EXP
        expBarObject = new GameObject("ExpBarUI");
        expBarObject.transform.SetParent(mainCanvas.transform, false);
        
        // Thiết lập RectTransform
        RectTransform rect = expBarObject.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0);
        rect.anchorMax = new Vector2(0.5f, 0);
        rect.pivot = new Vector2(0.5f, 0);
        rect.anchoredPosition = expBarPosition;
        rect.sizeDelta = new Vector2(expBarSize.x + 20, expBarSize.y + 40); // Thêm khoảng trống cho text
        
        // Thêm component ExpBarUI
        expBarUI = expBarObject.AddComponent<ExpBarUI>();
        
        // Tạo background
        GameObject background = CreateUIElement("Background", expBarObject.transform);
        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 0);
        bgRect.anchorMax = new Vector2(1, 0);
        bgRect.pivot = new Vector2(0.5f, 0);
        bgRect.anchoredPosition = new Vector2(0, 20);
        bgRect.sizeDelta = new Vector2(0, expBarSize.y);
        
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        
        // Tạo thanh EXP (Slider)
        GameObject sliderObj = CreateUIElement("ExpSlider", expBarObject.transform);
        RectTransform sliderRect = sliderObj.GetComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0, 0);
        sliderRect.anchorMax = new Vector2(1, 0);
        sliderRect.pivot = new Vector2(0.5f, 0);
        sliderRect.anchoredPosition = new Vector2(0, 20);
        sliderRect.sizeDelta = new Vector2(0, expBarSize.y);
        
        Slider slider = sliderObj.AddComponent<Slider>();
        slider.transition = Selectable.Transition.None;
        slider.navigation = Navigation.defaultNavigation;
        slider.interactable = false;
        
        // Tạo fill area
        GameObject fillArea = CreateUIElement("Fill Area", sliderObj.transform);
        RectTransform fillAreaRect = fillArea.GetComponent<RectTransform>();
        fillAreaRect.anchorMin = new Vector2(0, 0);
        fillAreaRect.anchorMax = new Vector2(1, 1);
        fillAreaRect.pivot = new Vector2(0.5f, 0.5f);
        fillAreaRect.anchoredPosition = Vector2.zero;
        fillAreaRect.sizeDelta = Vector2.zero;
        
        // Tạo fill
        GameObject fill = CreateUIElement("Fill", fillArea.transform);
        RectTransform fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0, 0);
        fillRect.anchorMax = new Vector2(1, 1);
        fillRect.pivot = new Vector2(0.5f, 0.5f);
        fillRect.anchoredPosition = Vector2.zero;
        fillRect.sizeDelta = Vector2.zero;
        
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = expBarColor;
        
        // Thiết lập slider
        slider.fillRect = fillRect;
        slider.targetGraphic = null;
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = 0;
        slider.maxValue = 100;
        slider.value = 0;
        slider.wholeNumbers = false;
        
        // Tạo text hiển thị EXP
        GameObject expTextObj = CreateUIElement("ExpText", expBarObject.transform);
        RectTransform expTextRect = expTextObj.GetComponent<RectTransform>();
        expTextRect.anchorMin = new Vector2(0.5f, 0);
        expTextRect.anchorMax = new Vector2(0.5f, 0);
        expTextRect.pivot = new Vector2(0.5f, 0);
        expTextRect.anchoredPosition = new Vector2(0, 20);
        expTextRect.sizeDelta = new Vector2(expBarSize.x, expBarSize.y);
        
        TextMeshProUGUI expText = expTextObj.AddComponent<TextMeshProUGUI>();
        expText.text = "0 / 100";
        expText.fontSize = 14;
        expText.color = Color.white;
        expText.alignment = TextAlignmentOptions.Center;
        
        // Tạo text hiển thị Level
        GameObject levelTextObj = CreateUIElement("LevelText", expBarObject.transform);
        RectTransform levelTextRect = levelTextObj.GetComponent<RectTransform>();
        levelTextRect.anchorMin = new Vector2(0.5f, 0);
        levelTextRect.anchorMax = new Vector2(0.5f, 0);
        levelTextRect.pivot = new Vector2(0.5f, 0);
        levelTextRect.anchoredPosition = new Vector2(0, 45);
        levelTextRect.sizeDelta = new Vector2(expBarSize.x, 20);
        
        TextMeshProUGUI levelText = levelTextObj.AddComponent<TextMeshProUGUI>();
        levelText.text = "Level 1";
        levelText.fontSize = 16;
        levelText.color = Color.white;
        levelText.alignment = TextAlignmentOptions.Center;
        
        // Gán references vào ExpBarUI
        expBarUI.expSlider = slider;
        expBarUI.expText = expText;
        expBarUI.levelText = levelText;
        
        // Gán vào UIManager nếu có
        if (UIManager.Instance != null)
        {
            UIManager.Instance.expBar = slider;
            UIManager.Instance.expText = expText;
            UIManager.Instance.levelText = levelText;
        }
        
        Debug.Log("[ExpBarSetup] Đã tạo thanh EXP UI thành công!");
    }
    
    private GameObject CreateUIElement(string name, Transform parent)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        obj.AddComponent<RectTransform>();
        return obj;
    }
    
    // Phương thức này có thể được gọi từ Inspector để tạo thanh EXP
    [ContextMenu("Create Exp Bar")]
    public void CreateExpBarFromEditor()
    {
        if (Application.isPlaying) return;
        
        Debug.Log("Sử dụng phương thức này trong Play Mode để tạo thanh EXP.");
    }
}
