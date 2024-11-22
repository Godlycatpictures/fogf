using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ByggPlacerare : MonoBehaviour
{
    public Grid grid; // grid
    public ByggValet aktivByggnad; // vilken byggnad via byggvalet.cs
    public LayerMask Byggnader; // r�tt layer
    public GameObject enKusligByggnad; // orka med tv� skripts previews �r h�r fr�n och med nu

    // eventsystem f�r b�ttre saker
    public static event Action<Vector3> ByggnadPlaceradEvent; // s�ger till allt och alla att en byggnad har placerats

    private readonly Dictionary<Vector3Int, GameObject> placeradeByggnader = new Dictionary<Vector3Int, GameObject>(); // plats

    public SceneInfo sceneInfo; // scene info f�r kol o s�nt

    // l�ter rakennushajoittaja ta bort fr�n dictionary
    public static ByggPlacerare Instance { get; private set; }

    private void Start()
    {
        BobGone(); // den ska bort i b�rjan d� man inte vill ha den i b�rjan

    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {

        visaKusligaByggnaden();

        if (Input.GetMouseButtonDown(0) && !IntePlaceraVidKnappar())
        {
            testaPlaceraByggnad();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            BobGone();
        }
    }

    private void testaPlaceraByggnad()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos);

        if (!placeradeByggnader.ContainsKey(gridPosition))
        {
            placeraByggnaden(gridPosition);
        }
        else
        {
            Debug.Log("kan ej placeras h�r, tagen av byggnad (en annan)");
        }
    }
    private void visaKusligaByggnaden()
    {
        if (enKusligByggnad == null) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos);
        Vector3 finalPosition = grid.CellToWorld(gridPosition);
        //finalPosition.z = -9;

        enKusligByggnad.transform.position = finalPosition;

        Renderer renderer = enKusligByggnad.GetComponent<Renderer>();
        renderer.material.color = placeradeByggnader.ContainsKey(gridPosition) ? Color.red : Color.green; // byt mellan gr�n/r�d ifall placerbar eller ej
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
        Debug.Log("placerad byggnad i " + gridPosition);

        Vector3 finalPosition = grid.CellToWorld(gridPosition);
        //finalPosition.z = -8;

        GameObject buildingPrefab = aktivByggnad.getAktivByggnad();
        GameObject newBuilding = Instantiate(buildingPrefab, finalPosition, Quaternion.identity);
        newBuilding.layer = LayerMask.NameToLayer("Byggnader");

        placeradeByggnader[gridPosition] = newBuilding;
        Debug.Log("byggnad sparas i " + gridPosition);

        // Notify listeners
        ByggnadPlaceradEvent?.Invoke(finalPosition);
    }

    public void ordBoksBorttagaren(Vector3Int gridPosition) // kallas fr�n rakennushajoittaja
    {

        if (placeradeByggnader.TryGetValue(gridPosition, out GameObject byggnad))
        {
            Debug.Log("borttagen " + gridPosition);
            Destroy(byggnad);
            placeradeByggnader.Remove(gridPosition);
        }
        else
        {
            Debug.LogWarning($"inget hittades vid {gridPosition}. finns f�r nuvarande vid: {string.Join(", ", placeradeByggnader.Keys)}");
        }
    }

    public void BobGone()
    {
        gameObject.SetActive(false); // hejd� byggare bob
        Destroy(enKusligByggnad); // brot med det l�skiga
    }

    private bool IntePlaceraVidKnappar()
    {
        return EventSystem.current.IsPointerOverGameObject(); // inte vid knappar
    }
}
