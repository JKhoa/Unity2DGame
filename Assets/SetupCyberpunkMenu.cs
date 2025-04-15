using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
// Script để tự động thêm menu vào scene
public class SetupCyberpunkMenu : MonoBehaviour
{
    [MenuItem("GameObject/UI/Cyberpunk Menu")]
    public static void CreateCyberpunkMenu()
    {
        // Tạo GameObject mới
        GameObject menuObject = new GameObject("CyberpunkMenu");
        
        // Thêm EmergencyMenuButton component
        menuObject.AddComponent<EmergencyMenuButton>();
        
        // Đặt vị trí của GameObject
        menuObject.transform.position = Vector3.zero;
        
        // Chọn GameObject trong hierarchy
        Selection.activeGameObject = menuObject;
        
        Debug.Log("Đã tạo CyberpunkMenu GameObject với EmergencyMenuButton script!");
    }
}
#endif
