using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace coal
{
    public class resourcemanagerscript : MonoBehaviour
    {

        public SceneInfo sceneInfo; // scene info f�r kol o s�nt

        private void Start()
        {
            sceneInfo.energyResource = 0;
            Debug.Log("du borde ha 0 koll st�mmer det d� du har: " + sceneInfo.energyResource + " st kol");
        }
    }
}
