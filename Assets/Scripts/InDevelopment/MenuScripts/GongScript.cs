using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class GongScript : MonoBehaviour
{
    [SerializeField] private SoundManager _soundManager;

    [SerializeField] private bool isMusic;
    [SerializeField] private bool isBig;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("LeftFist") || other.gameObject.CompareTag("RightFist"))
        {
            if (isMusic)
            {
                if (isBig)
                {
                    if (PlayerPrefs.GetFloat("MusicVolume") < 1)
                    {
                        PlayerPrefs.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume") + .1f);
                        //FMODManager.instance.MusicBus += .1f;
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong", transform.position);
                        _soundManager.SetMusicVolume();
                    }
                    else
                    {
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong", transform.position);
                        PlayerPrefs.SetFloat("MusicVolume", 1);
                    }
                }

                if (!isBig)
                {
                    if (PlayerPrefs.GetFloat("MusicVolume") > 0)
                    {
                        PlayerPrefs.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume") - .1f);
                        //FMODManager.instance.MusicBus -= .1f;
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong", transform.position);
                        _soundManager.SetMusicVolume();
                    }
                    else
                    {
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong", transform.position);
                        PlayerPrefs.SetFloat("MusicVolume", 0);
                    }
                }
            }

            if (!isMusic)
            {
                if (isBig)
                {
                    if (PlayerPrefs.GetFloat("SFXVolume") < 1)
                    {
                        PlayerPrefs.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume") + .1f);
                        //FMODManager.instance.SfxBus += .1f;
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong", transform.position);
                        _soundManager.SetSFXVolume();
                    }
                    else
                    {
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong", transform.position);
                        PlayerPrefs.SetFloat("SFXVolume", 1);
                    }
                }

                if (!isBig)
                {
                    if (PlayerPrefs.GetFloat("SFXVolume") > 0)
                    {
                        PlayerPrefs.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume") - .1f);
                        //FMODManager.instance.SfxBus -= .1f;
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong", transform.position);
                        _soundManager.SetSFXVolume();
                    }
                    else
                    {
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong", transform.position);
                        PlayerPrefs.SetFloat("SFXVolume", 0);
                    }
                }
            }
        }
    }
}
