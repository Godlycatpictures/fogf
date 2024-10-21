using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lampalogiken : MonoBehaviour
{

    public bool harLedning = false;
    public float lampRange = 10f;
    public resourcemanagerscript imJustHereForCoal;

    public GameObject annanLampa;

    private void FixedUpdate()
    {
        

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("lampa") ) // ska göra en radie, tag behövs inte (blev klar 16:22 orka fixa idad :D )
        {
            // Actions to take if the object has the tag "Enemy"
            Debug.Log("har en lampa i närheten!");
        }
    }
}
