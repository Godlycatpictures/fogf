using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class ByggValet : MonoBehaviour
{
    public GameObject[] prefabBuildings;
    public GameObject aktivByggnad;
    public GameObject Bobby;
    public GameObject Borttagare;
    public SpookyBuilding deleteremovebanish;

    private void Start()
    {
        
        setAktivByggnad(0);
    }

    public void setAktivByggnad(int index)
    {
        if (index >= 0 && index < prefabBuildings.Length)
        {
            aktivByggnad = prefabBuildings[index]; // id av byggnaden (i en array)
            Debug.Log("Aktiva byggnaden: " + aktivByggnad.name);
            Bobby.SetActive(true); // gör byggare bob tillgänglig
            Borttagare.SetActive(false);
        }
    }

    public void taBortTagareAvByggnader()
    {
        deleteremovebanish.hejhejpreview();
        Bobby.SetActive(false);
        Borttagare.SetActive(true);

    }
    

    public GameObject getAktivByggnad()
    {
        return aktivByggnad;
        
    }

}
