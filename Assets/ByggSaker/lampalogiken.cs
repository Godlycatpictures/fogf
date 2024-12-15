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
    public float UppslukningsTid = 30f;

    public SceneInfo sceneInfo; // scene info f�r kol o s�nt

    private void OnEnable()
    {
        ByggPlacerare.ByggnadPlaceradEvent += ByggnaderHarUppdaterats;
        rakennusHajoittaja.ByggnadBorttagenEvent += ByggnaderHarUppdaterats;
    }

    private void OnDisable()
    {
        ByggPlacerare.ByggnadPlaceradEvent -= ByggnaderHarUppdaterats;
        rakennusHajoittaja.ByggnadBorttagenEvent -= ByggnaderHarUppdaterats;
    }


    private void Start()
    {
        tittaLedning();
    }
    private void Update()
    {
        if (harLedning && placerad)
        {
            // h�r skicka s�ken s� att lampan �r p� till relaterad skript

            UppslukningsTid -= Time.deltaTime;
            if (UppslukningsTid <= 0)
            {
                /// tar bort x kol beroende p� kolUppslukaren (som generatorn kan g�re mer effektiv)
                /// t�nker att den skulle kunna antingen �ndra p� tiden eller andel kol den tar varje uppslukningstid
                UppslukningsTid = 30;
                sceneInfo.energyResource -= kolUppslukare;
                Debug.Log(kolUppslukare + " kol anv�ndts");

            }
        }
    }

    private void ByggnaderHarUppdaterats(Vector3 buildingPosition) // m�ste checka sakerna igen
    {
        if (Vector3.Distance(transform.position, buildingPosition) <= lampRange)
        {
            tittaLedning(); 
        }
    }

    private void tittaLedning()
    {
        bool kopplingFinns = false; // temp f�r att inte nolst�lla update

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

        // ingen extractorgenerator betyder leta lampor
        if (!kopplingFinns)
        {
            HashSet<lampalogiken> visitedLamps = new HashSet<lampalogiken>();
            kopplingFinns = KontrolleraLedningViaLampor(this, visitedLamps);
        }

        if (kopplingFinns)
        {
            harLedning = true;
        }
        else
        {
            harLedning = false;
        }
    }

    private bool KontrolleraLedningViaLampor(lampalogiken startLampa, HashSet<lampalogiken> bes�ktaLampor)
    {
        // om jag har ledning, beh�ver jag inte checka f� ledning
        if (startLampa.harLedning)
        {
            return true;
        }

        // redan chackad check
        bes�ktaLampor.Add(startLampa);

        // cirkel
        Collider2D[] nearbyLamps = Physics2D.OverlapCircleAll(startLampa.transform.position, lampRange);
        foreach (var obj in nearbyLamps)
        {
            lampalogiken nearbyLamp = obj.GetComponent<lampalogiken>();

            if (nearbyLamp != null && !bes�ktaLampor.Contains(nearbyLamp))
            {
                if (nearbyLamp.harLedning)
                {
                    // om en lampa har koppling har jag koppling :D
                    return true;
                }
                else
                {
                    // titta efter andra lampor
                    if (KontrolleraLedningViaLampor(nearbyLamp, bes�ktaLampor))
                    {
                        return true;
                    }
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
