using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("References")]
    public GameObject upgradeMenuCanvas;
    public Transform buttonContainer;
    public GameObject buttonPrefab;
    public PlayerStats playerStats;

    [System.Serializable]
    public class Upgrade
    {
        public string name;
        public string description;
        public int level;
        public int maxLevel = 5;
        public float[] values;
        public float[] additionalValues; // Cho các skill cần thêm tham số (ví dụ: góc cho Triple Shot)
    }

    [Header("Upgrades")]
    public List<Upgrade> upgrades = new List<Upgrade>();
    private List<Upgrade> currentUpgradeChoices = new List<Upgrade>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeUpgrades();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ẩn menu upgrade khi bắt đầu
        upgradeMenuCanvas.SetActive(false);
    }

    private void InitializeUpgrades()
    {
        // Shield
        upgrades.Add(new Upgrade
        {
            name = "Shield",
            description = "Tạo khiên bảo vệ xoay quanh người chơi",
            level = 0,
            maxLevel = 5,
            values = new float[] { 10f, 15f, 20f, 25f, 30f }, // Sát thương
            additionalValues = new float[] { 2f, 2.5f, 3f, 3.5f, 4f } // Bán kính
        });

        // Triple Shot
        upgrades.Add(new Upgrade
        {
            name = "Triple Shot",
            description = "Bắn 3 viên đạn cùng lúc",
            level = 0,
            maxLevel = 5,
            values = new float[] { 0.8f, 0.85f, 0.9f, 0.95f, 1f }, // Hệ số sát thương
            additionalValues = new float[] { 15f, 20f, 25f, 30f, 35f } // Góc bắn
        });

        // Double Shot
        upgrades.Add(new Upgrade
        {
            name = "Double Shot",
            description = "Bắn 2 viên đạn song song",
            level = 0,
            maxLevel = 5,
            values = new float[] { 0.7f, 0.75f, 0.8f, 0.85f, 0.9f } // Hệ số sát thương
        });

        // Piercing Shot
        upgrades.Add(new Upgrade
        {
            name = "Piercing Shot",
            description = "Đạn xuyên qua kẻ địch",
            level = 0,
            maxLevel = 5,
            values = new float[] { 1f, 2f, 3f, 4f, 5f } // Số lượng xuyên
        });

        // Damage
        upgrades.Add(new Upgrade
        {
            name = "Damage",
            description = "Tăng sát thương",
            level = 0,
            maxLevel = 5,
            values = new float[] { 1.2f, 1.4f, 1.6f, 1.8f, 2f }
        });

        // Speed
        upgrades.Add(new Upgrade
        {
            name = "Speed",
            description = "Tăng tốc độ di chuyển",
            level = 0,
            maxLevel = 5,
            values = new float[] { 1.1f, 1.2f, 1.3f, 1.4f, 1.5f }
        });

        // Health
        upgrades.Add(new Upgrade
        {
            name = "Health",
            description = "Tăng máu tối đa",
            level = 0,
            maxLevel = 5,
            values = new float[] { 1.2f, 1.4f, 1.6f, 1.8f, 2f }
        });
    }

    public void ShowUpgradeOptions()
    {
        // Xóa các button cũ nếu có
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Chọn ngẫu nhiên 3 upgrade
        currentUpgradeChoices.Clear();
        List<Upgrade> availableChoices = new List<Upgrade>(upgrades);
        
        for (int i = 0; i < 3 && availableChoices.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableChoices.Count);
            Upgrade selectedUpgrade = availableChoices[randomIndex];
            
            // Chỉ thêm vào lựa chọn nếu chưa đạt cấp tối đa
            if (selectedUpgrade.level < selectedUpgrade.maxLevel)
            {
                currentUpgradeChoices.Add(selectedUpgrade);
                CreateUpgradeButton(selectedUpgrade, buttonContainer);
            }
            
            availableChoices.RemoveAt(randomIndex);
        }

        // Hiển thị menu
        upgradeMenuCanvas.SetActive(true);
        Time.timeScale = 0f; // Tạm dừng game
    }

    private void CreateUpgradeButton(Upgrade upgrade, Transform parent)
    {
        GameObject buttonObj = Instantiate(buttonPrefab, parent);
        
        // Thiết lập tên
        TextMeshProUGUI nameText = buttonObj.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        nameText.text = upgrade.name;
        
        // Thiết lập mô tả
        TextMeshProUGUI descText = buttonObj.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
        if (upgrade.level > 0)
        {
            descText.text = $"{upgrade.description}\nLevel {upgrade.level + 1}/{upgrade.maxLevel}";
        }
        else
        {
            descText.text = upgrade.description;
        }
        
        // Thêm sự kiện click
        Button button = buttonObj.GetComponent<Button>();
        button.onClick.AddListener(() => ApplyUpgrade(upgrade.name));
    }

    public void ApplyUpgrade(string upgradeName)
    {
        Upgrade upgrade = upgrades.Find(u => u.name == upgradeName);
        if (upgrade != null && upgrade.level < upgrade.maxLevel)
        {
            upgrade.level++;
            float value = upgrade.values[upgrade.level - 1];

            switch (upgradeName)
            {
                case "Shield":
                    float radius = upgrade.additionalValues[upgrade.level - 1];
                    PlayerShooting.Instance.SetShieldDamage(value);
                    PlayerShooting.Instance.SetShieldRadius(radius);
                    PlayerShooting.Instance.SetShieldRotationSpeed(90f);
                    PlayerShooting.Instance.EnableShield(5f);
                    break;

                case "Triple Shot":
                    float angle = upgrade.additionalValues[upgrade.level - 1];
                    PlayerShooting.Instance.EnableTripleShot(value, angle);
                    break;

                case "Double Shot":
                    PlayerShooting.Instance.EnableDoubleShot(value);
                    break;

                case "Piercing Shot":
                    PlayerShooting.Instance.EnablePiercingShot((int)value);
                    break;

                case "Damage":
                    PlayerStats.Instance.damageMultiplier = value;
                    break;

                case "Speed":
                    PlayerStats.Instance.moveSpeedMultiplier = value;
                    break;

                case "Health":
                    PlayerStats.Instance.maxHealthMultiplier = value;
                    PlayerStats.Instance.UpdateMaxHealth();
                    break;
            }
        }

        // Đóng menu và tiếp tục game
        upgradeMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public List<Upgrade> GetRandomUpgrades(int count)
    {
        List<Upgrade> availableUpgrades = upgrades.FindAll(u => u.level < u.maxLevel);
        List<Upgrade> randomUpgrades = new List<Upgrade>();

        while (randomUpgrades.Count < count && availableUpgrades.Count > 0)
        {
            int randomIndex = Random.Range(0, availableUpgrades.Count);
            randomUpgrades.Add(availableUpgrades[randomIndex]);
            availableUpgrades.RemoveAt(randomIndex);
        }

        return randomUpgrades;
    }

    public void ApplyShieldUpgrade()
    {
        if (PlayerShooting.Instance != null)
        {
            PlayerShooting.Instance.EnableShield(5f); // Chỉ truyền duration
        }
    }
} 