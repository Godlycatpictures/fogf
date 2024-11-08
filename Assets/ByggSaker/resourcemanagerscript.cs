using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace coal
{
    public class resourcemanagerscript : MonoBehaviour
    {

        public SceneInfo sceneInfo; // scene info för kol o sånt

        private void Start()
        {
            sceneInfo.energyResource = 0;
            Debug.Log("du borde ha 0 koll stämmer det då du har: " + sceneInfo.energyResource + " st kol");
        }
    }
}
