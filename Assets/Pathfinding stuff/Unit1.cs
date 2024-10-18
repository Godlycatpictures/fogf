using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Unit1 : MonoBehaviour
{
    private AIPath path;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stoppingDistance;
    private float distanceToTarget;
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
        if (isMoving)
        {
            distanceToTarget = Vector2.Distance(transform.position, targetPosition);

            if (distanceToTarget < stoppingDistance)
            {
                path.destination = transform.position;
                isMoving = false;
            }
            else
            {
                path.destination = targetPosition;
            }
        }
    }


    public void SetTarget(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        isMoving = true;
    }
}
