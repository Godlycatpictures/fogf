using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class Gatherer : MonoBehaviour // har blivit interakt men startades som gatherer så därav behöll det gamla namnet
{
    public popupscript popupscript;
    public GameObject UI;

    List<string> Resources = new List<string> { "stone", "coal", "tree" };
    List<string> Buildings = new List<string> { "lampa", "generator", "extractor", "turret" };

    private GameObject selectedObject;

    public SceneInfo sceneInfo; // scene info för kol o sånt

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!UI.activeSelf) // kan ej använda ifall UI är aktivt
        {
            if (Input.GetMouseButtonDown(0))
            {
                vasen();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            UI.SetActive(false);
        }

    }

    public void vasen()
    {
        Debug.Log("klick");

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log("Träffade objekt: " + hit.collider.name + ", Tag: " + hit.collider.tag);

            if (Resources.Contains(hit.collider.tag))
            {
                popupscript.OpenInteractable("gather " + hit.collider.tag);
                selectedObject = hit.collider.gameObject; // Skicka objektet
            }
            else if (Buildings.Contains(hit.collider.tag))
            {
                popupscript.OpenBuilding("byggnad"); // öppna menun för byggnader
            }
        }
    }

    public void GatherResource()
    {
        if (selectedObject != null && Resources.Contains(selectedObject.tag))
        {
            if (selectedObject.tag.Equals("coal"))
            {
                Debug.Log("(energy resource) + 1");
                sceneInfo.energyResource += 10;
                Destroy(selectedObject);
                UI.SetActive(false);
            } 
            else if ((selectedObject.tag.Equals("tree") || selectedObject.tag.Equals("stone")))
            {
                Debug.Log("(building resource) + 1");
                sceneInfo.buildingResource += 10;
                Destroy(selectedObject);
                UI.SetActive(false);
            }
        }
        else
        {
            Debug.Log("No valid resource selected.");
        }
    }
}
