using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject musicSlider;
    private float sliderPos;

    private void Awake()
    {
        PlayerPrefs.SetFloat("MusicVolume", 1);
    }

    /*private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.GetFloat("MusicVolume");
            SetVolume();
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            SetVolume();
        }
    }*/

    public void SetVolume()
    {
        sliderPos = PlayerPrefs.GetFloat("MusicVolume") - 0.5f;
        //musicSlider.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + sliderPos);
        //musicSlider.transform.= sliderPos;
    }

    private void Update()
    {
        Debug.Log(PlayerPrefs.GetFloat("MusicVolume"));
    }
}
