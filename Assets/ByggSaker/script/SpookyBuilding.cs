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

        // "spök"byggnad
        if (currentSpookBuild == null || currentSpookBuild.name != spookBuild.name + "(Clone)")
        {
            if (currentSpookBuild != null) Destroy(currentSpookBuild); 
            currentSpookBuild = Instantiate(spookBuild); 
            currentSpookBuild.SetActive(true);
            currentSpookBuild.layer = LayerMask.NameToLayer("ByggnaderPreview"); // annars interfareare eller vad det än kallas med placeringslogiken(den är ovanför så man kna ej placera med det här fixar det väldigt bra extrem bra)
        }
        if (Input.GetMouseButtonDown(1)) // onödig?
        {
            hejhejpreview(); // liksom den kallas vid ByggPlacerare.cs men osäker och orka ta bart
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // displayar den (samma logik som byggplacerare)
        Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos);
        Vector3 finalPosition = grid.CellToWorld(gridPosition);
        finalPosition.z = -9;
        currentSpookBuild.transform.position = finalPosition;

        
    }

    public void hejhejpreview() // man behöver inte preview om man inte håller på att bygga saker väl?
    {
        if (currentSpookBuild != null)
        {
            Destroy(currentSpookBuild);
        }
    }


}
