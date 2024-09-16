using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByggPlacerare : MonoBehaviour
{
    public Grid grid; // Vilken grid
    public GameObject buildingPrefab; // Vilken byggnad
    public SpookyBuilding spookyBuildings;

    /*
    private void Awake()
    {
        buildingPrefab = GetComponent<GameObject>();
        buildingPrefab = GameObject.FindGameObjectWithTag("").transform;
    }
    */

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Tar musens position
            Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos); // Konverterar musen till cell/grid
            Vector3 finalPosition = grid.CellToWorld(gridPosition); // Grid snappar byggnaden till närmsta cell (till musen)
            finalPosition.z = -8;

            // Bygg
            Instantiate(buildingPrefab, finalPosition, Quaternion.identity);

            // göm spöket
            //spookyBuildings.noHalloween();
        }
    }
}
