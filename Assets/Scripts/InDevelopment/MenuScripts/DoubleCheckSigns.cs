using System;
using System.Collections;
using System.Collections.Generic;
using FinalScripts;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class DoubleCheckSigns : TransitionAnimation, IPunchable
{
    [SerializeField] private GameObject _nextPrefab;
    [SerializeField] private bool isBreaking;

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
        CylinderTrigger.LeftHandCanHit = false;
        CylinderTrigger.RightHandCanHit = false;
        Instantiate(_nextPrefab, transform.position, _nextPrefab.transform.rotation);
        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/PlankTap", transform.position);
        transform.localScale = new Vector3(0,0,0);
    }

    private void DestroySign()
    {
        CylinderTrigger.LeftHandCanHit = false;
        CylinderTrigger.RightHandCanHit = false;
        PlayerPrefs.SetInt("HighScoreLevelOne",0);
        PlayerPrefs.SetInt("HighScoreLevelTwo",0);
        PlayerPrefs.SetInt("HighScoreLevelThree",0);
        _resetScore = GameObject.FindGameObjectWithTag("ResetScores");
        _resetScore.GetComponent<DoubleCheckSigns>().UpdateScore();
        Instantiate(_nextPrefab, transform.position, _nextPrefab.transform.rotation);
        FMODManager.instance.PlayOneShot("event:/SFX/MenuSounds/PlankBreak", transform.position);
        Destroy(gameObject);
    }
    
    private void UpdateScore()
    {
        if (!gameObject.CompareTag("ResetScores")) return;
        levelOneScore.text = PlayerPrefs.GetInt("HighScoreLevelOne").ToString("0");
        levelTwoScore.text = PlayerPrefs.GetInt("HighScoreLevelTwo").ToString("0");
        levelThreeScore.text = PlayerPrefs.GetInt("HighScoreLevelThree").ToString("0");
    }
    public void PunchObject(ControllerManager controllerManager, string fistUsed)
    {
        if (fistUsed == "LeftFist" && CylinderTrigger.LeftHandCanHit && controllerManager.leftVelMagnitude > 1)
        {
            HapticManager.leftWoodPunch = true;
            if (!isBreaking)
            {
                HitSign();
            }

            if (isBreaking)
            {
                DestroySign();
            }
        }
        else if (fistUsed == "RightFist" && CylinderTrigger.RightHandCanHit && controllerManager.rightVelMagnitude > 1)
        {
            HapticManager.rightWoodPunch = true;
            if (!isBreaking)
            {
                HitSign();
            }

            if (isBreaking)
            {
                DestroySign();
            }
        }
        else
        {
            RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/PlankTap", transform.position);
        }
    }
}
