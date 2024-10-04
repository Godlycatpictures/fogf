using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Unit1 : MonoBehaviour
{
    private AIPath path;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private float stoppingDistance;
    private float distanceToTarget;
    void Start()
    {
        path = GetComponent<AIPath>();
    }

   
    void Update()
    {
        path.maxSpeed = moveSpeed;
        
        distanceToTarget = Vector2.Distance(transform.position, target.position);
        if(distanceToTarget < stoppingDistance)
        {
            path.destination = transform.position;
        }
        else
        {
            path.destination = target.position;
        }
    }
}
