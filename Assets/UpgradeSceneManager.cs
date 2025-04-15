using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeSceneManager : MonoBehaviour
{
    public static UpgradeSceneManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject buttonContainer;
    public GameObject upgradeButtonPrefab;

    [Header("Upgrade Options")]
    public UpgradeOption[] upgradeOptions;

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
        // Tạm dừng game khi mở upgrade menu
        Time.timeScale = 0f;
        ShowUpgradeOptions();
    }

    public void ShowUpgradeOptions()
    {
        // Xóa các button cũ
        foreach (Transform child in buttonContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Chọn ngẫu nhiên 3 upgrade để hiển thị
        int optionsToShow = 3;
        UpgradeOption[] selectedUpgrades = GetRandomUpgrades(optionsToShow);

        // Tạo button cho mỗi upgrade
        foreach (var upgrade in selectedUpgrades)
        {
            CreateUpgradeButton(upgrade);
        }
    }

    private UpgradeOption[] GetRandomUpgrades(int count)
    {
        // Logic chọn ngẫu nhiên upgrade ở đây
        // Tạm thời trả về 3 upgrade đầu tiên
        UpgradeOption[] result = new UpgradeOption[count];
        for (int i = 0; i < count && i < upgradeOptions.Length; i++)
        {
            result[i] = upgradeOptions[i];
        }
        return result;
    }

    private void CreateUpgradeButton(UpgradeOption upgrade)
    {
        GameObject buttonObj = Instantiate(upgradeButtonPrefab, buttonContainer.transform);
        UpgradeButton upgradeButton = buttonObj.GetComponent<UpgradeButton>();
        
        if (upgradeButton != null)
        {
            upgradeButton.Setup(upgrade.name, upgrade.description, upgrade.icon);
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => OnUpgradeSelected(upgrade));
        }
    }

    public void OnUpgradeSelected(UpgradeOption upgrade)
    {
        // Áp dụng upgrade
        upgrade.Apply();

        // Tiếp tục game
        Time.timeScale = 1f;

        // Unload scene
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("UpgradeScene");
    }
}

[System.Serializable]
public class UpgradeOption
{
    public string name;
    public string description;
    public Sprite icon;
    public UpgradeType type;
    public float value;

    public void Apply()
    {
        switch (type)
        {
            case UpgradeType.Health:
                if (PlayerStats.Instance != null)
                    PlayerStats.Instance.IncreaseMaxHealth(value);
                break;
            case UpgradeType.Damage:
                if (PlayerStats.Instance != null)
                    PlayerStats.Instance.IncreaseDamage(value);
                break;
            case UpgradeType.AttackSpeed:
                if (PlayerStats.Instance != null)
                    PlayerStats.Instance.IncreaseAttackSpeed(value);
                break;
            case UpgradeType.TripleShot:
                if (PlayerShooting.Instance != null)
                    PlayerShooting.Instance.EnableTripleShot(value, 15f);
                break;
            case UpgradeType.DoubleShot:
                if (PlayerShooting.Instance != null)
                    PlayerShooting.Instance.EnableDoubleShot(value);
                break;
            case UpgradeType.PiercingShot:
                if (PlayerShooting.Instance != null)
                    PlayerShooting.Instance.EnablePiercingShot((int)value);
                break;
        }
    }
}

public enum UpgradeType
{
    Health,
    Damage,
    AttackSpeed,
    TripleShot,
    DoubleShot,
    PiercingShot
} 