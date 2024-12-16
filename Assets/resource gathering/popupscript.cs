using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popupscript : MonoBehaviour
{
    // cred till https://discussions.unity.com/t/open-a-popup-window-with-text-and-images-when-clicking-on-an-object-please-help/184303
    // bara f�r originella koden �r ganska modifierad
    public GameObject ui;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenInteractable(string message)
    {
        
        if (!string.IsNullOrEmpty(message))
        {
            Text textObject = ui.gameObject.GetComponentInChildren<Text>(); // hade problem med textmeshpro (�ndrades till text legacy och fungerar nu)
            textObject.text = message;
        }

        bool isActive = ui.activeSelf;
        ui.SetActive(!isActive);

      /*  if (ui.activeSelf)
        {
            Time.timeScale = 0f; // freeze
        }
        else
        {
            Time.timeScale = 1f; // unfreeze
        }*/
    }
    public void OpenBuilding(string message)
    {

        if (!string.IsNullOrEmpty(message))
        {
            Text textObject = ui.gameObject.GetComponentInChildren<Text>(); // hade problem med textmeshpro (�ndrades till text legacy och fungerar nu)
            textObject.text = message;
        }

        bool isActive = ui.activeSelf;
        ui.SetActive(!isActive);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f; // freeze
        }
        else
        {
            Time.timeScale = 1f; // unfreeze
        }
    }

    public void Close()
    {
        ui.SetActive(!ui.activeSelf);
        if (!ui.activeSelf)
        {
            Time.timeScale = 1f; // unfreeze
        }
    }
    
}