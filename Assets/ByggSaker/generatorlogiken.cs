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
        if (placerad == true)
        {
            sceneInfo.kolSomTasUpp += 10;
        } else
        {
            sceneInfo.kolSomTasUpp -= 10;
        }

    }

}
