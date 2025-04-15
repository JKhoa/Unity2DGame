using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
public class EnemyBorderConstraint : MonoBehaviour
{
    private Vector2 borderCenter;
    private Vector2 borderSize;
    private AIPath aiPath;
    private Transform target;
    private AIDestinationSetter aiDestination;
    private Vector3 lastValidPosition;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        aiDestination = GetComponent<AIDestinationSetter>();
        if (aiDestination != null)
        {
            target = aiDestination.target;
        }
        lastValidPosition = transform.position;
    }

    public void SetBorderBounds(Vector2 center, Vector2 size)
    {
        borderCenter = center;
        borderSize = size;
    }

    void Update()
    {
        if (target == null || aiPath == null) return;

        // Kiểm tra xem player có nằm trong vùng giới hạn không
        bool isTargetInBorder = IsPointInBorder(target.position);

        if (!isTargetInBorder)
        {
            // Nếu player ở ngoài vùng giới hạn, tìm điểm gần nhất trên border
            Vector2 closestPoint = GetClosestPointOnBorder(target.position);
            
            // Chỉ tạo target mới nếu cần thiết
            if (aiDestination.target == target)
            {
                GameObject tempTarget = new GameObject("TempTarget");
                tempTarget.transform.position = closestPoint;
                aiDestination.target = tempTarget.transform;
                Destroy(tempTarget, 0.1f);
            }
        }
        else if (aiDestination.target != target)
        {
            // Nếu player trong vùng giới hạn và chưa là target, set lại target
            aiDestination.target = target;
        }

        // Kiểm tra và giới hạn vị trí của enemy
        Vector3 currentPos = transform.position;
        bool isInsideBorder = IsPointInBorder(currentPos);
        
        if (!isInsideBorder)
        {
            // Nếu enemy ra ngoài border, đưa về vị trí hợp lệ gần nhất
            Vector2 validPos = GetClosestPointOnBorder(currentPos);
            transform.position = new Vector3(validPos.x, validPos.y, currentPos.z);
            
            // Reset velocity nếu cần
            if (aiPath.canMove)
            {
                aiPath.SetPath(null);
            }
        }
        else
        {
            // Lưu lại vị trí hợp lệ
            lastValidPosition = transform.position;
        }
    }

    bool IsPointInBorder(Vector2 point)
    {
        float halfWidth = borderSize.x / 2;
        float halfHeight = borderSize.y / 2;
        
        return point.x >= borderCenter.x - halfWidth &&
               point.x <= borderCenter.x + halfWidth &&
               point.y >= borderCenter.y - halfHeight &&
               point.y <= borderCenter.y + halfHeight;
    }

    Vector2 GetClosestPointOnBorder(Vector2 point)
    {
        float halfWidth = borderSize.x / 2;
        float halfHeight = borderSize.y / 2;
        
        float x = Mathf.Clamp(point.x, borderCenter.x - halfWidth, borderCenter.x + halfWidth);
        float y = Mathf.Clamp(point.y, borderCenter.y - halfHeight, borderCenter.y + halfHeight);
        
        return new Vector2(x, y);
    }
} 