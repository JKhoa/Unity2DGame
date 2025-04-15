using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
public class EnemyMovement : MonoBehaviour
{
    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;
    private Transform player;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (destinationSetter != null)
        {
            destinationSetter.target = player;
        }
    }

    void Update()
    {
        if (aiPath.reachedDestination)
        {
            // Đã đến đích, có thể thêm hành động tại đây
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PlayerBullet"))
        {
            Destroy(gameObject);
        }
    }

    // Thêm OnDrawGizmos để debug hướng di chuyển
    void OnDrawGizmos()
    {
        if (Application.isPlaying && aiPath != null && aiPath.hasPath)
        {
            Gizmos.color = Color.red;
            Vector3 targetPoint = aiPath.destination;
            Gizmos.DrawLine(transform.position, targetPoint);
        }
    }
} 