using UnityEngine;

// Script tự động thêm menu vào scene khi game chạy
public class MenuBootstrap : MonoBehaviour
{
    // Script này sẽ chạy trước tất cả các script khác
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Debug.Log("MenuBootstrap: Khởi tạo menu...");
        
        // Tạo GameObject cho menu
        GameObject menuObject = new GameObject("SuperSimpleMenu");
        
        // Đảm bảo GameObject này không bị hủy khi chuyển scene
        DontDestroyOnLoad(menuObject);
        
        // Thêm script menu vào GameObject
        menuObject.AddComponent<SuperSimpleMenuButton>();
        
        Debug.Log("MenuBootstrap: Đã thêm menu vào game thành công!");
    }
}
