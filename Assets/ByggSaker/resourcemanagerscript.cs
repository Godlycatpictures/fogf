using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class resourcemanagerscript : MonoBehaviour
{

    public SceneInfo sceneInfo; // scene info f�r kol o s�nt

    private void Start()
    {
        // reset sceneinfo saker
        sceneInfo.energyResource = 0;
        sceneInfo.kolSomTasUpp = 5;
        sceneInfo.buildingResource = 0;
        Debug.Log("du borde ha 0 koll stämmer det då du har: " + sceneInfo.energyResource + " st kol");
        Debug.Log("du borde ha noll st byggmateriakl, nu har du: " + sceneInfo.buildingResource + " st byggmaterial");
        Debug.Log("och kol som tas upp bprde vara 5, just nu tar du upp: " + sceneInfo.kolSomTasUpp + " st kol");
        
    }
}
