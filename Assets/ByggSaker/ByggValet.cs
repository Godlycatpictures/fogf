using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByggValet : MonoBehaviour
{
    public GameObject[] prefabBuildings;
    public GameObject aktivByggnad;
    public GameObject Bobby;

    private void Start()
    {
        
        setAktivByggnad(0);
    }

    public void setAktivByggnad(int index)
    {
        if (index >= 0 && index < prefabBuildings.Length)
        {
            aktivByggnad = prefabBuildings[index];
            Debug.Log("Aktiva byggnaden: " + aktivByggnad.name);
            Bobby.SetActive(true); // gör byggare bob tillgänglig
        }
    }

    public GameObject getAktivByggnad()
    {
        return aktivByggnad;
        
    }

}
