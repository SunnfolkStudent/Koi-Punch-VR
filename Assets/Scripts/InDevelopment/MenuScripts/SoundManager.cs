using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject _musicSlider;
    private float _musicSliderPos;

    [SerializeField] private GameObject _sFXSlider;
    private float _sFXSliderPos;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.GetFloat("MusicVolume");
            SetMusicVolume();
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            SetMusicVolume();
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.GetFloat("SFXVolume");
            SetSFXVolume();
        }
        else
        {
            PlayerPrefs.SetFloat("SFXVolume", 1);
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        // TODO RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong", transform.position);
        
        _musicSliderPos = PlayerPrefs.GetFloat("MusicVolume") - 0.5f;
        _musicSlider.transform.localPosition = new Vector3(transform.localPosition.x - _musicSliderPos, -0.1f, 0);
    }

    public void SetSFXVolume()
    {
        // TODO RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/Gong", transform.position);
        
        _sFXSliderPos = PlayerPrefs.GetFloat("SFXVolume") - 0.5f;
        _sFXSlider.transform.localPosition = new Vector3(transform.localPosition.x - _sFXSliderPos, -0.1f, 0);
    }
}
