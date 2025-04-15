using UnityEngine;
using Pathfinding;

public class EnemySpacing : MonoBehaviour
{
    public Transform target;
    public Transform EnemyGFX;
    public float speed = 200f;
    public float nextWayPointDistance = 3f;
    
    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 1f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())    
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }    

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }    
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count)
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }

        // Cập nhật hướng nhìn của enemy
        if (force.x >= 0.01f)
        {
            EnemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            EnemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}

