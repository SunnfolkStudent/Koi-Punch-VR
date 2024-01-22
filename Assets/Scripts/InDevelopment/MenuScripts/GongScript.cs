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
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong",2f, transform.position);
                        _soundManager.SetMusicVolume();
                    }
                    else
                    {
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong",2f, transform.position);
                        PlayerPrefs.SetFloat("MusicVolume", 1);
                        _soundManager.SetMusicVolume();
                    }
                }

                if (!isBig)
                {
                    if (PlayerPrefs.GetFloat("MusicVolume") > 0)
                    {
                        PlayerPrefs.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume") - .1f);
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong",0.5f, transform.position);
                        _soundManager.SetMusicVolume();
                    }
                    else
                    {
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong",0.5f, transform.position);
                        PlayerPrefs.SetFloat("MusicVolume", 0);
                        _soundManager.SetMusicVolume();
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
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong",2f, transform.position);
                        _soundManager.SetSFXVolume();
                    }
                    else
                    {
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong",2f, transform.position);
                        PlayerPrefs.SetFloat("SFXVolume", 1);
                        _soundManager.SetSFXVolume();
                    }
                }

                if (!isBig)
                {
                    if (PlayerPrefs.GetFloat("SFXVolume") > 0)
                    {
                        PlayerPrefs.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume") - .1f);
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong",0.5f, transform.position);
                        _soundManager.SetSFXVolume();
                    }
                    else
                    {
                        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong",0.5f, transform.position);
                        PlayerPrefs.SetFloat("SFXVolume", 0);
                        _soundManager.SetSFXVolume();
                    }
                }
            }
        }
    }
}
