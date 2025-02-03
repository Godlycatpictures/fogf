using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour

{
    public SceneInfo sceneInfo;
    public Slider slider; // Reference to the UI Slider
    
    void Start()
    {
        // Ensure the slider starts with the correct value
        slider.value = sceneInfo.TimeScale;

        // Add listener for when the slider value changes
        slider.onValueChanged.AddListener(UpdateVariable);
    }

    void UpdateVariable(float value)
    {
        sceneInfo.TimeScale = value;
        Debug.Log("Variable updated: " + sceneInfo.TimeScale);
    }
}

