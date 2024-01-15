using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class FMODManager: MonoBehaviour
{
    private static FMODManager instance;
    
    #region Variables
    
    private Camera cam;
    private GameObject leftHand;
    private GameObject rightHand;
    
    private EventInstance leftHandWind = RuntimeManager.CreateInstance("event:/SFX/PlayerSounds/HandSounds/HandWind");
    private EventInstance rightHandWind = RuntimeManager.CreateInstance("event:/SFX/PlayerSounds/HandSounds/HandWind");
    private EventInstance levelOne = RuntimeManager.CreateInstance("event:/Music/LevelMusic/SpringLevel");
    private EventInstance levelTwo = RuntimeManager.CreateInstance("event:/Music/LevelMusic/FallLevel");
    private EventInstance levelThree = RuntimeManager.CreateInstance("event:/Music/LevelMusic/WinterLevel");
    private EventInstance koiPunch = RuntimeManager.CreateInstance("event:/SFX/PlayerSounds/ChargeSounds/KoiPunch");
    
    [SerializeField] [Range(0,100)] private float velocityFloor;
    [Range(0, 1)] private float sfxVolume;
    [Range(0, 1)] private float musicVolume;
    private float playerLeftHandVelocity;
    private float playerRightHandVelocity;
    
    #endregion
    
    public float SfxVolume
    {
        get => sfxVolume;
        set => sfxVolume = value;
    }
    public float MusicVolume
    {
        get => musicVolume;
        set => musicVolume = value;
    }
    private void Awake()
    {
        if (FMODManager.instance == null)
        {
            FMODManager.instance = this;
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
        
        RuntimeManager.AttachInstanceToGameObject(leftHandWind,  leftHand.GetComponent<Transform>(), leftHand.GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(rightHandWind,  rightHand.GetComponent<Transform>(), rightHand.GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(levelOne, cam.GetComponent<Transform>(), cam.GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(levelTwo, cam.GetComponent<Transform>(), cam.GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(levelThree, cam.GetComponent<Transform>(), cam.GetComponent<Rigidbody>());
    }
    
    private Bus musicBus;
    private Bus sfxBus;
    private void Update()
    {
        sfxBus.setVolume(sfxVolume);
        musicBus.setVolume(musicVolume);
        
        if (playerLeftHandVelocity > velocityFloor)
        {
            leftHandWind.start();
            leftHandWind.setParameterByName("soundVelocity", playerLeftHandVelocity);
        }
        else
        {
            leftHandWind.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        if (playerRightHandVelocity > velocityFloor)
        {
            rightHandWind.start();
            rightHandWind.setParameterByName("soundVelocity", playerRightHandVelocity);
        }
        else
        {
            rightHandWind.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
    
    /*how to use:
     1: using FMODUnity
     2: [SerializeField] private EventReference exampleVariable
     3: FMODManager.instance.PlayOneShot(exampleVariable, this.transform.position)*/
    public void PlayOneShot(string sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    
    public void OnStartLevelMusic(int levelNumber)
    {
        switch (levelNumber)
        {
            case 0:
            {
                //add main menu logic
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

    public FMODManager(GameObject leftHand)
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
