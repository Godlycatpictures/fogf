using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusikManager : MonoBehaviour
{
    public AudioSource låt1;
    // Start is called before the first frame update
    void Start()
    {
        låt1 = GetComponent<AudioSource>();
        låt1.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
