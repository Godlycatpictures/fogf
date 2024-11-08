using System.Collections.Generic;
using UnityEngine;

public class GroupMovementManager : MonoBehaviour
{
    [SerializeField] private float unitSpacing = 1.5f;  // Spacing between units to avoid overlap
    [SerializeField] private LayerMask unitLayerMask;   // Layer mask to detect other units

    // Moves selected units to target positions, ensuring no overlap
    public void MoveUnitsToTarget(List<GameObject> selectedUnits, Vector3 targetPosition)
    {
        // Move the first unit to the exact target position
        if (selectedUnits.Count > 0)
        {
            selectedUnits[0].GetComponent<Unit1>().SetTarget(targetPosition);
        }

        // Assign new positions to remaining units
        for (int i = 1; i < selectedUnits.Count; i++)
        {
            Vector3 newPosition = FindNonOverlappingPosition(targetPosition, selectedUnits[i].GetComponent<Collider2D>(), i);
            selectedUnits[i].GetComponent<Unit1>().SetTarget(newPosition);
        }
    }

    // Find a nearby, non-overlapping position for a unit
    Vector3 FindNonOverlappingPosition(Vector3 targetPosition, Collider2D unitCollider, int unitIndex)
    {
        float radius = unitCollider.bounds.extents.x + unitSpacing;  // Calculate radius needed to avoid overlap
        Vector3 newPosition = targetPosition;
        int maxAttempts = 100;  // Maximum number of attempts to find a valid position
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            // Calculate position in a circular pattern around the target
            float angle = attempts * (360f / maxAttempts);
            float radians = angle * Mathf.Deg2Rad;

            newPosition = targetPosition + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * radius;

            if (!IsPositionOccupied(newPosition, unitCollider))
            {
                break;  // Position is free
            }

            attempts++;
        }

        return newPosition;
    }

    // Check if a calculated position overlaps with other units
    bool IsPositionOccupied(Vector3 position, Collider2D unitCollider)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, unitCollider.bounds.extents.x, unitLayerMask);

        foreach (Collider2D collider in colliders)
        {
            if (collider != unitCollider)
            {
                return true;  // Position is occupied
            }
        }

        return false;  // Position is free
    }

    // Optional: Draw Gizmos to visualize the avoid radius
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, unitSpacing);
        }
    }
}

