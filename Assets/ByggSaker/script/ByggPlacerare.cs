using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ByggPlacerare : MonoBehaviour
{
    public Grid grid; // Vilken grid
    public ByggValet aktivByggnad; // aktiv byggnad via annan skript
    public SpookyBuilding deleteremovebanish; // hatar den h�r, hatar hur det blev komplicerat
    public LayerMask Byggnader;

    private void Start()
    {
        BobGone();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IntePlaceraVidKnappar())
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Tar musens position
            Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos); // Konverterar musen till cell/grid
            Vector3 finalPosition = grid.CellToWorld(gridPosition); // Grid snappar byggnaden till n�rmsta cell (till musen)
            finalPosition.z = -8;

            Collider2D hitCollider = Physics2D.OverlapPoint(finalPosition, Byggnader);
            Debug.Log("tittar p�: " + finalPosition + " - upptagen: " + (hitCollider != null));


            if (hitCollider == null) // tittar ifall platsen �r ledig
            {
                GameObject buildingPrefab = aktivByggnad.getAktivByggnad();
                GameObject newBuilding = Instantiate(buildingPrefab, finalPosition, Quaternion.identity);
                newBuilding.layer = LayerMask.NameToLayer("Byggnader"); // r�tt layer
                Debug.Log("byggnad placerad vid: " + finalPosition);

                extractorlogik extractor = newBuilding.GetComponent<extractorlogik>();
                if (extractor != null)
                {
                    extractor.placerad = true;
                }

            }
            else
            {
                Debug.Log("ej placerbar h�r!(redan tagen av annan byggnad)" + hitCollider.name);
            }


        }
        if (Input.GetMouseButtonDown(1))
        {
            BobGone();
        }
    }

    public void BobGone()
    {
        gameObject.SetActive(false);

        deleteremovebanish.hejhejpreview(); // va inte roligt att fixa det h�r
    }

    private bool IntePlaceraVidKnappar()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

   
}
