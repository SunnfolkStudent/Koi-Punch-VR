using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelsMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text levelOneScore;
    [SerializeField] private TMP_Text levelTwoScore;
    [SerializeField] private TMP_Text levelThreeScore;
    
    void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            levelOneScore.text = PlayerPrefs.GetInt("HighScore").ToString("0");
        }
        else
        {
            levelOneScore.text = "0";
        }
    }
    
}
