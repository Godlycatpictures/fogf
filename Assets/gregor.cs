using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneInfoDisplay : MonoBehaviour
{
    // Referens till TextMeshPro för att visa resurser
    public TextMeshProUGUI buildResourceText; // Koppla detta i Inspector
    public TextMeshProUGUI energyResourceText; // Koppla detta i Inspector

    // Referens till SceneInfo-objektet
    public SceneInfo sceneInfo; // Koppla detta i Inspector

    void Update()
    {
        if (sceneInfo == null) return; // Kontrollera att sceneInfo finns

        // Uppdatera texten baserat på variablerna i sceneInfo
        buildResourceText.text = "Build Resources: " + sceneInfo.buildingResource;
        energyResourceText.text = "Energy: " + sceneInfo.energyResource;
    }
}
