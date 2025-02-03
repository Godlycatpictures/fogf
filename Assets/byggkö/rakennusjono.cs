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
private void update(){
byggDelay = 2f * sceneInfo.TimeScale;
}
    void Start()
    {
        byggPlacerare = FindObjectOfType<ByggPlacerare>();

}
    
    public void villPlaceraDennaByggnad(GameObject byggnadPrefab)
    {
        jonoRakennukseille.Enqueue(byggnadPrefab);
        if (!bygger)
        {
            StartCoroutine(PlaceraByggnader());
        }
    }

    /// ta bort delay
    /// g�r transparent n�r placerad
    /// byggare ska g� till byggnad, sen bygga det
    /// inte l�ngre transparent
    /// g� till n�sta byggnad

    // Coroutine f�r att placera byggnader
    private IEnumerator PlaceraByggnader()
    {
        bygger = true;

        while (jonoRakennukseille.Count > 0)
        {
            GameObject byggnadPrefab = jonoRakennukseille.Peek(); // Kollar f�rsta byggnaden i k�n utan att ta bort den

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = byggPlacerare.grid.WorldToCell(mouseWorldPos);
            gridPosition.z = -8; // S�tt r�tt h�jd s� att alla byggnader hamnar p� samma niv�

            if (byggPlacerare.finnsByggnadVidPlatsen(gridPosition))
            {
                Debug.Log("Kan ej placera byggnad h�r, plats upptagen.");
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
                break; // Stoppa byggk�n tills spelaren f�r mer resurser
            }

            yield return new WaitForSeconds(byggDelay);
        }

        bygger = false;
    }

}
