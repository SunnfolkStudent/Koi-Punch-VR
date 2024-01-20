using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Musictest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            FMODManager.instance.ambientOne.start();
        }

        if (Input.GetKeyDown("down"))
        {
            FMODManager.instance.ambientOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            FMODManager.instance.menuTheme.setParameterByName("PunchedThing", 0);
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/BossComments/BossPhase3");
            RuntimeManager.AttachInstanceToGameObject(FMODManager.instance.koiPunch, gameObject.transform, this.GetComponent<Rigidbody>());
        }
    }
}
