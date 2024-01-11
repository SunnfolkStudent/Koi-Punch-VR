using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSmallGong : MonoBehaviour
{
    private AudioSource _audioSource;
    private GameObject _soundManagerObj;
    private SoundManager _soundManager;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _soundManagerObj = GameObject.FindGameObjectWithTag("SoundManager");
        _soundManager = _soundManagerObj.GetComponent<SoundManager>();
    }

    public void OnMouseDown()
    {
        if (PlayerPrefs.GetFloat("MusicVolume") > 0)
        {
            PlayerPrefs.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume") - .1f);
            Debug.Log(PlayerPrefs.GetFloat("MusicVolume"));
            
            _soundManager.SetVolume();
            
            //_audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            //_audioSource.Play();
        }
    }
}
