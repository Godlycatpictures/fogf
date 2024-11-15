using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lampalogiken : MonoBehaviour
{

    public bool harLedning = false; // ifall den �r n�ra npnting n�ra en generator
    public float lampRange = 5f; // med den h�r radien ish
    public bool placerad = false;



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

    private void ByggnaderHarUppdaterats(Vector3 buildingPosition) // m�ste checka sakerna igen
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
                Debug.Log("finns en generator/extractor f�r ledning!");
                break;
            }
            else
            {
                harLedning = false;
                Debug.Log("ingen ledning fr�n generator/extractor");
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
        Collider2D[] nearbyLamps = Physics2D.OverlapCircleAll(transform.position, lampRange); // tittar innanf�r innanf�r range

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
                Debug.Log("ingen ledning fr�n lampa");
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
