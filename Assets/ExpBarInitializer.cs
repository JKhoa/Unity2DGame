using UnityEngine;

public class ExpBarInitializer : MonoBehaviour
{
    // Singleton instance
    private static ExpBarInitializer instance;
    
    [Header("References")]
    public ExpBarSetup expBarSetup;
    
    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Tự động tạo ExpBarSetup nếu chưa có
        if (expBarSetup == null)
        {
            GameObject setupObj = new GameObject("ExpBarSetup");
            expBarSetup = setupObj.AddComponent<ExpBarSetup>();
            setupObj.transform.SetParent(transform);
        }
    }
    
    void Start()
    {
        // Đảm bảo thanh EXP được tạo khi game bắt đầu
        if (expBarSetup != null)
        {
            expBarSetup.CreateExpBar();
        }
    }
}
