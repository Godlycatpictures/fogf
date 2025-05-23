using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine;

public class ByggPlacerare : MonoBehaviour
{
    // för byggnande kanske (kommenterat långt efter koden skapats)
    public Grid grid; // grid
    public ByggValet aktivByggnad; // vilken byggnad via byggvalet.cs
    public LayerMask Byggnader; // rätt layer
    public GameObject enKusligByggnad; // orka med två skripts previews är här från och med nu

    // för byggkön
    public GameObject byggnadManVillBygga; // den skickas till rakennusjono.cs
   

    // eventsystem för bättre saker
    public static event Action<Vector3> ByggnadPlaceradEvent; // säger till allt och alla att en byggnad har placerats

    private readonly Dictionary<Vector3Int, GameObject> placeradeByggnader = new Dictionary<Vector3Int, GameObject>(); // plats

    public SceneInfo sceneInfo; // scene info för kol o sånt

    // låter rakennushajoittaja ta bort från dictionary
    public static ByggPlacerare Instance { get; private set; }

    private void Start()
    {
        BobGone(); // den ska bort i början då man inte vill ha den i början

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
        gridPosition.z = -8;

        if (!placeradeByggnader.ContainsKey(gridPosition))
        {
            placeraByggnaden(gridPosition);
        }
        else
        {
            Debug.Log("kan ej placeras här, tagen av byggnad (en annan)");
        }
    }
    private void visaKusligaByggnaden()
    {
        if (enKusligByggnad == null) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos);
        Vector3 finalPosition = grid.CellToWorld(gridPosition);

        gridPosition.z = -8; // vet var byggnad placerad är
        finalPosition.z = -9; // så att den syns över den (previewn)

        enKusligByggnad.transform.position = finalPosition;

        Renderer renderer = enKusligByggnad.GetComponent<Renderer>();
        renderer.material.color = placeradeByggnader.ContainsKey(gridPosition) ? Color.red : Color.green; // byt mellan grön/röd ifall placerbar eller ej
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
        gridPosition.z = -8;
        Debug.Log("placerad byggnad i " + gridPosition);

        Vector3 finalPosition = grid.CellToWorld(gridPosition);
        //finalPosition.z = -8;

        GameObject buildingPrefab = aktivByggnad.getAktivByggnad();
        GameObject newBuilding = Instantiate(buildingPrefab, finalPosition, Quaternion.identity);
        newBuilding.layer = LayerMask.NameToLayer("Byggnader");

        placeradeByggnader[gridPosition] = newBuilding;
        Debug.Log("byggnad sparas i " + gridPosition);

        // för placerad sak i varje byggnad
        if (newBuilding.TryGetComponent<extractorlogik>(out var extractor))
        {
            if (sceneInfo.buildingResource < 30)
            {
                Debug.Log("Du har inte tillräckligt med resurser för att bygga en extractor");
                Destroy(newBuilding);
                return;
            }
            else
            {
                sceneInfo.buildingResource -= 30;
                extractor.placerad = true;
                Debug.Log("extractor placerad");
            }
        }

        if (newBuilding.TryGetComponent<lampalogiken>(out var lampa))
        {
            if (sceneInfo.buildingResource < 10)
            {
                Debug.Log("Du har inte tillr�ckligt med resurser f�r att bygga en lampa");
                Destroy(newBuilding);
                return;
            }
            else
            {
                lampa.placerad = true;
                Debug.Log("lampa placerad");
                sceneInfo.buildingResource -= 10;
            }
        }
        if (newBuilding.TryGetComponent<generatorlogiken>(out var generator))
        {
            if ((sceneInfo.buildingResource < 20) && (sceneInfo.energyResource < 50))
            {
                Debug.Log("Du har inte tillr�ckligt med resurser f�r att bygga en generator");
                Destroy(newBuilding);
                return;
            }
            else
            {
                sceneInfo.buildingResource -= 20;
                sceneInfo.energyResource -= 50;
                generator.placerad = true;
                Debug.Log("generator placerad");
            }
        }

        // Notify listeners
        ByggnadPlaceradEvent?.Invoke(finalPosition);
        lampalogiken.UppdateraAllaLampor();
    }

    public void ordBoksBorttagaren(Vector3Int gridPosition) // kallas från rakennushajoittaja
    {
        gridPosition.z = -8;
        if (placeradeByggnader.TryGetValue(gridPosition, out GameObject byggnad))
        {
            Debug.Log("borttagen " + gridPosition);
            Destroy(byggnad);
            placeradeByggnader.Remove(gridPosition);
        }
        else
        {
            Debug.LogWarning($"inget hittades vid {gridPosition}. finns för nuvarande vid: {string.Join(", ", placeradeByggnader.Keys)}");
        }
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


