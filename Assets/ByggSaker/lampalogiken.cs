using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lampalogiken : MonoBehaviour
{

    public bool harLedning = false; // ifall den �r n�ra npnting n�ra en generator
    public float lampRange = 5f; // med den h�r radien ish
    public bool placerad = false;
    public int kolUppslukare = 5;
    public float UppslukningsTid = 60f;
    public LampOnOff lampOnOff;

    public SceneInfo sceneInfo; // scene info f�r kol o s�nt

    private void OnEnable()
    {
        ByggPlacerare.ByggnadPlaceradEvent += ByggnaderHarUppdaterats;
        PopupManager.ByggnadBorttagenEvent += ByggnaderHarUppdaterats;
    }

    private void OnDisable()
    {
        ByggPlacerare.ByggnadPlaceradEvent -= ByggnaderHarUppdaterats;
        PopupManager.ByggnadBorttagenEvent -= ByggnaderHarUppdaterats;
    }


    private void Start()
    {
        tittaLedning();
        lampOnOff.HideLight();
    }
    private void Update()
    {

        if (harLedning)
        {
            lampOnOff.ShowLight();
        } else
        {
            lampOnOff.HideLight();
        }

        if (harLedning && placerad)
        {
            // h�r skicka saken s� att lampan �r p� till relaterad skript


            UppslukningsTid -= Time.deltaTime;
            if (UppslukningsTid <= 0)
            {
                /// tar bort x kol beroende p� kolUppslukaren (som generatorn kan g�re mer effektiv)
                /// t�nker att den skulle kunna antingen �ndra p� tiden eller andel kol den tar varje uppslukningstid
                UppslukningsTid = 60;
                sceneInfo.energyResource -= kolUppslukare;
                Debug.Log(kolUppslukare + " kol anv�ndts");

            }
        }
    }

    private void ByggnaderHarUppdaterats(Vector3 buildingPosition)
    {
        UppdateraAllaLampor(); // Ist�llet f�r att bara uppdatera enskilda
    }

    private void tittaLedning()
    {
        harLedning = false; // �terst�ll s� vi inte har gamla felaktiga v�rden

        bool kopplingFinns = false;

        // Kolla om en generator eller extractor finns i n�rheten
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, lampRange);
        foreach (var obj in nearbyObjects)
        {
            if (obj.CompareTag("generator") || obj.CompareTag("extractor"))
            {
                kopplingFinns = true;
                break;
            }
        }

        if (!kopplingFinns)
        {
            HashSet<lampalogiken> visitedLamps = new HashSet<lampalogiken>();
            kopplingFinns = KontrolleraLedningViaLampor(this, visitedLamps);
        }

        harLedning = kopplingFinns;
    }


    private bool KontrolleraLedningViaLampor(lampalogiken startLampa, HashSet<lampalogiken> bes�ktaLampor)
    {
        if (bes�ktaLampor.Contains(startLampa)) return false;

        bes�ktaLampor.Add(startLampa);

        Collider2D[] nearbyLamps = Physics2D.OverlapCircleAll(startLampa.transform.position, lampRange);
        foreach (var obj in nearbyLamps)
        {
            lampalogiken nearbyLamp = obj.GetComponent<lampalogiken>();

            if (nearbyLamp != null && !bes�ktaLampor.Contains(nearbyLamp))
            {
                if (nearbyLamp.harLedning)
                {
                    return true;
                }
                else if (KontrolleraLedningViaLampor(nearbyLamp, bes�ktaLampor))
                {
                    return true;
                }
            }
        }

        return false;
    }
    public static void UppdateraAllaLampor()
    {
        lampalogiken[] allaLampor = FindObjectsOfType<lampalogiken>();
        foreach (var lampa in allaLampor)
        {
            lampa.tittaLedning(); // Uppdatera varje lampas koppling
        }
    }

    private void OnDrawGizmosSelected()
    {
        // visa range i editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lampRange);
    }

}
