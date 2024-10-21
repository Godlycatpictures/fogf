using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extractorlogik : MonoBehaviour
{
    public bool exracting = false;
    public float tidsPress = 10f;
    public int kolkolkolkolkol = 5;
    public resourcemanagerscript imJustHereForCoal;
    public bool placerad = false;
    public int antalloops = 0;


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
        Debug.Log("collision");

        if (placerad && other.gameObject.CompareTag("coal")) // 
        {
                // start exractorreactor
            Debug.Log("kolkolkolkol");
            exracting = true;
        }
        
        
    }
    private void FixedUpdate()
    {
        
    }

    private void extractorReactor()
    {
        if (antalloops < 6)
        {
            antalloops++;
            imJustHereForCoal.totalKolManHar += kolkolkolkolkol; // kol plus är lika med kol
            Debug.Log("du har nu " + imJustHereForCoal.totalKolManHar + " st. kol");
        } else
        {
            exracting = false;
        }
        
    }

}
