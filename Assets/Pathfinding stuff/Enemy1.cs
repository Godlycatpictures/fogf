using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    private AIPath path;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float wanderRadius;
    [SerializeField] private Vector2 wanderIntervalRange = new Vector2(1.0f, 3.0f);
    private float distanceToTarget;
    private Vector3 wanderDestination;
    private float wanderTimer;
    private float currentWanderInterval;


    private GameObject closestUnit;

    void Start()
    {
        path = GetComponent<AIPath>();
        path.maxSpeed = moveSpeed;

        SetRandomWanderDestination();
        SetRandomWanderInterval();
    }

    void Update()
    {
        path.maxSpeed = moveSpeed;

        FindClosestUnit();

        if (closestUnit != null)
        {
            distanceToTarget = Vector2.Distance(transform.position, closestUnit.transform.position);

            if (distanceToTarget <= detectionRadius)
            {
                if (distanceToTarget > stoppingDistance)
                {
                    path.destination = closestUnit.transform.position;
                }
                else
                {
                    path.destination = transform.position;
                }
            }
            else
            {
                WanderAround();
            }
        }
        else
        {
            WanderAround();
        }
    }

    private void FindClosestUnit()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        float closestDistance = Mathf.Infinity;
        GameObject nearestUnit = null;

        foreach (GameObject unit in units)
        {
            float distanceToUnit = Vector2.Distance(transform.position, unit.transform.position);

            if (distanceToUnit < closestDistance)
            {
                closestDistance = distanceToUnit;
                nearestUnit = unit;
            }
        }

        closestUnit = nearestUnit;
    }

    private void WanderAround()
    {
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= currentWanderInterval)
        {
            SetRandomWanderDestination();
            SetRandomWanderInterval();
            wanderTimer = 0;
        }

        path.destination = wanderDestination;
    }

    private void SetRandomWanderDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        wanderDestination = new Vector3(transform.position.x + randomPoint.x, transform.position.y, transform.position.z + randomPoint.y);
    }

    private void SetRandomWanderInterval()
    {
        currentWanderInterval = Random.Range(wanderIntervalRange.x, wanderIntervalRange.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
