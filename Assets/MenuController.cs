using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }
    
    [Header("Menu Settings")]
    public GameObject mainMenuObject;
    public string mainMenuSceneName = "MainMenuScene";
    public KeyCode menuToggleKey = KeyCode.Escape;
    
    private bool isMenuOpen = false;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // If we have a reference to the menu object, make sure it's initially closed
        if (mainMenuObject != null)
        {
            mainMenuObject.SetActive(false);
        }
    }
    
    void Update()
    {
        // Check for menu toggle key press
        if (Input.GetKeyDown(menuToggleKey))
        {
            ToggleMenu();
        }
    }
    
    /// <summary>
    /// Toggle the menu open/closed
    /// </summary>
    public void ToggleMenu()
    {
        if (isMenuOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
    
    /// <summary>
    /// Open the menu
    /// </summary>
    public void OpenMenu()
    {
        // If we have a direct reference to the menu object
        if (mainMenuObject != null)
        {
            mainMenuObject.SetActive(true);
            isMenuOpen = true;
            
            // Pause the game
            Time.timeScale = 0f;
        }
        // If we need to load the menu scene
        else if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            // Check if the scene exists
            if (SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + mainMenuSceneName + ".unity") != -1)
            {
                SceneManager.LoadScene(mainMenuSceneName);
                isMenuOpen = true;
            }
            else
            {
                Debug.LogError("Menu scene not found: " + mainMenuSceneName);
            }
        }
        else
        {
            Debug.LogError("No menu reference or scene name specified!");
        }
    }
    
    /// <summary>
    /// Close the menu
    /// </summary>
    public void CloseMenu()
    {
        // If we have a direct reference to the menu object
        if (mainMenuObject != null)
        {
            mainMenuObject.SetActive(false);
            isMenuOpen = false;
            
            // Resume the game
            Time.timeScale = 1f;
        }
        // If we loaded the menu scene, go back to the game scene
        else if (!string.IsNullOrEmpty(mainMenuSceneName) && SceneManager.GetActiveScene().name == mainMenuSceneName)
        {
            SceneManager.LoadScene("SampleScene"); // Replace with your game scene name
            isMenuOpen = false;
            
            // Resume the game
            Time.timeScale = 1f;
        }
    }
    
    /// <summary>
    /// Check if the menu is currently open
    /// </summary>
    public bool IsMenuOpen()
    {
        return isMenuOpen;
    }
}
