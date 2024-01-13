using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : BreakOnHit
{
    [SerializeField] private string _newSceneName;
    //private GameObject _fadeScreenObj;
    //private FadeScreen _fadeScreen;

    private void Awake()
    {
        //_fadeScreenObj = GameObject.FindGameObjectWithTag("FadeScreen");
        //_fadeScreen = _fadeScreenObj.GetComponent<FadeScreen>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("collision detected");

            HittingSign();

            StartCoroutine(GoToSceneRoutine());
        }
    }

    IEnumerator GoToSceneRoutine()
    {
        //_fadeScreen.FadeOut();
        //yield return new WaitForSeconds(_fadeScreen.fadeDuration);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(_newSceneName);
    }
}
