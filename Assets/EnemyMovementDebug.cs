using UnityEngine;
using Pathfinding;

public class EnemyMovementDebug : MonoBehaviour
{
    private AIPath aiPath;
    private AIDestinationSetter aiDest;
    private Rigidbody2D rb;
    private Vector3 lastPosition;
    private Seeker seeker;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        aiDest = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        lastPosition = transform.position;

        if (aiPath != null)
        {
            aiPath.canMove = true;
            aiPath.maxSpeed = 8f;
            aiPath.maxAcceleration = 20f;
        }

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }

        InvokeRepeating("DebugMovement", 1f, 1f);
    }

    void DebugMovement()
    {
        if (aiPath != null && aiDest != null)
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);
            
            Debug.Log($"Enemy {gameObject.name} Debug:" +
                     $"\nPosition: {transform.position}" +
                     $"\nTarget: {(aiDest.target != null ? aiDest.target.position.ToString() : "No Target")}" +
                     $"\nDistance to target: {(aiDest.target != null ? Vector3.Distance(transform.position, aiDest.target.position) : 0)}" +
                     $"\nCan move: {aiPath.canMove}" +
                     $"\nVelocity: {aiPath.velocity}" +
                     $"\nDistance moved: {distanceMoved}" +
                     $"\nScale: {transform.localScale}");

            lastPosition = transform.position;
        }
    }

    void OnDrawGizmos()
    {
        if (seeker != null && seeker.IsDone())
        {
            Gizmos.color = Color.yellow;
            var path = seeker.GetCurrentPath();
            if (path != null && path.vectorPath != null)
            {
                for (int i = 0; i < path.vectorPath.Count - 1; i++)
                {
                    Gizmos.DrawLine(path.vectorPath[i], path.vectorPath[i + 1]);
                }
            }
        }
    }
} 