using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    void Update()
    {
        // Check if the P key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Set the camera's position to (0, 0, 0)
            transform.position = new Vector3(0, 0, -10); // Keep the Z position as -10 for 2D cameras
        }
    }
}
