using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI; // Huvudpanelen för pausmenyn
    [SerializeField] private GameObject spelKnappar; 
    [SerializeField] private ByggValet byggValet;
     public ByggPlacerare bob;
// UI-panel för spelknappar
    private bool isPaused = false;

    void Update()
    {
        // Kontrollera om ESC trycks
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Dölj pausmenyn
        spelKnappar.SetActive(true); // Visa spelknappar
        Time.timeScale = 1f; // Återuppta spelets tid
        isPaused = false; // Uppdatera tillstånd
    }

    public void Pause()
{
        if (bob != null)
    {
        bob.BobGone();
    }
    else
    {
        Debug.LogWarning("Bob (ByggPlacerare) is not assigned!");
    }
    pauseMenuUI.SetActive(true); // Visa pausmenyn
    spelKnappar.SetActive(false); // Dölj spelknappar
    Time.timeScale = 0f; // Pausa spelets tid
    isPaused = true; // Uppdatera tillstånd

    if (byggValet != null)
    {
        byggValet.taBortTagareAvByggnader(); // Anropa metoden
    }
}

    public void QuitGame()
    {
        Debug.Log("Avslutar spelet...");
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
