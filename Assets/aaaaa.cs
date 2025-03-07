using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject[] resourcePrefabs; // Assign resource prefabs in the Inspector
    public int resourceCount = 100; // Number of resources to spawn
    private BoxCollider2D spawnArea;

    void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>(); // Get the BoxCollider2D
        if (spawnArea == null)
        {
            Debug.LogError("No BoxCollider2D found! Please add one to define the spawn area.");
            return;
        }

        SpawnResources();
    }

    void SpawnResources()
    {
        for (int i = 0; i < resourceCount; i++)
        {
            Vector2 randomPosition = GetRandomPositionInArea();
            GameObject resource = Instantiate(GetRandomResource(), randomPosition, Quaternion.identity);
            resource.transform.parent = transform; // Optional: Keep hierarchy clean
        }
    }

    Vector2 GetRandomPositionInArea()
    {
        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

    GameObject GetRandomResource()
    {
        return resourcePrefabs[Random.Range(0, resourcePrefabs.Length)];
    }
}

