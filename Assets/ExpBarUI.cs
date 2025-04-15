using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBarUI : MonoBehaviour
{
    [Header("EXP Bar References")]
    public Slider expSlider;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelText;
    
    [Header("Animation Settings")]
    public float animationSpeed = 5f;
    public Color expBarColor = new Color(0.2f, 0.6f, 1f); // Màu xanh dương cho thanh EXP
    
    private float targetExpValue;
    private float currentDisplayedExp;
    private float maxExpValue;
    private int currentLevel;
    
    private PlayerLevel playerLevel;
    private Image fillImage;
    
    void Awake()
    {
        // Tìm PlayerLevel component
        playerLevel = FindFirstObjectByType<PlayerLevel>();
        
        // Lấy reference đến fill image của slider
        if (expSlider != null)
        {
            fillImage = expSlider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = expBarColor;
            }
        }
        
        // Đăng ký các sự kiện từ PlayerLevel
        if (playerLevel != null)
        {
            playerLevel.onExpGain.AddListener(OnExpGained);
            playerLevel.onLevelUp.AddListener(OnLevelUp);
        }
    }
    
    void Start()
    {
        if (playerLevel != null)
        {
            // Khởi tạo giá trị
            targetExpValue = playerLevel.currentExp;
            currentDisplayedExp = targetExpValue;
            maxExpValue = playerLevel.expToNextLevel;
            currentLevel = playerLevel.level;
            
            // Cập nhật UI
            UpdateUI();
        }
    }
    
    void Update()
    {
        // Làm mượt animation thanh EXP
        if (currentDisplayedExp != targetExpValue)
        {
            currentDisplayedExp = Mathf.Lerp(currentDisplayedExp, targetExpValue, Time.deltaTime * animationSpeed);
            
            // Nếu đã gần đến giá trị mục tiêu, đặt chính xác
            if (Mathf.Abs(currentDisplayedExp - targetExpValue) < 0.1f)
            {
                currentDisplayedExp = targetExpValue;
            }
            
            // Cập nhật UI
            UpdateSliderAndText();
        }
    }
    
    void OnExpGained(float newExpValue)
    {
        targetExpValue = playerLevel.currentExp;
        maxExpValue = playerLevel.expToNextLevel;
    }
    
    void OnLevelUp(int newLevel)
    {
        currentLevel = newLevel;
        maxExpValue = playerLevel.expToNextLevel;
        
        // Hiệu ứng khi lên cấp
        PlayLevelUpEffect();
        
        // Cập nhật UI
        UpdateUI();
    }
    
    void UpdateUI()
    {
        // Cập nhật level text
        if (levelText != null)
        {
            levelText.text = $"Level {currentLevel}";
        }
        
        // Cập nhật slider và text
        UpdateSliderAndText();
    }
    
    void UpdateSliderAndText()
    {
        // Cập nhật slider
        if (expSlider != null)
        {
            expSlider.maxValue = maxExpValue;
            expSlider.value = currentDisplayedExp;
        }
        
        // Cập nhật text
        if (expText != null)
        {
            expText.text = $"{Mathf.Floor(currentDisplayedExp)} / {Mathf.Floor(maxExpValue)}";
        }
    }
    
    void PlayLevelUpEffect()
    {
        // Tạo hiệu ứng flash cho thanh EXP khi lên cấp
        if (fillImage != null)
        {
            // Lưu màu hiện tại
            Color originalColor = fillImage.color;
            
            // Đổi màu thành trắng
            fillImage.color = Color.white;
            
            // Đặt lại màu sau một khoảng thời gian
            Invoke("ResetColor", 0.2f);
        }
    }
    
    void ResetColor()
    {
        if (fillImage != null)
        {
            fillImage.color = expBarColor;
        }
    }
    
    // Phương thức này có thể được gọi từ bên ngoài để hiển thị animation EXP tăng
    public void ShowExpGain(float amount)
    {
        // Hiển thị số EXP tăng lên trên đầu thanh EXP
        if (expText != null)
        {
            // Tạo text hiệu ứng
            GameObject expGainObj = new GameObject("ExpGainText");
            expGainObj.transform.SetParent(transform);
            
            RectTransform rect = expGainObj.AddComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, 30); // Hiển thị phía trên thanh EXP
            
            TextMeshProUGUI tmpText = expGainObj.AddComponent<TextMeshProUGUI>();
            tmpText.text = $"+{Mathf.Floor(amount)} EXP";
            tmpText.fontSize = 16;
            tmpText.color = new Color(1f, 0.8f, 0.2f); // Màu vàng
            tmpText.alignment = TextAlignmentOptions.Center;
            
            // Animation
            StartCoroutine(AnimateExpGainText(expGainObj));
        }
    }
    
    System.Collections.IEnumerator AnimateExpGainText(GameObject textObj)
    {
        RectTransform rect = textObj.GetComponent<RectTransform>();
        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        
        float duration = 1.5f;
        float startTime = Time.time;
        
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            
            // Di chuyển lên trên
            rect.anchoredPosition = new Vector2(0, 30 + t * 50);
            
            // Mờ dần
            Color color = text.color;
            color.a = 1 - t;
            text.color = color;
            
            yield return null;
        }
        
        Destroy(textObj);
    }
}
