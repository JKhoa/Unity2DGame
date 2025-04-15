using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PointManager : MonoBehaviour
{
    public GameObject pointPrefab;
    public Transform player;
    public float minXDistance = 2f;
    public float maxXDistance = 5f;
    public float minYDistance = 0.2f;
    public float maxYDistance = 0.5f;
    public float lavaYPosition = -5f;
    public float spawnInterval = 0.2f;
    public float despawnDistance = 12f;
    public int minLayers = 1;
    public int maxLayers = 2;
    public float minPointSpacing = 0.8f;
    public float layerHeightOffset = 0.3f;
    public float outsideCameraOffset = 1f;
    public float horizontalVariation = 0.3f;

    private List<GameObject> points = new List<GameObject>();
    private bool isSpawning = false;
    private Camera mainCamera;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing in PointManager!");
            enabled = false;
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            enabled = false;
            return;
        }

        StartCoroutine(SpawnPointsCoroutine());
    }

    void Update()
    {
        if (player == null || mainCamera == null) return;
        DespawnOldPoints();
    }

    IEnumerator SpawnPointsCoroutine()
    {
        isSpawning = true;
        while (isSpawning && player != null && mainCamera != null)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnPoints();
        }
    }

    void SpawnPoints()
    {
        try
        {
            Vector3 spawnPosition = GetRandomPosition();
            if (spawnPosition != Vector3.zero && CanSpawnAtPosition(spawnPosition))
            {
                SpawnPattern(spawnPosition);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in SpawnPoints: {e.Message}");
        }
    }

    Vector3 GetRandomPosition()
    {
        if (player == null) return Vector3.zero;

        float direction = Random.value > 0.5f ? 1f : -1f;
        float x = player.position.x + direction * Random.Range(minXDistance, maxXDistance);
        
        float maxSpawnHeight = player.position.y + maxYDistance;
        float minSpawnHeight = player.position.y - minYDistance;
        float y = Random.Range(minSpawnHeight, maxSpawnHeight);

        if (y < lavaYPosition + 0.5f)
            y = lavaYPosition + 0.5f;

        return new Vector3(x, y, 0);
    }

    void SpawnPattern(Vector3 basePosition)
    {
        if (pointPrefab == null)
        {
            Debug.LogError("Point prefab is missing!");
            return;
        }

        int patternType = Random.Range(0, 3);
        int points = Random.Range(3, 7);

        switch (patternType)
        {
            case 0:
                SpawnVerticalPattern(basePosition, points);
                break;
            case 1:
                SpawnDiagonalPattern(basePosition, points);
                break;
            case 2:
                SpawnCurvedPattern(basePosition, points);
                break;
        }
    }

    void SpawnVerticalPattern(Vector3 basePosition, int count)
    {
        for (int i = 0; i < count; i++)
        {
            float xOffset = Random.Range(-horizontalVariation, horizontalVariation);
            float yOffset = -i * layerHeightOffset;
            Vector3 position = new Vector3(
                basePosition.x + xOffset,
                basePosition.y + yOffset,
                0
            );

            if (CanSpawnAtPosition(position) && position.y >= lavaYPosition)
            {
                SpawnPoint(position);
            }
        }
    }

    void SpawnDiagonalPattern(Vector3 basePosition, int count)
    {
        float direction = Random.value > 0.5f ? 1f : -1f;
        for (int i = 0; i < count; i++)
        {
            float xOffset = direction * i * 0.3f + Random.Range(-horizontalVariation/2, horizontalVariation/2);
            float yOffset = -i * layerHeightOffset;
            Vector3 position = new Vector3(
                basePosition.x + xOffset,
                basePosition.y + yOffset,
                0
            );

            if (CanSpawnAtPosition(position) && position.y >= lavaYPosition)
            {
                SpawnPoint(position);
            }
        }
    }

    void SpawnCurvedPattern(Vector3 basePosition, int count)
    {
        float amplitude = Random.Range(0.5f, 1f);
        float frequency = Random.Range(0.5f, 1f);
        
        for (int i = 0; i < count; i++)
        {
            float progress = (float)i / (count - 1);
            float xOffset = amplitude * Mathf.Sin(progress * frequency * Mathf.PI * 2) 
                         + Random.Range(-horizontalVariation/2, horizontalVariation/2);
            float yOffset = -i * layerHeightOffset;
            
            Vector3 position = new Vector3(
                basePosition.x + xOffset,
                basePosition.y + yOffset,
                0
            );

            if (CanSpawnAtPosition(position) && position.y >= lavaYPosition)
            {
                SpawnPoint(position);
            }
        }
    }

    void SpawnPoint(Vector3 position)
    {
        try
        {
            GameObject newPoint = Instantiate(pointPrefab, position, Quaternion.identity);
            if (newPoint != null)
            {
                points.Add(newPoint);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error spawning point: {e.Message}");
        }
    }

    bool CanSpawnAtPosition(Vector3 position)
    {
        if (points == null) return true;

        for (int i = points.Count - 1; i >= 0; i--)
        {
            if (points[i] == null)
            {
                points.RemoveAt(i);
                continue;
            }

            try
            {
                if (Vector3.Distance(position, points[i].transform.position) < minPointSpacing)
                    return false;
            }
            catch (System.Exception)
            {
                points.RemoveAt(i);
            }
        }
        return true;
    }

    void DespawnOldPoints()
    {
        if (points == null || player == null) return;

        for (int i = points.Count - 1; i >= 0; i--)
        {
            try
            {
                if (points[i] == null)
                {
                    points.RemoveAt(i);
                    continue;
                }

                float distance = Vector3.Distance(points[i].transform.position, player.position);
                if (distance > despawnDistance)
                {
                    Destroy(points[i]);
                    points.RemoveAt(i);
                }
            }
            catch (System.Exception)
            {
                points.RemoveAt(i);
            }
        }
    }

    void OnDisable()
    {
        isSpawning = false;
    }

    void OnDestroy()
    {
        isSpawning = false;
        if (points != null)
        {
            foreach (var point in points)
            {
                if (point != null)
                {
                    Destroy(point);
                }
            }
            points.Clear();
        }
    }
}
