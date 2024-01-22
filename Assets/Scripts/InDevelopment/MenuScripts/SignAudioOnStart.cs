using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class SignAudioOnStart : MonoBehaviour
{
    void Start()
    {
        RuntimeManager.PlayOneShot("event:/SFX/MenuSounds/WoodHitGrass", transform.position);
    }
}
