using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public float speed = 5f; // Adjust movement speed

    private Vector3 targetPosition;

    void Start()
    {
        // Set the target position to the player's starting position
        targetPosition = transform.position;
    }

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse position in world coordinates
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z; // Keep the player's Z position unchanged
        }

        // Move the player towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
