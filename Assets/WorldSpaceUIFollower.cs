using UnityEngine;

public class WorldSpaceUIFollower : MonoBehaviour
{
    [Header("Settings")]
    public Transform target;              // Transform của player để follow
    public Vector3 offset = new Vector3(0, 1.5f, 0);  // Offset từ player
    public bool lookAtCamera = true;      // Có quay về phía camera không
    
    private Camera mainCamera;
    private Canvas canvas;

    private void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponent<Canvas>();

        if (target == null)
        {
            // Tự động tìm player nếu không được gán
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("Không tìm thấy Player! Hãy gán target hoặc gán tag 'Player' cho player.");
            }
        }

        if (canvas != null)
        {
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = mainCamera;
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Cập nhật vị trí
            transform.position = target.position + offset;

            // Quay về phía camera nếu được bật
            if (lookAtCamera && mainCamera != null)
            {
                transform.forward = mainCamera.transform.forward;
            }
        }
    }
} 