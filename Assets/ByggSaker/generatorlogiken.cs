using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatorlogiken : MonoBehaviour
{
    public bool placerad = false;
    
    public SceneInfo sceneInfo; // scene info f�r kol o s�nt

    // Update is called once per frame
    void Update()
    {
        if (placerad)
        {
            for (int i = 1; i < 2; i++)
            {
                sceneInfo.kolSomTasUpp += 5;
            }return;
        }
    }
}
