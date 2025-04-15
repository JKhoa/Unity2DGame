using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ExpBar : MonoBehaviour
{
    public Slider expSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
    private PlayerLevel playerLevel;

    void Start()
    {
        // Tìm PlayerLevel component
        playerLevel = Object.FindFirstObjectByType<PlayerLevel>();
        if (playerLevel == null)
        {
            Debug.LogError("[ExpBar] Cannot find PlayerLevel component!");
            return;
        }

        // Thiết lập slider
        if (expSlider == null)
        {
            expSlider = GetComponentInChildren<Slider>();
            if (expSlider == null)
            {
                Debug.LogError("[ExpBar] No Slider found!");
                return;
            }
        }

        SetupExpBar();
        
        // Đăng ký sự kiện với UnityAction
        if (playerLevel.onExpGain != null)
        {
            playerLevel.onExpGain.AddListener((float exp) => UpdateExpBar(exp));
        }
        if (playerLevel.onLevelUp != null)
        {
            playerLevel.onLevelUp.AddListener((int level) => OnLevelUp(level));
        }
    }

    void SetupExpBar()
    {
        if (expSlider != null)
        {
            expSlider.minValue = 0;
            expSlider.maxValue = playerLevel.expToNextLevel;
            expSlider.value = playerLevel.currentExp;
        }

        UpdateUI();
    }

    void UpdateExpBar(float currentExp)
    {
        if (expSlider != null)
        {
            expSlider.value = currentExp;
        }
        UpdateUI();
    }

    void OnLevelUp(int newLevel)
    {
        if (expSlider != null)
        {
            expSlider.maxValue = playerLevel.expToNextLevel;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text = $"Level {playerLevel.level}";
        }

        if (expText != null)
        {
            expText.text = $"{Mathf.FloorToInt(playerLevel.currentExp)} / {Mathf.FloorToInt(playerLevel.expToNextLevel)}";
        }
    }

    void OnDestroy()
    {
        if (playerLevel != null)
        {
            if (playerLevel.onExpGain != null)
            {
                playerLevel.onExpGain.RemoveListener((float exp) => UpdateExpBar(exp));
            }
            if (playerLevel.onLevelUp != null)
            {
                playerLevel.onLevelUp.RemoveListener((int level) => OnLevelUp(level));
            }
        }
    }
} 