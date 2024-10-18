using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extractorlogik : MonoBehaviour
{
    public bool exracting = false;
    public float tidsPress = 10f;
    public float kolkolkolkolkol = 5;
    public resourcemanagerscript imJustHereForCoal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (exracting)
        {
            tidsPress -= Time.deltaTime;

            if (tidsPress <= 0) // ger 5 kol per 10 sec
            {
                extractorReactor();
                tidsPress = 10;
                
            }
        } 
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collision");
        if (other.gameObject.CompareTag("coal")) // 
        {
            // start exractorreactor
            Debug.Log("kolkolkolkol");
            exracting = true;
        }
    }

    private void extractorReactor()
    {
        imJustHereForCoal.totalKolManHar += kolkolkolkolkol; // kol plus är lika med kol
        Debug.Log("du har nu" + imJustHereForCoal.totalKolManHar + " st. kol");
    }

}
