using UnityEngine;
using UnityEngine.Events;

public class PlayerLevel : MonoBehaviour
{
    public static PlayerLevel Instance { get; private set; }

    [Header("Level Settings")]
    public int level = 1;
    public float currentExp = 0;
    public float expToNextLevel = 100;
    public float expMultiplier = 1.2f;

    private UpgradeManager upgradeManager;

    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }

    public IntEvent onLevelUp = new IntEvent();
    public FloatEvent onExpGain = new FloatEvent();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        upgradeManager = Object.FindFirstObjectByType<UpgradeManager>();
        UpdateUI();
    }

    public void GainExp(float amount)
    {
        currentExp += amount;
        onExpGain.Invoke(currentExp);
        
        while (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        currentExp -= expToNextLevel;
        expToNextLevel *= expMultiplier;
        
        // Gọi event lên level
        onLevelUp.Invoke(level);
        
        if (upgradeManager != null)
        {
            upgradeManager.ShowUpgradeOptions();
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        UIManager uiManager = Object.FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateExpUI(currentExp, expToNextLevel, level);
        }
    }
} 