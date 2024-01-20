using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BreakOnHit : TransitionAnimation//, IPunchable
{
    [SerializeField] private GameObject _brokenPrefab;
    [SerializeField] private int LevelToGoTo;
    [SerializeField] private GameObject newMenuParent;

    private GameObject _sceneControllerObj;
    private SceneController _sceneController;

    private void Awake()
    {
        if (gameObject.CompareTag("SceneChanger") || gameObject.CompareTag("StartButton"))
        {
            _sceneControllerObj = GameObject.FindGameObjectWithTag("SceneController");
            _sceneController = _sceneControllerObj.GetComponent<SceneController>();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.CompareTag("LeftFist") && CylinderTrigger.LeftHandCanHit) || (other.gameObject.CompareTag("RightFist") && CylinderTrigger.RightHandCanHit))
        {
            Debug.Log("Hit went through");
            CylinderTrigger.LeftHandCanHit = false;
            CylinderTrigger.RightHandCanHit = false;
            HittingSign();
        }
    }

    private void HittingSign()
    {
        Instantiate(_brokenPrefab, transform.position, _brokenPrefab.transform.rotation);
        MenuEventManager.ExplodeTransition();
        
        //TODO play break audio
        
        if (gameObject.CompareTag("SceneChanger"))
        {
            if (_sceneControllerObj == null)
                return;
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            _sceneController.ChangeScenes(LevelToGoTo);
        }
        else if (gameObject.CompareTag("StartButton"))
        {
            _sceneController.StartGame();
        }
        else
        {
            Instantiate(newMenuParent);
            Destroy(gameObject);
        }
    }

    /*public void PunchObject(ControllerManager controllerManager, string fistUsed)
    {
        throw new NotImplementedException();
    }*/
}