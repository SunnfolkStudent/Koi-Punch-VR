using System;
using System.Collections;
using UnityEngine;
using FinalScripts.Fish;
using FMOD.Studio;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

namespace FinalScripts
{
    public class OnLevelStart : MonoBehaviour
    {
        [Header("OnLevelStart")]
        [SerializeField] private Arenas currentArena;
        [SerializeField] private float initialVoiceLineDelay = 3f;
        [SerializeField] private float startSpawningFishDelayAfterVoiceLine = 2f;

        private PLAYBACK_STATE _1Play;
        private PLAYBACK_STATE _2Play;
        private PLAYBACK_STATE _3Play;
        
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

        private void Update()
        {
            // Debug.Log("CurrentModeUpdate 1" + _1Play);
            //
            // Debug.Log("CurrentModeUpdate 2" + _2Play);
            //
            // Debug.Log("CurrentModeUpdate 3" + _3Play);

            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                FMODManager.instance.levelOne.start();
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                FMODManager.instance.StopAllInstances();
            }

            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                FMODManager.instance.levelOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                FMODManager.instance.ambientOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                FMODManager.instance.zenMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        private IEnumerator LevelStart()
        {
            FMODManager.instance.StopAllInstances();
            
            switch (currentArena)
            {
                case Arenas.Area1:
                    //Memory.Initialize();
                    FMODManager.instance.ambientOne.start();
                    FMODManager.instance.levelOne.start();
                    FMODManager.instance.levelOne.getPlaybackState( out _1Play);
                    
                    
                    /*if (_play == PLAYBACK_STATE.PLAYING)
                    {
                        Debug.Log("MusicIsPlaying");
                    }
                    else if (_play == PLAYBACK_STATE.STOPPED)
                    {
                        Debug.Log("MusicIsNotPlaying");
                    }
                    else
                    {
                        Debug.Log("CurrentMode" + _play);
                    }*/
                    break;
                case Arenas.Area2:
                    FMODManager.instance.ambientTwo.start();
                    FMODManager.instance.levelTwo.start();
                    FMODManager.instance.levelTwo.getPlaybackState( out _2Play);
                    break;
                case Arenas.Area3:
                    FMODManager.instance.ambientThree.start();
                    FMODManager.instance.levelThree.start();
                    FMODManager.instance.levelThree.getPlaybackState( out _3Play);
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