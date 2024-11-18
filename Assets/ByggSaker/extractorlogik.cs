using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class extractorlogik : MonoBehaviour
{
    public bool exracting = false;
    public float tidsPress = 10f;
    public int kolkolkolkolkol = 5;
    public bool placerad = false;
    public int antalloops = 0;

    public SceneInfo sceneInfo; // scene info för kol o sånt

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (exracting && placerad)
        {
            tidsPress -= Time.deltaTime;

            if (tidsPress <= 0) // ger 5 kol per 10 sec
            {
                extractorReactor();
                tidsPress = 10f;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (placerad && other.gameObject.CompareTag("coal")) // 
        {
            // start exractorreactor
            Debug.Log("kolkolkolkol");
            exracting = true;
        }
    }

    private void extractorReactor()
    {
        if (antalloops < 6)
        {
            antalloops++;
            sceneInfo.energyResource += kolkolkolkolkol; // kol plus är lika med kol
            Debug.Log("du har nu " + sceneInfo.energyResource + " st. kol");
        }
        else
        {
            exracting = false;
        }
    }
}


