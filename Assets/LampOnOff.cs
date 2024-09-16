using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampOnOff : MonoBehaviour
{
    public GameObject maskObject; 

   
    public void HideMask()
    {
        maskObject.SetActive(false);
    }

   
    public void ShowMask()
    {
        maskObject.SetActive(true); 
    }
}

