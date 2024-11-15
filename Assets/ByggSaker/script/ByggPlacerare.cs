using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ByggPlacerare : MonoBehaviour
{
    public Grid grid; // grid
    public ByggValet aktivByggnad; // vilken byggnad via byggvalet.cs
    public LayerMask Byggnader; // rätt layer
    public GameObject enKusligByggnad; // orka med två skripts previews är här från och med nu

    // eventsystem för bättre saker
    public static event Action<Vector3> ByggnadPlaceradEvent; // säger till allt och alla att en byggnad har placerats

    private Dictionary<Vector3Int, GameObject> placeradeByggnader = new Dictionary<Vector3Int, GameObject>(); // plasts

    void Update()
    {

        visaKusligaByggnaden();

        if (Input.GetMouseButtonDown(0) && !IntePlaceraVidKnappar())
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // tar musens porition
            Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos); // tar musens porition 2

            if (!placeradeByggnader.ContainsKey(gridPosition))
            {
                placeraByggnaden(gridPosition); // byggna till poritionsnen
                // Destroy(enKusligByggnad); // brot med det läskiga BEHÖVS INTE HÄR
            }
            else
            {
                Debug.Log("Ej placerbar här! (redan tagen av annan byggnad) " + placeradeByggnader[gridPosition].name);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            BobGone();
        }
    }
    private void visaKusligaByggnaden()
    {
        if (enKusligByggnad != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // muspositioner x3
            Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos);
            Vector3 finalPosition = grid.CellToWorld(gridPosition);
            finalPosition.z = -9;  // rätt "layer"

            enKusligByggnad.transform.position = finalPosition;

            if (placeradeByggnader.ContainsKey(gridPosition))
            {
                enKusligByggnad.GetComponent<Renderer>().material.color = Color.red; // röd/ ejplacerbar
            }
            else
            {
                enKusligByggnad.GetComponent<Renderer>().material.color = Color.green; // grön/ kan placerbar
            }
        }
    }
    public void SetPreviewBuilding(GameObject buildingPrefab)
    {
        if (enKusligByggnad != null)
        {
            Destroy(enKusligByggnad);  // den ska BORT
        }

        enKusligByggnad = Instantiate(buildingPrefab, Vector3.zero, Quaternion.identity);
        enKusligByggnad.layer = LayerMask.NameToLayer("ByggnaderPreview");
        
    }


    private void placeraByggnaden(Vector3Int gridPosition)
    {
        Vector3 finalPosition = grid.CellToWorld(gridPosition); // finala positionen plus -8 på z värdet
        finalPosition.z = -8;

        GameObject buildingPrefab = aktivByggnad.getAktivByggnad(); // väljer aktiva byggnaden &
        GameObject newBuilding = Instantiate(buildingPrefab, finalPosition, Quaternion.identity); // placerar den
        newBuilding.layer = LayerMask.NameToLayer("Byggnader"); // på den korrekta layern

        // sparar i hashen
        placeradeByggnader[gridPosition] = newBuilding;

        // sparar vilken byggnad placeras
        if (newBuilding.TryGetComponent<extractorlogik>(out var extractor))
            extractor.placerad = true;
        if (newBuilding.TryGetComponent<lampalogiken>(out var lampa))
            lampa.placerad = true;

        Debug.Log("Byggnad placerad vid: " + finalPosition);
        ByggnadPlaceradEvent?.Invoke(finalPosition);
    }

    public void BobGone()
    {
        gameObject.SetActive(false); // hejdå byggare bob
        Destroy(enKusligByggnad); // brot med det läskiga
    }

    private bool IntePlaceraVidKnappar()
    {
        return EventSystem.current.IsPointerOverGameObject(); // inte vid knappar
    }
}
