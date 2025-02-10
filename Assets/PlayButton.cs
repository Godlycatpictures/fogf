using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public string sceneName; // Set this in the Inspector to the name of the scene you want to load

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}

