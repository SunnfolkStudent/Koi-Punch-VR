using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLevelsSign : MonoBehaviour
{
    [SerializeField] private bool credits;

    /*private void Awake()
    {
        PlayerPrefs.SetInt("HideSign", 0);
    }*/

    void Start()
    {
        if (credits)
        {
            PlayerPrefs.SetInt("HideSign", 1);
        }
        else if(PlayerPrefs.GetInt("HideSign") == 0)
        {
            Destroy(gameObject);
        }
    }
}
