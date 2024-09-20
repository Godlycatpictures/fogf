using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnappKnapp : MonoBehaviour
{
    public ByggPlacerare byggPlacerare; // Drag the ByggPlacerare component here

    public void VaknaByggSakerna()
    {
        // Enable the ByggPlacerare script
        byggPlacerare.enabled = true;
    }
}
