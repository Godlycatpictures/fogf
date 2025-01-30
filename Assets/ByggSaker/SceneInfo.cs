using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Persistence")]
public class SceneInfo : MonoBehaviour
{
    public int energyResource = 0; // kol och s�nt
    public int buildingResource = 0; // till att bygga

    public int kolSomTasUpp = 5; // kol man f�r

    public float TimeScale = 1;

}
