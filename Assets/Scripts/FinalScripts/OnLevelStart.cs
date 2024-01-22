using System;
using System.Collections;
using UnityEngine;
using FinalScripts.Fish;

namespace FinalScripts
{
    public class OnLevelStart : MonoBehaviour
    {
        [Header("OnLevelStart")]
        [SerializeField] private Arenas currentArena;
        [SerializeField] private float initialVoiceLineDelay = 3f;
        [SerializeField] private float startSpawningFishDelayAfterVoiceLine = 2f;

        private enum Arenas
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
            StopAllAmbientSounds();
            switch (currentArena)
            {
                case Arenas.Area1:
                    FMODManager.instance.ambientOne.start();
                    FMODManager.instance.levelOne.start();
                    break;
                case Arenas.Area2:
                    FMODManager.instance.ambientTwo.start();
                    FMODManager.instance.levelTwo.start();
                    break;
                case Arenas.Area3:
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

        private static void StopAllAmbientSounds()
        {
            FMODManager.instance.ambientOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            FMODManager.instance.ambientTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            FMODManager.instance.ambientThree.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}