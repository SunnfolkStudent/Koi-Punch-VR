using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class FMOD_Manager : MonoBehaviour
{
    private static FMOD_Manager instance;
    private Camera cam;
    [SerializeField] [Range(0,100)] private float velocityFloor;
    public Rigidbody leftHandRigid;
    private GameObject leftHand;
    private GameObject rightHand;
    private EventInstance leftHandWoosh = RuntimeManager.CreateInstance("event:/SFX/PlayerSounds/HandSounds/Swoosh");
    private EventInstance rightHandWoosh = RuntimeManager.CreateInstance("event:/SFX/PlayerSounds/HandSounds/Swoosh");
    [Range(0, 1)] private float _SFXVolume;
    [Range(0, 1)] private float _musicVolume;
    private float playerLeftHandVelocity;
    private float playerRightHandVelocity;
    public float SFXVolume
    {
        get => _SFXVolume;
        set => _SFXVolume = value;
    }
    public float MusicVolume
    {
        get => _musicVolume;
        set => _musicVolume = value;
    }
    void Awake()
    {
        leftHandRigid = gameObject.GetComponent<Rigidbody>();
        
        if (FMOD_Manager.instance == null)
        {
            FMOD_Manager.instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        cam = Camera.main;
        musicBus = RuntimeManager.GetBus("bus:/Music");
        musicBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        
        leftHand = GameObject.Find("leftHand");
        rightHand = GameObject.Find("rightHand");
        
        RuntimeManager.AttachInstanceToGameObject(leftHandWoosh,  leftHand.GetComponent<Transform>(), leftHand.GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(rightHandWoosh,  rightHand.GetComponent<Transform>(), rightHand.GetComponent<Rigidbody>());
    }
    
    private Bus musicBus;
    private Bus sfxBus;
    private void Update()
    {
        sfxBus.setVolume(_SFXVolume);
        musicBus.setVolume(_musicVolume);
        
        if (playerLeftHandVelocity > velocityFloor)
        {
            leftHandWoosh.start();
            leftHandWoosh.setParameterByName("soundVelocity", playerLeftHandVelocity);
        }
        else
        {
            leftHandWoosh.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        if (playerRightHandVelocity > velocityFloor)
        {
            rightHandWoosh.start();
            rightHandWoosh.setParameterByName("soundVelocity", playerRightHandVelocity);
        }
        else
        {
            rightHandWoosh.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
    
    /*for å bruke:
     1: using FMODUnity
     2: [SerializeField] private EventReference eksempelVariabel
     3: FMODManager.instance.PlayOneShot(eksempelvariabel, this.transform.position)*/
    public void PlayOneShot(string sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    private EventInstance levelOne = RuntimeManager.CreateInstance("event:/Music/FallLevel");
    private EventInstance levelTwo = RuntimeManager.CreateInstance("event:/Music/SpringLevel");
    private EventInstance levelThree = RuntimeManager.CreateInstance("event:/Music/WinterLevel");
    
    public void OnStartLevelMusic(int levelNumber)
    {
        switch (levelNumber)
        {
            case 0:
            {
                //legg til hovedmeny-logikk
                break;
            }
            case 1:
            {
                RuntimeManager.PlayOneShotAttached("event:/SFX/Stingers/LevelStart", cam.gameObject);
                levelTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                levelThree.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                levelOne.start();
                break;
            }
            case 2:
            {
                RuntimeManager.PlayOneShotAttached("event:/SFX/Stingers/LevelStart", cam.gameObject);
                levelThree.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                levelOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                levelTwo.start();
                break;
            }
            case 3:
            {
                RuntimeManager.PlayOneShotAttached("event:/SFX/Stingers/LevelStart", cam.gameObject);
                levelOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                levelTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                levelThree.start();
                break;
            }
        }
    }

    public void ZenModeMusicManager(int zenStage, float zenPercent)
    {
        
    }
    
    private EventInstance koiPunch = RuntimeManager.CreateInstance("event:/SFX/PlayerSounds/ChargeSounds/KoiPunch");

    public FMOD_Manager(GameObject leftHand)
    {
        this.leftHand = leftHand;
    }

    public void KoiPunchSounds(int koiPunchState)
    {
        switch (koiPunchState)
        {
            case 1:
            {
                koiPunch.start();
                koiPunch.setParameterByName("koiPunchSoundState", 0);
                break;
            }
            case 2:
            {
                koiPunch.start();
                koiPunch.setParameterByName("koiPunchSoundState", 1);
                break;
            }
        }
    }
}
