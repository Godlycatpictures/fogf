using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampOnOff : MonoBehaviour
{
    public GameObject lightObject;

    private void Start()
    {
        ShowLight(); // ska vara hidelight() men har inte fixat kolsaken och orka tbh
    }
    public void HideLight()
    {
        lightObject.SetActive(false);
    }

   
    public void ShowLight()
    {
        lightObject.SetActive(true); 
    }
}

