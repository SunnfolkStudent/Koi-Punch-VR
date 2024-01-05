using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class FMOD_Manager : MonoBehaviour
{
    private static FMOD_Manager instance;
    void Awake()
    {
        if (FMOD_Manager.instance == null)
        {
            FMOD_Manager.instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        musicBus = RuntimeManager.GetBus("bus:/Music");
        musicBus = RuntimeManager.GetBus("bus:/SFX");
    }

    [Range(0, 1)] public float SFXVolume;
    [Range(0, 1)] public float musicVolume;
    
    private Bus musicBus;
    private Bus sfxBus;
    private void Update()
    {
        sfxBus.setVolume(SFXVolume);
        musicBus.setVolume(musicVolume);
    }
}
