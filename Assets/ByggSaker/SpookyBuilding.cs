using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyBuilding : MonoBehaviour
{
    
    private GameObject currentSpookBuild;
    public Grid grid;
    public ByggValet aktivByggnad; // aktivbyggnad

    void Update()
    {
        // aktivbyggnad^2
        GameObject spookBuild = aktivByggnad.getAktivByggnad();

        // "sp�k"byggnad
        if (currentSpookBuild == null || currentSpookBuild.name != spookBuild.name + "(Clone)")
        {
            if (currentSpookBuild != null) Destroy(currentSpookBuild); 
            currentSpookBuild = Instantiate(spookBuild); 
            currentSpookBuild.SetActive(true);
        }
        if (Input.GetMouseButtonDown(1)) // on�dig?
        {
            hejhejpreview(); // liksom den kallas vid ByggPlacerare.cs men os�ker och orka ta bart
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos);
        Vector3 finalPosition = grid.CellToWorld(gridPosition);
        finalPosition.z = -9;
        currentSpookBuild.transform.position = finalPosition;

        
    }

    public void hejhejpreview()
    {
        if (currentSpookBuild != null)
        {
            Destroy(currentSpookBuild);
        }
    }


}
