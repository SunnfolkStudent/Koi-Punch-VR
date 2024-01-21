using System;
using System.Collections;
using System.Collections.Generic;
using FinalScripts;
using Unity.Mathematics;
using UnityEngine;
//using FMOD.Studio;
//using FMODUnity;

public class BreakOnHit : TransitionAnimation, IPunchable
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
        // TODO FMODManager.instance.PlayOneShot("event:/SFX/MenuSounds/PlankBreak", transform.position);
        MenuEventManager.ExplodeTransition();
        
        if (gameObject.CompareTag("SceneChanger"))
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            _sceneController.ChangeScenes(LevelToGoTo);
        }
        else if (gameObject.CompareTag("StartButton"))
        {
            _sceneController.StartGame();
        }
        else if (gameObject.CompareTag("StartButton2"))
        {
            _sceneController.StartGameAfterIntro();
        }
        else
        {
            Instantiate(newMenuParent);
            Destroy(gameObject);
        }
    }
    
    public void PunchObject(ControllerManager controllerManager, string fistUsed)
    {
        /*var v = fistUsed == "LeftFist"
            ? controllerManager.leftControllerVelocity
            : controllerManager.rightControllerVelocity;
        if (math.abs(v.magnitude) >= fish.successfulPunchThreshold) HittingSign();
        else Debug.Log("Not Good Enough!");*/
    }
}