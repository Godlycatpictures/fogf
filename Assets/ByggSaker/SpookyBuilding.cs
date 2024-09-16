using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyBuilding : MonoBehaviour
{
    public GameObject spookBuild; // en preview
    private GameObject currentSpookBuild; // Current ghost object
    public Grid grid; // Reference to the Grid component

    void Start()
    {
        currentSpookBuild = Instantiate(spookBuild); // Create ghost object
        currentSpookBuild.SetActive(true);
    }

    void Update()
    {
        // Move ghost object with the mouse, snapping it to the grid
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos);
        currentSpookBuild.transform.position = grid.CellToWorld(gridPosition);

        Vector3 finalPosition = grid.CellToWorld(gridPosition);
        finalPosition.z = -9; // kamera saker (orka med layers än)
        currentSpookBuild.transform.position = finalPosition;
    }

    public void noHalloween()
    {
        currentSpookBuild.SetActive(false); // göm efter objekt blivit placerad
    }
}
