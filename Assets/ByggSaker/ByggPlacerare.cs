using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ByggPlacerare : MonoBehaviour
{
    public Grid grid; // Vilken grid
    public ByggValet aktivByggnad; // aktiv byggnad via annan skript
    public SpookyBuilding deleteremovebanish; // hatar den här, hatar hur det blev komplicerat
    private HashSet<Vector3Int> valdGridPlats = new HashSet<Vector3Int>(); // tittar på gridden med tillåtelse/samtycke


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IntePlaceraVidKnappar())
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Tar musens position
            Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos); // Konverterar musen till cell/grid
            Vector3 finalPosition = grid.CellToWorld(gridPosition); // Grid snappar byggnaden till närmsta cell (till musen)
            finalPosition.z = -8;

            
            if (!valdGridPlats.Contains(gridPosition)) // tittar ifall platsen är ledig
            {
                GameObject buildingPrefab = aktivByggnad.getAktivByggnad(); // välj byggnad utifrån vald prefab
                Instantiate(buildingPrefab, finalPosition, Quaternion.identity); // sätt ut byggnad utifrån vald prefab
                // gör platsen oledigt efter placering till en ledig grid
                valdGridPlats.Add(gridPosition);
            }
            else
            {
                Debug.Log("ej placerbar här!(redan tagen av annan byggnad)");
            }


        }
        if (Input.GetMouseButtonDown(1))
        {
            BobGone();
        }
    }

    public void BobGone()
    {
        gameObject.active = false;

        deleteremovebanish.hejhejpreview(); // va inte roligt att fixa det här
    }

    private bool IntePlaceraVidKnappar()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

   
}
