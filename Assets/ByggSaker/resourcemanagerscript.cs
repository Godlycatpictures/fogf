using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class resourcemanagerscript : MonoBehaviour
{

    public SceneInfo sceneInfo; // scene info för kol o sånt

    private void Start()
    {
        // reset sceneinfo saker
        sceneInfo.energyResource = 0;
        sceneInfo.kolSomTasUpp = 5;
        Debug.Log("du borde ha 0 koll stämmer det då du har: " + sceneInfo.energyResource + " st kol");
        Debug.Log("och kol som tas upp bprde vara 5, just nu tar du upp: " + sceneInfo.energyResource + " st kol");

    }
}
