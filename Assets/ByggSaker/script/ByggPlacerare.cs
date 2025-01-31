using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ByggPlacerare : MonoBehaviour
{
    // för byggnande kanske (kommenterat långt efter koden skapats)
    public Grid grid; // grid
    public ByggValet aktivByggnad; // vilken byggnad via byggvalet.cs
    public LayerMask Byggnader; // rätt layer
    public GameObject enKusligByggnad; // orka med två skripts previews är här från och med nu

    // för byggkön
    public rakennusjono rakennusjono; // så att den kan skickas

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
            if (aktivByggnad != null)
            {
                GameObject byggnadPrefab = aktivByggnad.getAktivByggnad();

                // resurserna tas innan den placerars
                if (harResurserna(byggnadPrefab))
                {
                    taBortResurserna(byggnadPrefab);
                    rakennusjono.villPlaceraDennaByggnad(byggnadPrefab);
                }
                else
                {
                    Debug.Log("Inte tillräckligt med resurser!");
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            BobGone();
        }
    }

    private void visaKusligaByggnaden()
    {
        if (enKusligByggnad == null || aktivByggnad == null) return;

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
        Debug.Log("körs jag? (setpreviewbuilding byggplacerare)");
        if (enKusligByggnad != null)
        {
            Destroy(enKusligByggnad);  // den ska BORT
        }

        enKusligByggnad = Instantiate(buildingPrefab, Vector3.zero, Quaternion.identity);
        enKusligByggnad.layer = LayerMask.NameToLayer("ByggnaderPreview");
    }

    public void placeraByggnaden(GameObject byggnadPrefab, Vector3Int gridPosition)
    {
        Vector3 finalPosition = grid.CellToWorld(gridPosition);
        GameObject newBuilding = Instantiate(byggnadPrefab, finalPosition, Quaternion.identity);
        newBuilding.layer = LayerMask.NameToLayer("Byggnader");

        placeradeByggnader[gridPosition] = newBuilding; // Lägg till byggnaden i dictionaryn
        Debug.Log("Byggnad placerad i " + gridPosition);

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

    public bool finnsByggnadVidPlatsen(Vector3Int gridPosition)
    {
        return placeradeByggnader.ContainsKey(gridPosition);
    }


    // resource control neråt

    public bool harResurserna(GameObject byggnad)
    {
        if (byggnad.TryGetComponent<extractorlogik>(out var extractor))
        {
            return sceneInfo.buildingResource >= 30;
        }
        if (byggnad.TryGetComponent<lampalogiken>(out var lampa))
        {
            return sceneInfo.buildingResource >= 10;
        }
        if (byggnad.TryGetComponent<generatorlogiken>(out var generator))
        {
            return sceneInfo.buildingResource >= 20 && sceneInfo.energyResource >= 50;
        }
        return true; 
    }

    public void taBortResurserna(GameObject byggnad)
    {
        if (byggnad.TryGetComponent<extractorlogik>(out var extractor))
        {
            sceneInfo.buildingResource -= 30;
        }
        if (byggnad.TryGetComponent<lampalogiken>(out var lampa))
        {
            sceneInfo.buildingResource -= 10;
        }
        if (byggnad.TryGetComponent<generatorlogiken>(out var generator))
        {
            sceneInfo.buildingResource -= 20;
            sceneInfo.energyResource -= 50;
        }
    }

}
