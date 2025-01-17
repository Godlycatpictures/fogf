using System;
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

    public static event Action<Vector3> ByggnadBorttagenEvent; // s�ger till allt och alla att en byggnad har borttagits
    public SceneInfo sceneInfo; // scene info f�r kol o s�nt

    private void Start()
    {
        Debug.Log("start f�rs�tarer");
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IntePlaceraVidKnappar()) // egentligen beh�vs det inte tittas ifall den placerar vid knappar d� den inte kan placera, men bra d� jag vet inte orka ta bort den kan va bra f�r att f�rebygga problem som kan f�rekomma ifall man skulle kunna ta bort gridden vid knapparna.
        {
            vasenNappi();
        } else if (Input.GetMouseButtonDown(1))
        {
            gameObject.SetActive(false);
        }
    }

    private void vasenNappi()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPos2d = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        // leta i r�tt lager
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos2d, Vector2.zero, Mathf.Infinity, Byggnader);

        if (hit.collider != null)
        {
            TaBortByggnad(hit.collider);
        }
        else
        {
            Debug.Log("tomt");
        }
    }

    private void TaBortByggnad(Collider2D collider)
    {
        Vector3 worldPosition = collider.transform.position;
        Vector3Int gridPosition = ByggPlacerare.Instance.grid.WorldToCell(worldPosition);


        // ta bort pls
        ByggPlacerare.Instance.ordBoksBorttagaren(gridPosition);
        Debug.Log("tar bort (kanske hoppas): " + gridPosition);

        //tag bort
        Destroy(collider.gameObject);

        // lampa vill ha den h�r
        ByggnadBorttagenEvent?.Invoke(worldPosition);
    }

    private bool IntePlaceraVidKnappar() // i eftertanke s� �r det v�ldigt bra att titta ifall den �r vid en knapp
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
