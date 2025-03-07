using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    private List<GameObject> selectedUnits = new List<GameObject>();
    private List<Unit> selectedUnitScripts = new List<Unit>(); // Fixed to use correct class

    void Update()
    {
        // Change to right-click for movement
        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
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
            Unit unitScript = unit.GetComponent<Unit>(); // Fixed class name
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

            foreach (Unit unitScript in selectedUnitScripts)
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
