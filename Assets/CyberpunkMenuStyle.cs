using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CyberpunkMenuStyle : MonoBehaviour
{
    [Header("Styling")]
    public Color primaryColor = new Color(1f, 0.8f, 0f, 1f); // Yellow
    public Color secondaryColor = new Color(0.9f, 0.2f, 0.2f, 1f); // Red
    public Color backgroundColor = new Color(0.1f, 0.1f, 0.15f, 0.9f); // Dark blue
    public Color textColor = new Color(0.9f, 0.9f, 0.9f, 1f); // White
    public Color selectedTextColor = new Color(1f, 0.8f, 0f, 1f); // Yellow
    
    [Header("Fonts")]
    public TMP_FontAsset titleFont;
    public TMP_FontAsset menuFont;
    
    [Header("UI Elements")]
    public Image backgroundPanel;
    public Image logoImage;
    public TextMeshProUGUI titleText;
    public GameObject menuItemsContainer;
    
    [Header("Effects")]
    public bool useGlitchEffect = true;
    public float glitchInterval = 3f;
    public float glitchDuration = 0.2f;
    
    private TextMeshProUGUI[] menuTexts;
    private float nextGlitchTime;
    
    void Start()
    {
        // Apply cyberpunk style to all menu elements
        ApplyStyle();
        
        // Set up glitch effect timing
        nextGlitchTime = Time.time + Random.Range(glitchInterval * 0.5f, glitchInterval * 1.5f);
    }
    
    void Update()
    {
        // Apply random glitch effects to menu items
        if (useGlitchEffect && Time.time > nextGlitchTime)
        {
            StartGlitchEffect();
            nextGlitchTime = Time.time + Random.Range(glitchInterval * 0.5f, glitchInterval * 1.5f);
        }
    }
    
    void ApplyStyle()
    {
        // Apply background styling
        if (backgroundPanel != null)
        {
            backgroundPanel.color = backgroundColor;
        }
        
        // Apply title styling
        if (titleText != null)
        {
            titleText.color = primaryColor;
            if (titleFont != null)
                titleText.font = titleFont;
            titleText.text = "GRAPPLER SHOOTING";
        }
        
        // Get all menu item texts
        if (menuItemsContainer != null)
        {
            menuTexts = menuItemsContainer.GetComponentsInChildren<TextMeshProUGUI>();
            
            // Apply styling to each menu item
            foreach (TextMeshProUGUI text in menuTexts)
            {
                text.color = textColor;
                if (menuFont != null)
                    text.font = menuFont;
                
                // Add outline effect
                if (text != null)
                {
                    text.outlineWidth = 0.2f;
                    text.outlineColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
                }
            }
        }
    }
    
    void StartGlitchEffect()
    {
        if (menuTexts == null || menuTexts.Length == 0)
            return;
            
        // Select a random menu item to glitch
        int randomIndex = Random.Range(0, menuTexts.Length);
        TextMeshProUGUI textToGlitch = menuTexts[randomIndex];
        
        // Save original text
        string originalText = textToGlitch.text;
        
        // Apply glitch effect
        ApplyTextGlitch(textToGlitch);
        
        // Reset after glitch duration
        Invoke("ResetGlitchEffect", glitchDuration);
    }
    
    void ApplyTextGlitch(TextMeshProUGUI text)
    {
        if (text == null)
            return;
            
        // Save original values
        Color originalColor = text.color;
        
        // Apply glitch effect
        text.color = secondaryColor;
        
        // Add random characters to text
        string originalText = text.text;
        string glitchedText = "";
        
        for (int i = 0; i < originalText.Length; i++)
        {
            if (Random.value > 0.7f)
            {
                // Replace with random character
                char randomChar = (char)Random.Range(33, 126);
                glitchedText += randomChar;
            }
            else
            {
                glitchedText += originalText[i];
            }
        }
        
        text.text = glitchedText;
    }
    
    void ResetGlitchEffect()
    {
        if (menuTexts == null)
            return;
            
        // Reset all texts to their original state
        foreach (TextMeshProUGUI text in menuTexts)
        {
            // Get the original text from the button name
            Button parentButton = text.GetComponentInParent<Button>();
            if (parentButton != null)
            {
                string originalText = parentButton.name.Replace("Button", "");
                text.text = originalText.ToUpper();
            }
            
            text.color = textColor;
        }
    }
}
