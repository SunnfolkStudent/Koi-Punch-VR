using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private GameObject _musicSlider;
    private float _musicSliderPos;

    private GameObject _sFXSlider;
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
        _musicSliderPos = PlayerPrefs.GetFloat("MusicVolume") - 0.5f;
        FMODManager.instance.MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        _musicSlider = GameObject.FindGameObjectWithTag("MusicSlider");
        if (!_musicSlider) return;
        _musicSlider.transform.localPosition = new Vector3(transform.localPosition.x - _musicSliderPos, -0.1f, 0);
    }

    public void SetSFXVolume()
    {
        _sFXSliderPos = PlayerPrefs.GetFloat("SFXVolume") - 0.5f;
        FMODManager.instance.SfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        _sFXSlider = GameObject.FindGameObjectWithTag("SfxSlider");
        if (!_sFXSlider) return;
        _sFXSlider.transform.localPosition = new Vector3(transform.localPosition.x - _sFXSliderPos, -0.1f, 0);
    }
}
