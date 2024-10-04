using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    public Transform player;        // Reference to the player's transform
    public RectTransform minimapUI; // Reference to the minimap UI RectTransform
    public RectTransform icon;      // Reference to the icon RectTransform

    // Bounds of the game world, set these manually
    public Vector2 worldMin;
    public Vector2 worldMax;

    void Update()
    {
        // Get the player's position relative to the world bounds
        Vector2 relativePosition = new Vector2(
            (player.position.x - worldMin.x) / (worldMax.x - worldMin.x),
            (player.position.y - worldMin.y) / (worldMax.y - worldMin.y)
        );

        // Update the icon position based on the minimap size
        Vector2 iconPosition = new Vector2(
            relativePosition.x * minimapUI.sizeDelta.x,
            relativePosition.y * minimapUI.sizeDelta.y
        );

        icon.anchoredPosition = iconPosition;
    }
}

