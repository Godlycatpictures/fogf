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
    public Transform target;
    void Start()
    {
        path = GetComponent<AIPath>();
    }

   
    void Update()
    {
        path.maxSpeed = moveSpeed;

        //kollar efter GameObject med tag("target")
        target = GameObject.FindGameObjectWithTag("Target").transform;

        
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
    public void SetTarget(Transform NewTarget)
    {
        target = NewTarget;
    }

    private void OnCollisionEnter2D(Collider2D other)
    {
        if (other.CompareTag("Target"))
        {
            StartCoroutine(DestroyTarget(other.gameObject, 1f));
        }
    }
    private IEnumerator DestroyTarget(GameObject targetObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(targetObject);
        Debug.Log("Target destroyed: " + targetObject.name);
    }
}
