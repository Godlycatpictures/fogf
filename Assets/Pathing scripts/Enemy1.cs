using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    private AIPath path;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform target;  // Player/Unit
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float wanderRadius;
    [SerializeField] private Vector2 wanderIntervalRange = new Vector2(1.0f, 3.0f);  // Random range for wander intervals

    private Vector3 wanderDestination;
    private float wanderTimer;
    private float currentWanderInterval;

    private void Start()
    {
        path = GetComponent<AIPath>();
        path.maxSpeed = moveSpeed;  // Ensure the speed is set on the AIPath component

        if (target == null) // If no target is set, find the player dynamically
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player"); // Assuming the player has a "Player" tag
            if (player != null)
            {
                target = player.transform;
            }
        }

        SetRandomWanderDestination(); // Set an initial random destination
        SetRandomWanderInterval();    // Set an initial random wander interval
    }

    void Update()
    {
        path.maxSpeed = moveSpeed;

        if (target == null) return;  // Exit if there's no valid target

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Check if the target is within the detection radius
        if (distanceToTarget <= detectionRadius)
        {
            // Move towards the target if it's outside the stopping distance
            if (distanceToTarget > stoppingDistance)
            {
                path.destination = target.position;
            }
            else
            {
                // Stop moving if within stopping distance
                path.destination = transform.position;
            }
        }
        else
        {
            // Wander randomly when the target is not within the detection radius
            WanderAround();
        }
    }

    private void WanderAround()
    {
        // Update the wander destination at random intervals
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= currentWanderInterval)
        {
            SetRandomWanderDestination();
            SetRandomWanderInterval(); // Set a new random interval
            wanderTimer = 0;
        }

        // Set the destination to the wander point
        path.destination = wanderDestination;
    }

    private void SetRandomWanderDestination()
    {
        // Choose a random point within the wander radius
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        wanderDestination = new Vector3(transform.position.x + randomPoint.x, transform.position.y, transform.position.z + randomPoint.y);
    }

    private void SetRandomWanderInterval()
    {
        // Set a random interval between the specified range
        currentWanderInterval = Random.Range(wanderIntervalRange.x, wanderIntervalRange.y);
    }

    // Optional: Draw the detection and wander radii in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
