using Unity.VisualScripting;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    [SerializeField] private float grapplerLength = 4f;
    [SerializeField] private LayerMask grapplerLayer;
    [SerializeField] private LineRenderer rope;
    [SerializeField] private float targetGrapplerLength = 2f;
    [SerializeField] private float retractSpeed = 1f;

    private DistanceJoint2D joint;
    private Transform bestPoint;
    private bool isGrappling = false;
    private Vector3 grapplerPoint;

    void Start()
    {
        joint = gameObject.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        rope.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isGrappling)
        {
            FindBestPoint();
            if (bestPoint != null)
            {
                isGrappling = true;
                grapplerPoint = bestPoint.position;
                grapplerPoint.z = 0;

                // Thiết lập joint
                joint.connectedAnchor = grapplerPoint;
                joint.enabled = true;
                joint.distance = Vector3.Distance(transform.position, grapplerPoint);

                // Hiển thị rope
                rope.SetPosition(0, grapplerPoint);
                rope.SetPosition(1, transform.position);
                rope.enabled = true;

                StartCoroutine(RetractGrappler());
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isGrappling = false;
            joint.enabled = false;
            rope.enabled = false;
        }

        if (rope.enabled)
        {
            rope.SetPosition(1, transform.position);
        }
    }

    void FindBestPoint()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, grapplerLength, grapplerLayer);
        float bestScore = -Mathf.Infinity;
        bestPoint = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.CompareTag("Ground"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance <= grapplerLength)
                {
                    float heightScore = hit.transform.position.y;
                    float distanceScore = hit.transform.position.x;
                    float score = heightScore + distanceScore;

                    if (score > bestScore)                                                                          
                    {
                        bestScore = score;
                        bestPoint = hit.transform;
                    }
                }
            }
        }
    }

    IEnumerator RetractGrappler()
    {
        float currentDistance = joint.distance;
        float elapsedTime = 0f;
        float startDistance = currentDistance;

        while (joint.enabled && currentDistance > targetGrapplerLength)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime * retractSpeed;
            currentDistance = Mathf.Lerp(startDistance, targetGrapplerLength, t);
            joint.distance = currentDistance;
            yield return null;
        }

        if (joint.enabled)
        {
            joint.distance = targetGrapplerLength;
        }
    }
}
