using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetNewHighScore : MonoBehaviour
{
    private TMP_Text _newHighScoreText;
    void Start()
    {
        _newHighScoreText = GetComponent<TMP_Text>();
        _newHighScoreText.text = PlayerPrefs.GetInt("HighScore").ToString("0");
    }
}
