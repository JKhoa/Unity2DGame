using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<UIManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("UI Manager");
                    instance = go.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    private Canvas mainCanvas;
    private CanvasScaler canvasScaler;

    [Header("Health UI")]
    public Slider healthBar;
    public TextMeshProUGUI healthText;

    [Header("Experience UI")]
    public Slider expBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;

    [Header("Weapon UI")]
    public Image weaponIcon;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponStats;

    [Header("Enemy UI")]
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI waveText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetupUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetupUI()
    {
        // Tìm hoặc tạo main canvas
        mainCanvas = FindAnyObjectByType<Canvas>();
        if (mainCanvas == null)
        {
            GameObject canvasObj = new GameObject("Main Canvas");
            mainCanvas = canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Thiết lập Canvas
        mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        mainCanvas.sortingOrder = 100; // Đảm bảo UI luôn hiển thị trên cùng

        // Thiết lập CanvasScaler
        canvasScaler = mainCanvas.GetComponent<CanvasScaler>();
        if (canvasScaler != null)
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0.5f; // 0.5 để cân bằng giữa width và height
        }

        // Tạo EventSystem nếu chưa có
        if (FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    public void OptimizeUIElement(GameObject uiElement)
    {
        if (uiElement == null) return;

        // Tối ưu hóa Image component
        Image image = uiElement.GetComponent<Image>();
        if (image != null)
        {
            // Đảm bảo pixel perfect
            image.preserveAspect = true;
            
            // Tối ưu material
            if (image.material != null)
            {
                image.material.SetInt("_PixelSnap", 1);
            }
        }

        // Tối ưu hóa Text component
        Text text = uiElement.GetComponent<Text>();
        if (text != null)
        {
            // Đảm bảo text sắc nét
            text.fontSize = Mathf.RoundToInt(text.fontSize);
            text.alignByGeometry = true;
        }

        // Đặt RectTransform
        RectTransform rect = uiElement.GetComponent<RectTransform>();
        if (rect != null)
        {
            // Đảm bảo vị trí pixel perfect
            Vector3 pos = rect.localPosition;
            rect.localPosition = new Vector3(
                Mathf.Round(pos.x),
                Mathf.Round(pos.y),
                pos.z
            );
        }
    }

    public void SetupHealthBar(GameObject healthBar)
    {
        if (healthBar == null) return;

        // Thiết lập RectTransform
        RectTransform rect = healthBar.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.sizeDelta = new Vector2(200, 20); // Kích thước health bar
        }

        // Tối ưu hóa các component UI
        OptimizeUIElement(healthBar);

        // Thiết lập Slider nếu có
        Slider slider = healthBar.GetComponent<Slider>();
        if (slider != null)
        {
            // Đảm bảo smooth filling
            slider.wholeNumbers = false;
            
            // Tối ưu các image trong slider
            if (slider.fillRect != null)
                OptimizeUIElement(slider.fillRect.gameObject);
            if (slider.handleRect != null)
                OptimizeUIElement(slider.handleRect.gameObject);
            if (slider.targetGraphic != null)
                OptimizeUIElement(slider.targetGraphic.gameObject);
        }
    }

    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        }
    }

    public void UpdateExpUI(float currentExp, float expToNextLevel, int currentLevel)
    {
        if (expBar != null)
        {
            expBar.maxValue = expToNextLevel;
            expBar.value = currentExp;
        }

        if (expText != null)
        {
            expText.text = $"{Mathf.CeilToInt(currentExp)} / {Mathf.CeilToInt(expToNextLevel)}";
        }

        if (levelText != null)
        {
            levelText.text = $"Level {currentLevel}";
        }
    }

    public void UpdateWeaponUI(string name, Sprite icon, string stats)
    {
        if (weaponIcon != null)
        {
            weaponIcon.sprite = icon;
        }

        if (weaponName != null)
        {
            weaponName.text = name;
        }

        if (weaponStats != null)
        {
            weaponStats.text = stats;
        }
    }

    public void UpdateEnemyUI(int enemyCount, int currentWave)
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"Enemies: {enemyCount}";
        }

        if (waveText != null)
        {
            waveText.text = $"Wave {currentWave}";
        }
    }
}