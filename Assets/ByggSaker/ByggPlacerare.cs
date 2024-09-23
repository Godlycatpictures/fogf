using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ByggPlacerare : MonoBehaviour
{
    public Grid grid; // Vilken grid
    public GameObject buildingPrefab; // Vilken byggnad
    public ByggValet aktivByggnad; // aktiv byggnad via annan skript
    public SpookyBuilding deleteremovebanish;


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IntePlaceraVidKnappar())
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Tar musens position
            Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos); // Konverterar musen till cell/grid
            Vector3 finalPosition = grid.CellToWorld(gridPosition); // Grid snappar byggnaden till närmsta cell (till musen)
            finalPosition.z = -8;

            // välj byggnad
            GameObject buildingPrefab = aktivByggnad.getAktivByggnad();

            // Bygg
            Instantiate(buildingPrefab, finalPosition, Quaternion.identity);

            
        }
        if (Input.GetMouseButtonDown(1))
        {
            gameObject.active = false;

            deleteremovebanish.hejhejpreview(); // va inte roligt att fixa det här
        }
    }

    private bool IntePlaceraVidKnappar()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
