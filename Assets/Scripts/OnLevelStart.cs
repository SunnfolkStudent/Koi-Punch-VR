using System.Collections;
using FinalScripts.Fish;
using UnityEngine;

public class OnLevelStart : MonoBehaviour
{
    [Header("AudioSources")]
    [SerializeField] private AudioSource audioPlayer;
    
    [Header("OnLevelStart")]
    [SerializeField] private float initialVoiceLineDelay = 3f;
    [SerializeField] private AudioClip initialVoiceLine;
    [SerializeField] private float startSpawningFishDelayAfterVoiceLine = 2f;
    
    private void Start()
    {
        StartCoroutine(LevelStart());
    }
    
    private IEnumerator LevelStart()
    {
        yield return new WaitForSeconds(initialVoiceLineDelay);
        //  TODO: audioPlayer.PlayOneShot(initialVoiceLine);
        yield return new WaitForSeconds(startSpawningFishDelayAfterVoiceLine);
        EventManager.SpawnFish.Invoke();
    }
}