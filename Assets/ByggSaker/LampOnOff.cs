using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampOnOff : MonoBehaviour
{
    public GameObject lightObject;

    private void Start()
    {
        ShowLight();
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

