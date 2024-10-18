using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    private List<GameObject> selectedUnits = new List<GameObject>();
    private List<Unit1> selectedUnitScripts = new List<Unit1>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedUnits.Count > 0)
        {
            MoveSelectedUnitsToClick();
        }
    }

    public void AssignSelectedUnits(List<GameObject> units)
    {
        selectedUnits = units;

        selectedUnitScripts.Clear();
        foreach (GameObject unit in selectedUnits)
        {
            Unit1 unitScript = unit.GetComponent<Unit1>();
            if (unitScript != null)
            {
                selectedUnitScripts.Add(unitScript);
            }
        }
    }

    void MoveSelectedUnitsToClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider == null)
        {
            Debug.Log("Moving selected units to: " + mousePosition);

            foreach (Unit1 unitScript in selectedUnitScripts)
            {
                if (unitScript != null)
                {
                    unitScript.SetTarget(mousePosition);
                }
            }
        }
        else
        {
            Debug.Log("Clicked on: " + hit.collider.gameObject.name + ", not moving units.");
        }
    }
}

