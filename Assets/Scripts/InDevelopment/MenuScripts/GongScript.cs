using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                        
                        _soundManager.SetMusicVolume();
                    }
                    else
                    {
                        PlayerPrefs.SetFloat("MusicVolume", 1);
                    }
                }

                if (!isBig)
                {
                    if (PlayerPrefs.GetFloat("MusicVolume") > 0)
                    {
                        PlayerPrefs.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume") - .1f);
                        
                        _soundManager.SetMusicVolume();
                    }
                    else
                    {
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
                        
                        _soundManager.SetSFXVolume();
                       
                    }
                    else
                    {
                        PlayerPrefs.SetFloat("SFXVolume", 1);
                    }
                }

                if (!isBig)
                {
                    if (PlayerPrefs.GetFloat("SFXVolume") > 0)
                    {
                        PlayerPrefs.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume") - .1f);
                        
                        _soundManager.SetSFXVolume();
                        
                    }
                    else
                    {
                        PlayerPrefs.SetFloat("SFXVolume", 0);
                    }
                }
            }
        }
    }
}
