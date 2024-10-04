using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class rakennusHajoittaja : MonoBehaviour
{
    private HashSet<Vector3Int> valdGridPlats = new HashSet<Vector3Int>(); // tittar p� gridden med till�telse/samtycke
    public Grid grid; // Vilken grid
    public GameObject Bobby;

    
    // kanske beh�vde inte g�ra ett nytt skript men det blir b�ttre (ifall det funkar) 

    public void bortByggnadare()
    {
        Bobby.SetActive(false); // g�m byggplaceraren


        if (Input.GetMouseButtonDown(0) && !IntePlaceraVidKnappar()) // egentligen beh�vs det inte tittas ifall den placerar vid knappar d� den inte kan placera, men bra d� jag vet inte orka ta bort den kan va bra f�r att f�rebygga problem som kan f�rekomma ifall man skulle kunna ta bort gridden vid knapparna.
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Tar musens position
            Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos); // Konverterar musen till cell/grid
            Vector3 finalPosition = grid.CellToWorld(gridPosition); // Grid snappar byggnaden till n�rmsta cell (till musen)
            finalPosition.z = -8;

            if (valdGridPlats.Contains(gridPosition)) // tittar ifall oledig
            {
                valdGridPlats.Remove(gridPosition); // fr�n oledigt till 
                Debug.Log("byggnad borta");
            }
        }
    }
    private bool IntePlaceraVidKnappar()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }



    // bortkommenterad f�r referens (fungerade ej originellt i ByggPlacerare.cs)

    /*public void taBortByggnad(Vector3 mousePosition)
    {
        // tittar ifall n�gonting �r d�r
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        Vector3Int gridPosition = grid.WorldToCell(hit.collider.transform.position);


        if (hit.collider != null && valdGridPlats.Contains(gridPosition))
        {
            // om det finns tas det bort
            Destroy(hit.collider.gameObject); // byggnad borta :c
            valdGridPlats.Remove(gridPosition); // fr�n oledigt till ledigt
            Debug.Log("byggnad borta");
        }
        else
        {
            Debug.Log("vad f�rs�ker du ta bort?");
        }
    }*/
}
