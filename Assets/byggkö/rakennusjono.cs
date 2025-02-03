using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rakennusjono : MonoBehaviour
{

    public ByggPlacerare byggPlacerare; // få tillgång till vilken byggnad
    private Queue<GameObject> jonoRakennukseille = new Queue<GameObject>(); // Kö för byggnader
    public float byggDelay = 2f; // Fördröjning i sekunder mellan byggnader
    private bool bygger = false; // För att undvika flera coroutine samtidigt

    
    public void villPlaceraDennaByggnad(GameObject byggnadPrefab)
    {
        jonoRakennukseille.Enqueue(byggnadPrefab);
        if (!bygger)
        {
            StartCoroutine(PlaceraByggnader());
        }
    }

    /// ta bort delay
    /// gör transparent när placerad
    /// byggare ska gå till byggnad, sen bygga det
    /// inte längre transparent
    /// gå till nästa byggnad

    // Coroutine för att placera byggnader
    private IEnumerator PlaceraByggnader()
    {
        bygger = true;

        while (jonoRakennukseille.Count > 0)
        {
            GameObject byggnadPrefab = jonoRakennukseille.Peek(); // Kollar första byggnaden i kön utan att ta bort den

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = byggPlacerare.grid.WorldToCell(mouseWorldPos);
            gridPosition.z = -8; // Sätt rätt höjd så att alla byggnader hamnar på samma nivå

            if (byggPlacerare.finnsByggnadVidPlatsen(gridPosition))
            {
                Debug.Log("Kan ej placera byggnad här, plats upptagen.");
                jonoRakennukseille.Dequeue(); 
                continue; 
            }

            if (byggPlacerare.harResurserna(byggnadPrefab))
            {
                jonoRakennukseille.Dequeue(); 
                byggPlacerare.taBortResurserna(byggnadPrefab);
                byggPlacerare.placeraByggnaden(byggnadPrefab, gridPosition);
            }
            else
            {
                Debug.Log("Resurser tog slut innan byggnaden kunde placeras!");
                break; // Stoppa byggkön tills spelaren får mer resurser
            }

            yield return new WaitForSeconds(byggDelay);
        }

        bygger = false;
    }

}
