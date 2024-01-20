using System;
using System.Collections;
using FinalScripts.Fish;
using UnityEngine;
// TODO: using FMODUnity;
// TODO: using FMODStudio;

public class OnLevelStart : MonoBehaviour
{
    [Header("OnLevelStart")]
    [SerializeField] private Levels currentLevel;
    [SerializeField] private float initialVoiceLineDelay = 3f;
    [SerializeField] private float startSpawningFishDelayAfterVoiceLine = 2f;

    private enum Levels
    {
        Area1,
        Area2,
        Area3
    }
    
    private void Start()
    {
        StartCoroutine(LevelStart());
    }
    
    private IEnumerator LevelStart()
    {
        switch (currentLevel)
        {
            case Levels.Area1:
                // TODO: FMODManager.instance.ambientOne.start();
                // TODO: FMODManager.instance.levelOne.start();
            break;
            case Levels.Area2:
                // TODO: FMODManager.instance.ambientTwo.start();
                // TODO: FMODManager.instance.levelTwo.start();
            break;
            case Levels.Area3:
                // TODO: FMODManager.instance.ambientThree.start();
                // TODO: FMODManager.instance.levelThree.start();
            break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        yield return new WaitForSeconds(initialVoiceLineDelay);
        // TODO: FMODManager.instance.PlayOneShot("event:/SFX/Voice/GameStart");
        yield return new WaitForSeconds(startSpawningFishDelayAfterVoiceLine);
        EventManager.SpawnFish.Invoke();
    }
}