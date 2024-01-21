using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoubleCheckSigns : TransitionAnimation
{
    [SerializeField] private GameObject _nextPrefab;
    [SerializeField] private bool isBreaking = false;

    private GameObject _resetScore;
    
    [SerializeField] private TMP_Text levelOneScore;
    [SerializeField] private TMP_Text levelTwoScore;
    [SerializeField] private TMP_Text levelThreeScore;

    private void Awake()
    {
        UpdateScore();
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if ((other.gameObject.CompareTag("LeftFist") && CylinderTrigger.LeftHandCanHit) || (other.gameObject.CompareTag("RightFist") && CylinderTrigger.RightHandCanHit))
        {
            CylinderTrigger.LeftHandCanHit = false;
            CylinderTrigger.RightHandCanHit = false;
            
            if (!isBreaking)
            {
                HitSign();
            }

            if (isBreaking)
            {
                DestroySign();
            }
        }
    }

    private void HitSign()
    {
        Instantiate(_nextPrefab, transform.position, _nextPrefab.transform.rotation);
        transform.localScale = new Vector3(0,0,0);
    }

    private void DestroySign()
    {
        PlayerPrefs.SetInt("HighScoreLevelOne",0);
        PlayerPrefs.SetInt("HighScoreLevelTwo",0);
        PlayerPrefs.SetInt("HighScoreLevelThree",0);
        _resetScore = GameObject.FindGameObjectWithTag("ResetScores");
        _resetScore.GetComponent<DoubleCheckSigns>().UpdateScore();
        Instantiate(_nextPrefab, transform.position, _nextPrefab.transform.rotation);
        Destroy(gameObject);
    }
    
    private void UpdateScore()
    {
        if (!gameObject.CompareTag("ResetScores")) return;
        levelOneScore.text = PlayerPrefs.GetInt("HighScoreLevelOne").ToString("0");
        levelTwoScore.text = PlayerPrefs.GetInt("HighScoreLevelTwo").ToString("0");
        levelThreeScore.text = PlayerPrefs.GetInt("HighScoreLevelThree").ToString("0");
    }
}
