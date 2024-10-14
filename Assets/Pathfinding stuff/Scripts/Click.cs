using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    private GameObject selectedUnit; 
    private Unit1 selectedUnitScript;  

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedUnit != null)
        {
            MoveSelectedUnitToClick();
        }
    }

    public void AssignSelectedUnit(GameObject unit)
    {
        selectedUnit = unit; 

        selectedUnitScript = selectedUnit.GetComponent<Unit1>();
    }

    void MoveSelectedUnitToClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Debug.Log("Moving selected unit to: " + mousePosition);

        if (selectedUnitScript != null)
        {
            selectedUnitScript.SetTarget(mousePosition);
        }
    }
}
