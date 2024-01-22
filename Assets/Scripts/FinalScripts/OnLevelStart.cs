using System;
using System.Collections;
using FinalScripts.Fish;
using UnityEngine;

namespace FinalScripts
{
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
                    FMODManager.instance.ambientOne.start();
                    FMODManager.instance.levelOne.start();
                    break;
                case Levels.Area2:
                    FMODManager.instance.ambientTwo.start();
                    FMODManager.instance.levelTwo.start();
                    break;
                case Levels.Area3:
                    FMODManager.instance.ambientThree.start();
                    FMODManager.instance.levelThree.start();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield return new WaitForSeconds(initialVoiceLineDelay);
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/GameStart");
            yield return new WaitForSeconds(startSpawningFishDelayAfterVoiceLine);
            EventManager.FishSpawning.Invoke();
        }
    }
}