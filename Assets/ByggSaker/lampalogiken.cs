using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lampalogiken : MonoBehaviour
{

    public bool harLedning = false; // ifall den är nära npnting nära en generator
    public float lampRange = 5f; // med den här radien ish
    public bool placerad = false;
    public int kolUppslukare = 5;
    public float UppslukningsTid = 30f;

    public SceneInfo sceneInfo; // scene info för kol o sånt

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
            // här skicka såken så att lampan är på till relaterad skript

            UppslukningsTid -= Time.deltaTime;
            if (UppslukningsTid <= 0)
            {
                /// tar bort x kol beroende på kolUppslukaren (som generatorn kan göre mer effektiv)
                /// tänker att den skulle kunna antingen ändra på tiden eller andel kol den tar varje uppslukningstid
                UppslukningsTid = 30;
                sceneInfo.energyResource -= kolUppslukare;
                Debug.Log(kolUppslukare + " kol användts");

            }
        }
    }

    private void ByggnaderHarUppdaterats(Vector3 buildingPosition) // måste checka sakerna igen
    {
        if (Vector3.Distance(transform.position, buildingPosition) <= lampRange)
        {
            tittaLedning(); 
        }
    }

    private void tittaLedning()
    {
        bool kopplingFinns = false; // temp för att inte nolställa update

        // Kolla om en generator eller extractor finns i närheten
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

    private bool KontrolleraLedningViaLampor(lampalogiken startLampa, HashSet<lampalogiken> besöktaLampor)
    {
        // om jag har ledning, behöver jag inte checka fö ledning
        if (startLampa.harLedning)
        {
            return true;
        }

        // redan chackad check
        besöktaLampor.Add(startLampa);

        // cirkel
        Collider2D[] nearbyLamps = Physics2D.OverlapCircleAll(startLampa.transform.position, lampRange);
        foreach (var obj in nearbyLamps)
        {
            lampalogiken nearbyLamp = obj.GetComponent<lampalogiken>();

            if (nearbyLamp != null && !besöktaLampor.Contains(nearbyLamp))
            {
                if (nearbyLamp.harLedning)
                {
                    // om en lampa har koppling har jag koppling :D
                    return true;
                }
                else
                {
                    // titta efter andra lampor
                    if (KontrolleraLedningViaLampor(nearbyLamp, besöktaLampor))
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
