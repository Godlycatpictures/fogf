using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{
    private List<GameObject> selectedUnits = new List<GameObject>();

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
                
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    
                    if (selectedUnits.Contains(hitObject))
                    {
                        DeselectUnit(hitObject);
                    }
                    else
                    {
                        AddUnitToSelection(hitObject);
                    }
                }
                else
                {
                    DeselectAllUnits();
                    AddUnitToSelection(hitObject);
                }

                ClickToMove clickToMove = FindObjectOfType<ClickToMove>();
                if (clickToMove != null)
                {
                    clickToMove.AssignSelectedUnits(selectedUnits);
                }
            }
        }
    }

    void AddUnitToSelection(GameObject unit)
    {
        selectedUnits.Add(unit);
        Highlight(unit);
        Debug.Log("Selected unit: " + unit.name);
    }

    void DeselectUnit(GameObject unit)
    {
        selectedUnits.Remove(unit);
        RemoveHighlight(unit);
        Debug.Log("Deselected unit: " + unit.name);
    }

    void DeselectAllUnits()
    {
        foreach (GameObject unit in selectedUnits)
        {
            RemoveHighlight(unit);
        }
        selectedUnits.Clear();
        Debug.Log("Deselected all units");
    }

    void Highlight(GameObject unit)
    {
        SpriteRenderer sr = unit.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.green;
        }
    }

    void RemoveHighlight(GameObject unit)
    {
        SpriteRenderer sr = unit.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.blue;
        }
    }
}
