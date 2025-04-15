using UnityEngine;

public class LavaFollow : MonoBehaviour
{
    [Header("Lava Settings")]
    [SerializeField] private Vector2 lavaSize = new Vector2(20f, 1f); // Kích thước của lava (width, height)
    [SerializeField] private float followSpeed = 5f; // Tốc độ di chuyển theo camera

    private Transform cameraTransform;
    private float initialY; // Lưu vị trí Y ban đầu

    void Start()
    {
        // Tìm camera nếu chưa được gán
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("Không tìm thấy Main Camera! Hãy gán camera vào LavaFollow.");
                enabled = false;
                return;
            }
        }

        // Lưu vị trí Y ban đầu
        initialY = transform.position.y;
        UpdateLavaSize();
    }

    void Update()
    {
        if (cameraTransform == null) return;

        // Chỉ cập nhật vị trí X, giữ nguyên Y
        Vector3 newPosition = transform.position;
        newPosition.x = cameraTransform.position.x;
        newPosition.y = initialY; // Sử dụng vị trí Y ban đầu
        
        // Di chuyển mượt mà đến vị trí mới
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * followSpeed);
    }

    void OnValidate()
    {
        // Cập nhật kích thước ngay khi thay đổi trong Inspector
        UpdateLavaSize();
    }

    void UpdateLavaSize()
    {
        // Cập nhật scale dựa trên lavaSize
        transform.localScale = new Vector3(lavaSize.x, lavaSize.y, 1f);
    }

    // Hàm public để thay đổi kích thước lava từ bên ngoài
    public void SetLavaSize(Vector2 newSize)
    {
        lavaSize = newSize;
        UpdateLavaSize();
    }

    // Thêm hàm để set vị trí Y mới nếu cần
    public void SetYPosition(float newY)
    {
        initialY = newY;
    }
}
