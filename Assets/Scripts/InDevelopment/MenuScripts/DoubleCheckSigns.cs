using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCheckSigns : TransitionAnimation
{
    [SerializeField] private GameObject _nextPrefab;
    [SerializeField] private bool isBreaking = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("RightFist") && !isBreaking)
        {
            HitSign();
        }
        
        if (other.gameObject.CompareTag("RightFist") && isBreaking)
        {
            DestroySign();
        }
    }

    private void HitSign()
    {
        Instantiate(_nextPrefab, transform.position, _nextPrefab.transform.rotation);
        Destroy(gameObject);
    }

    private void DestroySign()
    {
        PlayerPrefs.SetInt("HighScoreLevelOne",0);
        PlayerPrefs.SetInt("HighScoreLevelTwo",0);
        PlayerPrefs.SetInt("HighScoreLevelThree",0);
        Instantiate(_nextPrefab, transform.position, _nextPrefab.transform.rotation);
        Destroy(gameObject);
    }
}
