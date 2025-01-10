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
        popupRect.position = mousePosition;
        popupText.text = text;
        button1Text.text = button1Label;
        button2Text.text = button2Label;

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
