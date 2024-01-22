using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ChargeSfx : MonoBehaviour
{
    private void Awake()
    {
        RuntimeManager.AttachInstanceToGameObject(FMODManager.instance.koiPunch, gameObject.transform, this.GetComponent<Rigidbody>());
    }
    
    private void Start()
    {
        InternalZenEventManager.playChargeSfx += PlayChargeSfx;
        InternalZenEventManager.playChargeReadySfx += PlayChargeReadySfx;
        InternalZenEventManager.playChargePunchSfx += PlayChargePunchSfxCoroutine;
    }

    #region SFX

    private void PlayChargeSfx()
    {
        FMODManager.instance.koiPunch.setParameterByName("koiPunchSoundState", 0);
        FMODManager.instance.koiPunch.setParameterByName("koiPunchImpactState", 0);
        FMODManager.instance.koiPunch.start();
    }
        
    private void PlayChargeReadySfx()
    {
        FMODManager.instance.koiPunch.setParameterByName("koiPunchSoundState", 1);
        FMODManager.instance.koiPunch.setParameterByName("koiPunchImpactState", 1);
    }
        
    private void PlayChargePunchSfxCoroutine()
    {
        StartCoroutine(PlayChargePunchSfx());
    }

    private IEnumerator PlayChargePunchSfx()
    {
        //do this in each hand :3
        FMODManager.instance.koiPunch.setParameterByName("koiPunchSoundState", 2);
        yield return new WaitForSecondsRealtime(4f);
        FMODManager.instance.koiPunch.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //this is for the vocals:
        FMODManager.instance.koiPunchVocals.setParameterByName("koiPunchSoundState", 0);
        yield return new WaitForSecondsRealtime(4f);
        FMODManager.instance.koiPunchVocals.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    #endregion
}
