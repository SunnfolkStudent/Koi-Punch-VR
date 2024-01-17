using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GongScript : MonoBehaviour
{
    //private AudioSource _audioSource;
    [SerializeField] private GameObject _soundManagerObj;
    private SoundManager _soundManager;

    [SerializeField] private bool isMusic;
    [SerializeField] private bool isBig;

    private void Start()
    {
        //_audioSource = GetComponent<AudioSource>();
        _soundManager = _soundManagerObj.GetComponent<SoundManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (isMusic)
            {
                if (isBig)
                {
                    if (PlayerPrefs.GetFloat("MusicVolume") < 1)
                    {
                        PlayerPrefs.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume") + .1f);
                        //Debug.Log(PlayerPrefs.GetFloat("MusicVolume"));
                        
                        _soundManager.SetMusicVolume();
                        
                        //TODO play gong audio

                        //_audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
                        //_audioSource.Play();
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
                        //Debug.Log(PlayerPrefs.GetFloat("MusicVolume"));
                        
                        _soundManager.SetMusicVolume();
                        
                        //TODO play gong audio

                        //_audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
                        //_audioSource.Play();
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
                        //Debug.Log(PlayerPrefs.GetFloat("SFXVolume"));
                        
                        _soundManager.SetSFXVolume();
                        
                        //TODO play gong audio

                        //_audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
                        //_audioSource.Play();
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
                        //Debug.Log(PlayerPrefs.GetFloat("SFXVolume"));
                        
                        _soundManager.SetSFXVolume();
                        
                        //TODO play gong audio

                        //_audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
                        //_audioSource.Play();
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
