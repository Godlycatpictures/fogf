using Pathfinding;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private AIPath path;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.5f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        path = GetComponent<AIPath>();
        targetPosition = transform.position;
    }

    void Update()
    {
        path.maxSpeed = moveSpeed;
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

        if (distanceToTarget < stoppingDistance)
        {
            path.destination = transform.position; // Let AIPath handle stopping
        }
        else
        {
            path.destination = targetPosition;
        }
    }

    public void SetTarget(Vector3 newTargetPosition)
    {
        Debug.Log("Unit received move command to: " + newTargetPosition);

        targetPosition = newTargetPosition;
        isMoving = true;

        if (path == null)
        {
            Debug.LogError("AIPath component is missing!");
            return;
        }

        path.destination = targetPosition;
        path.SearchPath(); // Forces AIPath to recalculate movement
    }
}
