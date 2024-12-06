using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class Gatherer : MonoBehaviour // har blivit interakt men startades som gatherer så därav behöll det gamla namnet
{
    public popupscript popupscript;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("klick");

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            List<string> Resources = new List<string> { "stone", "coal", "tree"};
            List<string> Buildings = new List<string> { "lampa","generator","extractor","turret"};
            
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (Resources.Contains(hit.collider.tag))
                {
                    popupscript.OpenInteractable("gather"); // öppna menun för gather
                } else if (Buildings.Contains(hit.collider.tag))
                {
                    popupscript.OpenBuilding("byggnad"); // öppna menun för byggnader
                }
            }
        }
    }

    public void GatherResource()
    {

    }
}
