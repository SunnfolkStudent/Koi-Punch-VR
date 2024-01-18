using System;
using System.Collections;
using System.Collections.Generic;
using InDevelopment.Punch;
using UnityEngine;

public class BreakOnHit : TransitionAnimation
{
    [SerializeField] private GameObject _brokenPrefab;
    [SerializeField] private int LevelToGoTo;
    [SerializeField] private GameObject newMenuParent;

    private GameObject _sceneControllerObj;
    private SceneController _sceneController;

    private void Awake()
    {
        if (gameObject.CompareTag("SceneChanger"))
        {
            _sceneControllerObj = GameObject.FindGameObjectWithTag("SceneController");
            _sceneController = _sceneControllerObj.GetComponent<SceneController>();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("LeftFist") || other.gameObject.CompareTag("RightFist"))
        {
            HittingSign();
        }
    }

    protected void HittingSign()
    {
        Instantiate(_brokenPrefab,gameObject.transform.position,Quaternion.identity);
        
        //TODO play break audio
        
        if (gameObject.CompareTag("SceneChanger"))
        {
            if (_sceneControllerObj == null)
                return;
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            _sceneController.ChangeScenes(LevelToGoTo);
        }
        else
        {
            MenuEventManager.ExplodeTransition();
            Instantiate(newMenuParent);
            Destroy(gameObject);
        }
    }
}