using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 1.5f, 0);
    private Transform target;
    private Slider slider;
    private Camera mainCamera;
    private Image fillImage;
    private bool isInitialized = false;

    void Awake()
    {
        Debug.Log($"[HealthBar] Awake - GameObject: {gameObject.name}");
        mainCamera = Camera.main;
        SetupSlider();
    }

    void SetupSlider()
    {
        Debug.Log("[HealthBar] Setting up Slider");
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogError("[HealthBar] Slider component not found!");
            return;
        }

        // Thiết lập slider
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 1f;
        slider.interactable = false;

        // Tìm fill image
        fillImage = slider.fillRect?.GetComponent<Image>();
        if (fillImage == null)
        {
            Debug.LogError("[HealthBar] Fill Image not found!");
            return;
        }

        // Đặt màu mặc định là xanh lá
        fillImage.color = Color.green;
        
        isInitialized = true;
        Debug.Log("[HealthBar] Setup completed successfully");
    }

    void LateUpdate()
    {
        if (target == null || mainCamera == null) return;

        // Cập nhật vị trí
        Vector3 worldPosition = target.position + offset;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // Kiểm tra xem đối tượng có nằm phía trước camera không
        if (screenPosition.z < 0)
        {
            screenPosition *= -1;
        }

        // Đảm bảo vị trí pixel perfect
        transform.position = new Vector3(
            Mathf.Round(screenPosition.x),
            Mathf.Round(screenPosition.y),
            transform.position.z
        );

        // Ẩn health bar khi đối tượng ở ngoài màn hình
        bool isVisible = screenPosition.x > 0 && screenPosition.x < Screen.width &&
                        screenPosition.y > 0 && screenPosition.y < Screen.height;
        gameObject.SetActive(isVisible);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (!isInitialized)
        {
            SetupSlider();
        }
        
        // Đảm bảo thanh máu hiển thị
        gameObject.SetActive(true);
        
        Debug.Log($"[HealthBar] Target set to: {(newTarget ? newTarget.name : "null")}");
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (!isInitialized)
        {
            SetupSlider();
        }
        
        if (slider == null || fillImage == null)
        {
            Debug.LogError("[HealthBar] Cannot update health - slider or fillImage is null");
            return;
        }

        float healthPercent = Mathf.Clamp01(currentHealth / maxHealth);
        slider.value = healthPercent;

        // Cập nhật màu dựa trên phần trăm máu - từ xanh lá sang đỏ khi máu giảm
        fillImage.color = Color.Lerp(Color.red, Color.green, healthPercent);
        Debug.Log($"[HealthBar] Health updated: {currentHealth}/{maxHealth} = {healthPercent:F2}, Color: {fillImage.color}");
    }
    
    public void SetHealthBarActive(bool active)
    {
        gameObject.SetActive(active);
        Debug.Log($"[HealthBar] Setting health bar active: {active}");
    }
    
    public bool IsInitialized()
    {
        return isInitialized;
    }
}