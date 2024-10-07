using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObjectSpawner : MonoBehaviour
{
    public GameObject invisiblePrefab; 
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            SpawnInvisibleObject();
        }
    }

    void SpawnInvisibleObject()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

 
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

      
        if (hit.collider == null)
        {
            InstantiateInvisibleObject(mousePosition);
            Debug.Log("Created invisible object at: " + mousePosition);
        }
    }

    void InstantiateInvisibleObject(Vector2 position)
    {
        if (invisiblePrefab != null)
        {
            Instantiate(invisiblePrefab, position, Quaternion.identity);
        }
    }
}
