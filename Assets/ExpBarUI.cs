using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBarUI : MonoBehaviour
{
    [Header("References")]
    public Slider expBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI expGainText;

    [Header("Settings")]
    public float expGainDisplayTime = 2f;
    public Color expGainTextColor = Color.yellow;

    private Camera playerCamera;
    private Canvas canvas;
    private float expGainTimer;

    private void Start()
    {
        // Tự động tìm components nếu chưa được gán
        if (expBar == null)
        {
            expBar = GetComponentInChildren<Slider>();
            if (expBar == null)
            {
                Debug.LogError("Không tìm thấy Slider trong ExpBarUI!");
            }
        }

        if (levelText == null)
        {
            levelText = transform.Find("LevelText")?.GetComponent<TextMeshProUGUI>();
        }

        if (expText == null)
        {
            expText = transform.Find("ExpText")?.GetComponent<TextMeshProUGUI>();
        }

        if (expGainText == null)
        {
            expGainText = transform.Find("ExpGainText")?.GetComponent<TextMeshProUGUI>();
            if (expGainText != null)
            {
                expGainText.color = expGainTextColor;
                expGainText.gameObject.SetActive(false);
            }
        }

        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("Không tìm thấy Main Camera!");
        }

        // Lấy reference đến Canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {  
            Debug.LogError("ExpBarUI phải là con của một Canvas!");
        }
        else
        {
            // Thiết lập Canvas cho World Space
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = playerCamera;
        }

        // Khởi tạo giá trị ban đầu cho UI
        if (expBar != null)
        {
            expBar.minValue = 0;
            expBar.value = 0;
        }
    }

    private void Update()
    {
        // Xử lý hiển thị và ẩn exp gain text
        if (expGainText != null && expGainText.gameObject.activeSelf)
        {
            expGainTimer -= Time.deltaTime;
            if (expGainTimer <= 0)
            {
                expGainText.gameObject.SetActive(false);
            }
        }
    }

    private void LateUpdate()
    {
        if (canvas != null && playerCamera != null)
        {
            // Luôn quay về phía camera
            canvas.transform.forward = playerCamera.transform.forward;
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
            expText.text = $"{Mathf.FloorToInt(currentExp)} / {Mathf.FloorToInt(expToNextLevel)}";
        }

        if (levelText != null)
        {
            levelText.text = $"Level {currentLevel}";
        }
    }

    public void ShowExpGain(float expAmount)
    {
        if (expGainText != null)
        {
            expGainText.gameObject.SetActive(true);
            expGainText.text = $"+{Mathf.FloorToInt(expAmount)} EXP";
            expGainTimer = expGainDisplayTime;
        }
    }
}
