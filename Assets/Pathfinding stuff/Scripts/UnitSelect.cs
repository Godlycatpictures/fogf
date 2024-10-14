using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{
    private GameObject selectedUnit;  

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectOrDeselectUnit();
        }
    }

    void SelectOrDeselectUnit()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Unit"))
            {
                if (hitObject == selectedUnit)
                {
                    DeselectUnit();  
                }
                else
                {
                  
                    if (selectedUnit != null)
                    {
                        DeselectUnit();
                    }

                    selectedUnit = hitObject;
                    Highlight(selectedUnit);  

                    Debug.Log("Selected unit: " + selectedUnit.name);

                    ClickToMove clickToMove = FindObjectOfType<ClickToMove>();
                    if (clickToMove != null)
                    {
                        clickToMove.AssignSelectedUnit(selectedUnit);
                    }
                }
            }
        }
    }

    void DeselectUnit()
    {
        if (selectedUnit != null)
        {
            SpriteRenderer sr = selectedUnit.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.blue; 
            }

            Debug.Log("Deselected unit: " + selectedUnit.name);

            selectedUnit = null;

            ClickToMove clickToMove = FindObjectOfType<ClickToMove>();
            if (clickToMove != null)
            {
                clickToMove.AssignSelectedUnit(null);  
            }
        }
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
