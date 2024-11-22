using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Camera mainCamera;  // The main camera that moves in the game world
    public RectTransform minimapRect;  // The RectTransform of the minimap (Raw Image)
    public Camera minimapCamera;  // The camera for the minimap
    public float mapWorldWidth = 100f;  // The width of the world map (in game units)
    public float mapWorldHeight = 100f; // The height of the world map (in game units)

    // Detect when the minimap is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        // Convert the clicked UI position to local coordinates relative to the minimap
        Vector2 localCursor;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRect, eventData.position, eventData.pressEventCamera, out localCursor);

        // Normalize the local coordinates (0 to 1) relative to the minimap size
        Vector2 normalizedPos = new Vector2(
            (localCursor.x + minimapRect.rect.width / 2) / minimapRect.rect.width,
            (localCursor.y + minimapRect.rect.height / 2) / minimapRect.rect.height);

        // Map the normalized minimap coordinates to world coordinates
        Vector3 worldPosition = new Vector3(
            normalizedPos.x * mapWorldWidth,
            normalizedPos.y * mapWorldHeight,
            mainCamera.transform.position.z);  // Keep the camera's current Z position

        // Move the main camera to the new position
        mainCamera.transform.position = worldPosition;
    }
}
