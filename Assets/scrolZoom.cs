using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 5f; // Speed of zoom
    [SerializeField] private float minZoom = 2f; // Minimum zoom size
    [SerializeField] private float maxZoom = 10f; // Maximum zoom size

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraZoom script needs to be attached to a Camera!");
        }
    }

    private void Update()
    {
        HandleZoom();
    }

    private void HandleZoom()
    {
        if (cam == null) return;

        // Get the scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");



        // Adjust the orthographic size based on scroll input
        cam.orthographicSize -= scrollInput * zoomSpeed;

        // Clamp the size to the min and max zoom values
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }
}

