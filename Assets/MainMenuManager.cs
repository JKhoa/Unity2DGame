using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Items")]
    public Button continueButton;
    public Button newGameButton;
    public Button loadGameButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button exitButton;

    [Header("UI Elements")]
    public GameObject mainMenuPanel;
    public TextMeshProUGUI gameTitle;
    public Image backgroundImage;
    
    [Header("Settings")]
    public Color selectedColor = new Color(0.9f, 0.3f, 0.3f, 1f);
    public Color normalColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    public AudioClip hoverSound;
    public AudioClip selectSound;
    
    private AudioSource audioSource;
    
    void Awake()
    {
        // Add audio source for menu sounds
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        // Setup button listeners
        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueGame);
        
        if (newGameButton != null)
            newGameButton.onClick.AddListener(StartNewGame);
        
        if (loadGameButton != null)
            loadGameButton.onClick.AddListener(LoadGame);
        
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);
        
        if (creditsButton != null)
            creditsButton.onClick.AddListener(ShowCredits);
        
        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
        
        // Setup button hover events
        SetupButtonHoverEvents(continueButton);
        SetupButtonHoverEvents(newGameButton);
        SetupButtonHoverEvents(loadGameButton);
        SetupButtonHoverEvents(settingsButton);
        SetupButtonHoverEvents(creditsButton);
        SetupButtonHoverEvents(exitButton);
    }
    
    void SetupButtonHoverEvents(Button button)
    {
        if (button == null) return;
        
        // Get text component
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText == null) return;
        
        // Add event triggers
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();
            
        // Add select event
        EventTrigger.Entry selectEntry = new EventTrigger.Entry();
        selectEntry.eventID = EventTriggerType.Select;
        selectEntry.callback.AddListener((data) => {
            if (buttonText != null)
                buttonText.color = selectedColor;
            
            if (audioSource != null && hoverSound != null)
                audioSource.PlayOneShot(hoverSound);
        });
        trigger.triggers.Add(selectEntry);
        
        // Add deselect event
        EventTrigger.Entry deselectEntry = new EventTrigger.Entry();
        deselectEntry.eventID = EventTriggerType.Deselect;
        deselectEntry.callback.AddListener((data) => {
            if (buttonText != null)
                buttonText.color = normalColor;
        });
        trigger.triggers.Add(deselectEntry);
    }
    
    // Button action methods
    public void ContinueGame()
    {
        PlaySelectSound();
        // Load the last saved game
        SceneManager.LoadScene("SampleScene");
    }
    
    public void StartNewGame()
    {
        PlaySelectSound();
        // Start a new game
        SceneManager.LoadScene("SampleScene");
    }
    
    public void LoadGame()
    {
        PlaySelectSound();
        // Open load game menu
        Debug.Log("Load Game menu would open here");
    }
    
    public void OpenSettings()
    {
        PlaySelectSound();
        // Open settings menu
        Debug.Log("Settings menu would open here");
    }
    
    public void ShowCredits()
    {
        PlaySelectSound();
        // Show credits
        Debug.Log("Credits would show here");
    }
    
    public void ExitGame()
    {
        PlaySelectSound();
        // Exit the game
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    private void PlaySelectSound()
    {
        if (audioSource != null && selectSound != null)
            audioSource.PlayOneShot(selectSound);
    }
}
