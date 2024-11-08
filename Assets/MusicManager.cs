using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance = null;  // Singleton instance

    public AudioSource audioSource;  // Audio source component
    public AudioClip[] backgroundTracks;  // Array of background music tracks
    private int currentTrackIndex = 0;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Keep music playing across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate music manager
        }
    }
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
