using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{
    private GameObject selectedUnit;
    void Start()    
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectUnit();
        }

    }

    void SelectUnit()
    {
        //kollar vart musen är på skärmen och skcikar input när man clickar.
        Vector2 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //skickar ut en raycast från där man klickar.
        RaycastHit2D hit = Physics2D.Raycast(mouseposition, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Unit"))
            {
                selectedUnit = hitObject;
                Highlight(selectedUnit);
                Debug.Log("Selected unit: " + selectedUnit.name);
               
            }
        }
    }

    void DeselectUnit()
    {

    } 
    
    void Highlight(GameObject unit)
    {
        SpriteRenderer sr = unit.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.color = Color.green;
        }
    }
}
