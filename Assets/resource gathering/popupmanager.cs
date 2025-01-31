using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{

    // för popup
    public betterpopup popupController;
    private Camera mainCamera;
    public SceneInfo sceneInfo;

    // för resource
    List<string> Resources = new List<string> { "stone", "coal", "tree" };
    List<string> Buildings = new List<string> { "generator", "extractor", "lampa"};
    private GameObject selectedObject;

    private int byggnaderPreviewLayer;
    void Start()
    {
        mainCamera = Camera.main;
        byggnaderPreviewLayer = LayerMask.NameToLayer("ByggnaderPreview"); // den ska ignorera previewn så att man kan placera ut (annar gatherar preview)

    }

    void Update()
    {
        if (!popupController.IsActive())
        {
            if (Input.GetMouseButtonDown(0)) // vansterlkick
            {
                vasen();
            }
        }
    }

    private void vasen()
    {
        Vector2 mousePosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {

            int hitLayer = hit.collider.gameObject.layer;
            if (hitLayer == byggnaderPreviewLayer)
            {
                Debug.Log("inte placerad byggnad");
                return; // ignorerar
            }

            Debug.Log("Träffade objekt: " + hit.collider.name + ", Tag: " + hit.collider.tag);

            if (Resources.Contains(hit.collider.tag))
            {
                Vector2 mousescreenposition = Input.mousePosition;
                string popupText = $"Gather {hit.collider.tag}";
                popupController.ShowPopup(mousePosition, popupText, "Gather", "Cancel");
                selectedObject = hit.collider.gameObject; // Skicka objektet
            } else if (Buildings.Contains(hit.collider.tag))
            {
                Vector2 mousescreenposition = Input.mousePosition;
                string popupText = $"Destroy {hit.collider.tag}";
                popupController.ShowPopup(mousePosition, popupText, "Confirm", "Cancel");
                selectedObject = hit.collider.gameObject; // Skicka objektet
            }
        }
    }

    public void GatherResource() // ändrar värden beroende på tag
    {
        if (selectedObject != null && Resources.Contains(selectedObject.tag))
        {
            if (selectedObject.tag.Equals("coal"))
            {
                Debug.Log("(energy resource) + 1");
                sceneInfo.energyResource += 10;
                Destroy(selectedObject);
                popupController.HidePopup();
            }
            else if ((selectedObject.tag.Equals("tree") || selectedObject.tag.Equals("stone")))
            {
                Debug.Log("(building resource) + 1");
                sceneInfo.buildingResource += 10;
                Destroy(selectedObject);
                popupController.HidePopup();
            }
        }
        else if (selectedObject != null && Buildings.Contains(selectedObject.tag))
        {
            if (selectedObject.tag.Equals("extractor"))
            {
                sceneInfo.energyResource += 10;
                sceneInfo.buildingResource += 10;

                Destroy(selectedObject);
                popupController.HidePopup();
            }
            else if (selectedObject.tag.Equals("lampa"))
            {
                sceneInfo.buildingResource += 10;

                Destroy(selectedObject);
                popupController.HidePopup();
            }
            else if (selectedObject.tag.Equals("generator"))
            {
                sceneInfo.buildingResource += 15;
                sceneInfo.kolSomTasUpp -= 5;

                Destroy(selectedObject);
                popupController.HidePopup();
            }
        }
        else
        {
            Debug.Log("No valid resource selected.");
        }
    }

    public void CancelResource() // var i början enbart för resurser därav namnet cancelRESOURCE ;)
    {
        popupController.HidePopup();
    }


}
