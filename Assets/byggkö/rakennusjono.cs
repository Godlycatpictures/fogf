using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rakennusjono : MonoBehaviour
{

    public SceneInfo sceneInfo;
    public ByggPlacerare byggPlacerare; // f� tillg�ng till vilken byggnad
    private Queue<GameObject> jonoRakennukseille = new Queue<GameObject>(); // K� f�r byggnader
    public float byggDelay = 2f; // F�rdr�jning i sekunder mellan byggnader
    private bool bygger = false; // F�r att undvika flera coroutine samtidigt
    public Transform byggarn;
    public buildermove buildermove; // Referens till byggarens rörelsehantering

    void Start()
    {
        byggPlacerare = FindObjectOfType<ByggPlacerare>();
        buildermove = byggarn.GetComponent<buildermove>();
    }

    public void villPlaceraDennaByggnad(GameObject byggnadPrefab)
    {
        jonoRakennukseille.Enqueue(byggnadPrefab);
        if (!bygger)
        {
            StartCoroutine(PlaceraByggnader());
        }
    }

    private IEnumerator PlaceraByggnader()
    {
        bygger = true;

        while (jonoRakennukseille.Count > 0)
        {
            GameObject byggnadPrefab = jonoRakennukseille.Dequeue();

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = byggPlacerare.grid.WorldToCell(mouseWorldPos);
            gridPosition.z = -8;

            if (byggPlacerare.finnsByggnadVidPlatsen(gridPosition))
            {
                Debug.Log("Kan ej placera byggnad här, plats upptagen.");
                continue;
            }

            if (byggPlacerare.harResurserna(byggnadPrefab))
            {
                byggPlacerare.taBortResurserna(byggnadPrefab);

                // Spara lista över byggnader innan placering
                GameObject[] byggnaderInnan = GameObject.FindGameObjectsWithTag("Byggnad");

                // Placera byggnaden (returnerar inget)
                byggPlacerare.placeraByggnaden(byggnadPrefab, gridPosition);

                // Vänta en frame så att Unity hinner uppdatera scenen
                yield return null;

                // Hitta den nya byggnaden genom att jämföra listan
                GameObject byggnad = HittaNyByggnad(byggnaderInnan);

                if (byggnad != null)
                {
                    SetTransparency(byggnad, 0.5f);

                    // Flytta byggaren till byggnaden och vänta tills den är framme
                    yield return buildermove.FlyttaTillPosition(byggnad.transform.position);

                    // När byggaren är framme, gör byggnaden synlig
                    SetTransparency(byggnad, 1.0f);
                }
                else
                {
                    Debug.LogWarning("Kunde inte hitta den nya byggnaden!");
                }
            }
            else
            {
                Debug.Log("Resurser tog slut innan byggnaden kunde placeras!");
                break;
            }
        }

        bygger = false;
    }

    // Hjälpfunktion för att hitta den nya byggnaden
    private GameObject HittaNyByggnad(GameObject[] gamlaByggnader)
    {
        GameObject[] allaByggnader = GameObject.FindGameObjectsWithTag("Byggnad");

        foreach (GameObject byggnad in allaByggnader)
        {
            bool fannsInnan = false;
            foreach (GameObject gammal in gamlaByggnader)
            {
                if (byggnad == gammal)
                {
                    fannsInnan = true;
                    break;
                }
            }

            if (!fannsInnan)
            {
                return byggnad; // Ny byggnad hittad
            }
        }

        return null;
    }

    private void SetTransparency(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;
                mat.SetFloat("_Mode", alpha < 1.0f ? 3 : 0);
            }
        }
    }

}
