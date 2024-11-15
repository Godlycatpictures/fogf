using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lampalogiken : MonoBehaviour
{

    public bool harLedning = false; // ifall den är nära npnting nära en generator
    public float lampRange = 5f; // med den här radien ish
    public bool placerad = false;



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
        if (placerad)
        {
            tittaLedning();
            if (harLedning)
            {
                drainingCoal();
            }

        }
    }
    void drainingCoal()
    {

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
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, lampRange);

        foreach (var obj in nearbyObjects)
        {
            if (obj.CompareTag("generator") || obj.CompareTag("extractor"))
            {
                harLedning = true;
                Debug.Log("finns en generator/extractor för ledning!");
                break;
            }
            else
            {
                harLedning = false;
                Debug.Log("ingen ledning från generator/extractor");
                break;
            }
        }

        //  om ej en generator eller extractor, lampa?
        if (!harLedning)
        {
            tittaLedningLampa();
        }
    }

    private void tittaLedningLampa()
    {
        Collider2D[] nearbyLamps = Physics2D.OverlapCircleAll(transform.position, lampRange); // tittar innanför innanför range

        foreach (var obj in nearbyLamps)
        {
            lampalogiken nearbyLamp = obj.GetComponent<lampalogiken>();
            if (nearbyLamp != null && nearbyLamp.harLedning)
            {
                harLedning = true;
                Debug.Log("Lampan fick koppling via en annan lampa!");
                break; 
            } else
            {
                harLedning = false;
                Debug.Log("ingen ledning från lampa");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // visa range i editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lampRange);
    }

}
