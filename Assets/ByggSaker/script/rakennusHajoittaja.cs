using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class rakennusHajoittaja : MonoBehaviour
{
    
    public Grid grid; // Vilken grid
    public LayerMask Byggnader; // layer

    private void Start()
    {
        Debug.Log("start försötarer");
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IntePlaceraVidKnappar()) // egentligen behövs det inte tittas ifall den placerar vid knappar då den inte kan placera, men bra då jag vet inte orka ta bort den kan va bra för att förebygga problem som kan förekomma ifall man skulle kunna ta bort gridden vid knapparna.
        {
            Debug.Log("klick");
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // musposition
            Vector2 mousePosition2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            // raycast till musen
            RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero, Mathf.Infinity, Byggnader);

            if (hit.collider != null) // fanns det en byggnad där
            {
                Destroy(hit.collider.gameObject); // byggnad borta :c
                Debug.Log("byggnad borta :C");
            }
            else
            {
                Debug.Log("tomt");
            }
        }

    }

    private bool IntePlaceraVidKnappar() // i eftertanke så är det väldigt bra att titta ifall den är vid en knapp
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
