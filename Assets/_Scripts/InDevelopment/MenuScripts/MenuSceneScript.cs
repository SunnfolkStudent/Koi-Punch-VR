using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class MenuSceneScript : MonoBehaviour
{
    void Start()
    {
        FMODManager.instance.ambientTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODManager.instance.ambientThree.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODManager.instance.ambientOne.start();
        // Change ambientOne to ambientTwo and ambientThree for different soundscapes
        
        FMODManager.instance.zenMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODManager.instance.menuTheme.setParameterByName("PunchedThing", 0);
        FMODManager.instance.menuTheme.start();
    }
}
