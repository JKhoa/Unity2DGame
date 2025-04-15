using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuSetup : MonoBehaviour
{
    // This script helps set up the Cyberpunk-style menu in the Unity Editor
    
    [Header("Menu Items")]
    public string[] menuItems = new string[] {
        "CONTINUE",
        "NEW GAME",
        "LOAD GAME",
        "SETTINGS",
        "CREDITS",
        "EXIT GAME"
    };
    
    [Header("Styling")]
    public Color menuBackgroundColor = new Color(0.1f, 0.1f, 0.15f, 0.8f);
    public Color titleColor = new Color(1f, 0.8f, 0f, 1f); // Yellow
    public Color normalTextColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    public Color selectedTextColor = new Color(0.9f, 0.2f, 0.2f, 1f); // Red
    
    [Header("Prefab Creation")]
    public string menuPrefabPath = "Assets/Prefabs/CyberpunkMenu.prefab";
    
    public void SetupMenu()
    {
        // Create canvas if it doesn't exist
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("MainMenuCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        
        // Add EventSystem if it doesn't exist
        if (FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
        
        // Create main menu container
        GameObject menuContainer = new GameObject("CyberpunkMenu");
        menuContainer.transform.SetParent(canvas.transform, false);
        
        // Add the menu manager script
        menuContainer.AddComponent<MainMenuManager>();
        menuContainer.AddComponent<CyberpunkMenuStyle>();
        
        // Create background panel
        GameObject bgPanel = CreateUIElement("BackgroundPanel", menuContainer.transform);
        Image bgImage = bgPanel.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.7f); // Semi-transparent black
        RectTransform bgRect = bgPanel.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        
        // Create logo/title
        GameObject titleObj = CreateUIElement("GameTitle", menuContainer.transform);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "GRAPPLER SHOOTING";
        titleText.fontSize = 60;
        titleText.color = titleColor;
        titleText.alignment = TextAlignmentOptions.Left;
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.05f, 0.8f);
        titleRect.anchorMax = new Vector2(0.5f, 0.95f);
        titleRect.sizeDelta = Vector2.zero;
        
        // Create menu items container
        GameObject menuItemsContainer = CreateUIElement("MenuItems", menuContainer.transform);
        RectTransform menuItemsRect = menuItemsContainer.GetComponent<RectTransform>();
        menuItemsRect.anchorMin = new Vector2(0.05f, 0.2f);
        menuItemsRect.anchorMax = new Vector2(0.3f, 0.7f);
        menuItemsRect.sizeDelta = Vector2.zero;
        
        // Create menu items
        for (int i = 0; i < menuItems.Length; i++)
        {
            CreateMenuItem(menuItems[i], menuItemsContainer.transform, i, menuItems.Length);
        }
        
        // Create version text
        GameObject versionObj = CreateUIElement("VersionText", menuContainer.transform);
        TextMeshProUGUI versionText = versionObj.AddComponent<TextMeshProUGUI>();
        versionText.text = "v1.0";
        versionText.fontSize = 16;
        versionText.color = normalTextColor;
        versionText.alignment = TextAlignmentOptions.Left;
        RectTransform versionRect = versionObj.GetComponent<RectTransform>();
        versionRect.anchorMin = new Vector2(0.05f, 0.05f);
        versionRect.anchorMax = new Vector2(0.15f, 0.1f);
        versionRect.sizeDelta = Vector2.zero;
        
        Debug.Log("Cyberpunk menu setup complete!");
    }
    
    private GameObject CreateUIElement(string name, Transform parent)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        obj.AddComponent<RectTransform>();
        return obj;
    }
    
    private void CreateMenuItem(string itemText, Transform parent, int index, int totalItems)
    {
        // Create button container
        GameObject buttonObj = CreateUIElement(itemText + "Button", parent);
        Button button = buttonObj.AddComponent<Button>();
        
        // Set button position
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        float itemHeight = 1.0f / (totalItems + 1);
        float yPos = 1.0f - (index + 1) * itemHeight;
        buttonRect.anchorMin = new Vector2(0, yPos - itemHeight * 0.4f);
        buttonRect.anchorMax = new Vector2(1, yPos + itemHeight * 0.4f);
        buttonRect.sizeDelta = Vector2.zero;
        
        // Create text element
        GameObject textObj = CreateUIElement("Text", buttonObj.transform);
        TextMeshProUGUI buttonText = textObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = itemText;
        buttonText.fontSize = 28;
        buttonText.color = normalTextColor;
        buttonText.alignment = TextAlignmentOptions.Left;
        
        // Set text to fill button
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        // Set up button colors
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = Color.white;
        colors.pressedColor = Color.white;
        colors.selectedColor = Color.white;
        button.colors = colors;
        
        // Set up button navigation
        Navigation nav = button.navigation;
        nav.mode = Navigation.Mode.Vertical;
        button.navigation = nav;
        
        // Add event trigger for highlight effects
        EventTrigger trigger = buttonObj.AddComponent<EventTrigger>();
        
        // Add select event
        EventTrigger.Entry selectEntry = new EventTrigger.Entry();
        selectEntry.eventID = EventTriggerType.Select;
        selectEntry.callback.AddListener((data) => {
            buttonText.color = selectedTextColor;
        });
        trigger.triggers.Add(selectEntry);
        
        // Add deselect event
        EventTrigger.Entry deselectEntry = new EventTrigger.Entry();
        deselectEntry.eventID = EventTriggerType.Deselect;
        deselectEntry.callback.AddListener((data) => {
            buttonText.color = normalTextColor;
        });
        trigger.triggers.Add(deselectEntry);
    }
    
#if UNITY_EDITOR
    [MenuItem("Tools/Create Cyberpunk Menu")]
    static void CreateMenu()
    {
        // Find or create the MainMenuSetup component
        MainMenuSetup menuSetup = FindAnyObjectByType<MainMenuSetup>();
        if (menuSetup == null)
        {
            GameObject menuSetupObj = new GameObject("MenuSetup");
            menuSetup = menuSetupObj.AddComponent<MainMenuSetup>();
        }
        
        menuSetup.SetupMenu();
    }
#endif
}
