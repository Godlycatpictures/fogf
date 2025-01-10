using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class betterpopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public Button button1;
    public Button button2;

    private RectTransform popupRect;

    void Awake()
    {
        popupRect = GetComponent<RectTransform>();
        HidePopup(); // Se till att popupen är gömd vid start
    }

    public void ShowPopup(Vector2 mousePosition, string text, string button1Label, string button2Label)
    {
        popupRect.position = mousePosition; // fixar popupens position
        popupText.text = text; // text (gather tag)
        button1Text.text = button1Label; // knapptext
        button2Text.text = button2Label; // knapptext 2

        gameObject.SetActive(true); // Aktivera popupen
    }

    public void HidePopup()
    {
        gameObject.SetActive(false); // Dölj popupen
    }

    public bool IsActive()
    {
        return gameObject.activeSelf; // Returnerar om popupen är aktiv
    }

    private void Start()
    {
        button1.onClick.AddListener(() => Debug.Log("Button 1 Clicked"));
        button2.onClick.AddListener(HidePopup);
    }
}
