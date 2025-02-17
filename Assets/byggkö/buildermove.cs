using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class buildermove : MonoBehaviour
{
    public ByggPlacerare byggPlacerare; // vart byggaren ska gå
    public rakennusjono rakennusjono; // i vilken ordning

    
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public IEnumerator FlyttaTillPosition(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);

        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }
    }
}
