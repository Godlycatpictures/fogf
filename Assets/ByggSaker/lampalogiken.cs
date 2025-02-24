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
    public float UppslukningsTid = 60f;
    public LampOnOff lampOnOff;

    public SceneInfo sceneInfo; // scene info för kol o sånt

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
            // här skicka saken så att lampan är på till relaterad skript


            UppslukningsTid -= Time.deltaTime;
            if (UppslukningsTid <= 0)
            {
                /// tar bort x kol beroende på kolUppslukaren (som generatorn kan göre mer effektiv)
                /// tänker att den skulle kunna antingen ändra på tiden eller andel kol den tar varje uppslukningstid
                UppslukningsTid = 60;
                sceneInfo.energyResource -= kolUppslukare;
                Debug.Log(kolUppslukare + " kol användts");

            }
        }
    }

    private void ByggnaderHarUppdaterats(Vector3 buildingPosition)
    {
        UppdateraAllaLampor(); // Istället för att bara uppdatera enskilda
    }

    private void tittaLedning()
    {
        harLedning = false; // Återställ så vi inte har gamla felaktiga värden

        bool kopplingFinns = false;

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

        if (!kopplingFinns)
        {
            HashSet<lampalogiken> visitedLamps = new HashSet<lampalogiken>();
            kopplingFinns = KontrolleraLedningViaLampor(this, visitedLamps);
        }

        harLedning = kopplingFinns;
    }


    private bool KontrolleraLedningViaLampor(lampalogiken startLampa, HashSet<lampalogiken> besöktaLampor)
    {
        if (besöktaLampor.Contains(startLampa)) return false;

        besöktaLampor.Add(startLampa);

        Collider2D[] nearbyLamps = Physics2D.OverlapCircleAll(startLampa.transform.position, lampRange);
        foreach (var obj in nearbyLamps)
        {
            lampalogiken nearbyLamp = obj.GetComponent<lampalogiken>();

            if (nearbyLamp != null && !besöktaLampor.Contains(nearbyLamp))
            {
                if (nearbyLamp.harLedning)
                {
                    return true;
                }
                else if (KontrolleraLedningViaLampor(nearbyLamp, besöktaLampor))
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
