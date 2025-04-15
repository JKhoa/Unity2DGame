using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class FixGameErrors : MonoBehaviour
{
    [Header("Health Bar References")]
    public GameObject healthBarPrefab;
    public Canvas healthCanvas;
    public Slider healthSlider;
    
    [Header("Player References")]
    public GameObject player;
    
    void Awake()
    {
        // Fix missing references
        FixMissingReferences();
    }
    
    void FixMissingReferences()
    {
        // Fix health bar references
        if (healthCanvas == null)
        {
            Debug.Log("Creating Health Canvas");
            GameObject canvasObj = new GameObject("Health Canvas");
            healthCanvas = canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            healthCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        
        // Fix health bar prefab
        if (healthBarPrefab == null)
        {
            Debug.Log("Creating Health Bar Prefab");
            healthBarPrefab = new GameObject("HealthBarPrefab");
            healthBarPrefab.transform.SetParent(healthCanvas.transform);
            
            // Add slider component
            if (healthSlider == null)
            {
                healthSlider = healthBarPrefab.AddComponent<Slider>();
                
                // Create background
                GameObject background = new GameObject("Background");
                background.transform.SetParent(healthBarPrefab.transform);
                Image bgImage = background.AddComponent<Image>();
                bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                
                // Create fill area
                GameObject fillArea = new GameObject("Fill Area");
                fillArea.transform.SetParent(healthBarPrefab.transform);
                RectTransform fillRect = fillArea.AddComponent<RectTransform>();
                
                // Create fill
                GameObject fill = new GameObject("Fill");
                fill.transform.SetParent(fillArea.transform);
                Image fillImage = fill.AddComponent<Image>();
                fillImage.color = Color.red;
                
                // Setup slider
                healthSlider.fillRect = fill.GetComponent<RectTransform>();
                healthSlider.targetGraphic = fillImage;
                healthSlider.direction = Slider.Direction.LeftToRight;
                healthSlider.minValue = 0;
                healthSlider.maxValue = 100;
                healthSlider.value = 100;
            }
        }
        
        // Fix player reference
        if (player == null)
        {
            Debug.Log("Searching for Player");
            player = GameObject.FindGameObjectWithTag("Player");
            
            if (player == null)
            {
                Debug.Log("Creating Player GameObject");
                player = new GameObject("Player");
                player.tag = "Player";
                
                // Add basic components
                player.AddComponent<Rigidbody2D>();
                player.AddComponent<BoxCollider2D>();
                
                // Position player
                player.transform.position = new Vector3(0, 0, 0);
            }
        }
        
        // Find and fix PointManager references
        MonoBehaviour[] allComponents = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour component in allComponents)
        {
            if (component.GetType().Name == "PointManager")
            {
                Debug.Log("Fixing PointManager references: " + component.name);
                
                // Use reflection to set player reference if it exists
                FieldInfo playerField = component.GetType().GetField("player");
                if (playerField != null)
                {
                    Debug.Log("Setting player reference in PointManager");
                    playerField.SetValue(component, player.transform);
                }
            }
        }
    }
}
