using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lampalogiken : MonoBehaviour
{

    public bool harLedning = false; // ifall den �r n�ra npnting n�ra en generator
    public float lampRange = 10f; // med den h�r radien ish
    public bool placerad = false;


    public SceneInfo sceneInfo; // scene info f�r kol o s�nt

    private void Start()
    {
        if (placerad)
        {
            drainingCoal();
        }
    }
    void drainingCoal()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("lampa") ) // ska g�ra en radie, tag beh�vs inte (blev klar 16:22 orka fixa idad :D )
        {
            // Actions to take if the object has the tag "Enemy"
            Debug.Log("har en lampa i n�rheten!");
        }
    }
}
