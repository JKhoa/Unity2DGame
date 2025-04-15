using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;
    public Button button;

    public void Setup(string name, string description, Sprite icon)
    {
        nameText.text = name;
        descriptionText.text = description;
        iconImage.sprite = icon;
    }
} 