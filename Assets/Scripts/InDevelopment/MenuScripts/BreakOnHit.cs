using System;
using System.Collections;
using System.Collections.Generic;
using FinalScripts;
using Unity.Mathematics;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class BreakOnHit : TransitionAnimation, IPunchable
{
    [SerializeField] private GameObject _brokenPrefab;
    [SerializeField] private int LevelToGoTo;
    [SerializeField] private GameObject newMenuParent;

    private GameObject _sceneControllerObj;
    private SceneController _sceneController;

    private void Awake()
    {
        _sceneControllerObj = GameObject.FindGameObjectWithTag("SceneController");
        _sceneController = _sceneControllerObj.GetComponent<SceneController>();
    }

    private void HittingSign()
    {
        CylinderTrigger.LeftHandCanHit = false;
        CylinderTrigger.RightHandCanHit = false;
        Instantiate(_brokenPrefab, transform.position, _brokenPrefab.transform.rotation);
        FMODManager.instance.PlayOneShot("event:/SFX/MenuSounds/PlankBreak", transform.position);
        MenuEventManager.ExplodeTransition();
        
        if (gameObject.CompareTag("SceneChanger"))
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            _sceneController.ChangeScenes(LevelToGoTo);
        }
        else if (gameObject.CompareTag("StartButton"))
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            _sceneController.StartGame();
        }
        else if (gameObject.CompareTag("StartButton2"))
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0);
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
        if (fistUsed == "LeftFist" && CylinderTrigger.LeftHandCanHit && controllerManager.leftVelMagnitude > 10)
        {
            HapticManager.leftWoodPunch = true;
            Invoke("StopBuzzing", .5f);
            HittingSign();
        }
        else if (fistUsed == "RightFist" && CylinderTrigger.RightHandCanHit && controllerManager.rightVelMagnitude > 10)
        {
            HapticManager.rightWoodPunch = true;
            Invoke("StopBuzzing",.5f);
            HittingSign();
        }
        else
        {
            RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/PlankTap", transform.position);
        }
    }

    private void StopBuzzing()
    {
        HapticManager.leftWoodPunch = false;
        HapticManager.rightWoodPunch = false;
    }
}