using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ByggPlacerare : MonoBehaviour
{
    public Grid grid; // Vilken grid
    public ByggValet aktivByggnad; // aktiv byggnad via annan skript
    public SpookyBuilding deleteremovebanish; // hatar den h�r, hatar hur det blev komplicerat
    private HashSet<Vector3Int> valdGridPlats = new HashSet<Vector3Int>(); // tittar p� gridden med till�telse/samtycke

    private bool pvgAttTaBort = false; // vill man ta bort, det �r fr�gan?


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IntePlaceraVidKnappar())
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Tar musens position
            Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos); // Konverterar musen till cell/grid
            Vector3 finalPosition = grid.CellToWorld(gridPosition); // Grid snappar byggnaden till n�rmsta cell (till musen)
            finalPosition.z = -8;

            // Check if we're in removal mode
            if (pvgAttTaBort)
            {
                taBortByggnad(mouseWorldPos);
            }
            else
            {
                if (!valdGridPlats.Contains(gridPosition))
                {
                    // tittar ifall platsen �r ledig
                    GameObject buildingPrefab = aktivByggnad.getAktivByggnad(); // v�lj byggnad utifr�n vald prefab
                    Instantiate(buildingPrefab, finalPosition, Quaternion.identity); // s�tt ut byggnad utifr�n vald prefab
                    // g�r platsen oledigt efter placering till en ledig grid
                    valdGridPlats.Add(gridPosition);
                }
                else
                {
                    Debug.Log("ej placerbar h�r!(redan tagen av annan byggnad)");
                }
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            gameObject.active = false;

            deleteremovebanish.hejhejpreview(); // va inte roligt att fixa det h�r
        }
    }

    private bool IntePlaceraVidKnappar()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void ToggleRemovalMode() // funkar ej (tror pga sp�kbuild/previewn)
    {
        pvgAttTaBort = !pvgAttTaBort;
        Debug.Log("mumsarbyggnader: " + (pvgAttTaBort ? "sant" : "falskt"));
        

    }

    public void taBortByggnad(Vector3 mousePosition)
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
    }
}
