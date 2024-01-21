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
        UpdateScore();
    }

    private void UpdateScore()
    {
        levelOneScore.text = PlayerPrefs.GetInt("HighScoreLevelOne").ToString("0");
        levelTwoScore.text = PlayerPrefs.GetInt("HighScoreLevelTwo").ToString("0");
        levelThreeScore.text = PlayerPrefs.GetInt("HighScoreLevelThree").ToString("0");
    }
    
}
