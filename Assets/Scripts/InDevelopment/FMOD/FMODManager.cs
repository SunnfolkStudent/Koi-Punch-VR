using System.Collections;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Random = UnityEngine.Random;

public class FMODManager: MonoBehaviour
{
    public static FMODManager instance;
    
    #region Variables

    public ControllerManager controllerManager;
    
    private Camera cam;

    private EventInstance levelOne;
    private EventInstance levelTwo;
    private EventInstance levelThree;
    //private EventInstance koiPunch = RuntimeManager.CreateInstance("event:/SFX/PlayerSounds/ChargeSounds/KoiPunch"); //not done!
    
    [SerializeField] [Range(0,100)] private float velocityFloor; 
    [Range(0, 1)] private float sfxVolume;
    [Range(0, 1)] private float musicVolume;
    private float playerLeftHandVelocity;
    private float playerRightHandVelocity;
    [SerializeField] [Range(0, 100)] private float wooshVolume;
    [SerializeField] private bool windPlaying = false;

    public string[] soundPaths;
    public string selectedSoundPath;
    
    private Bus musicBus;
    private Bus sfxBus;

    private GameObject Left;
    private GameObject Right;
    
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
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        soundPaths = new string[2]
        {
            "event:/SFX/Voice/RandomPunchComments/PlayerGreatHit",
            "event:/SFX/Voice/RandomPunchComments/PlayerFantasticHit",
        };
        
        Left = GameObject.Find("Hand_L");
        Right = GameObject.Find("Hand_R");
        levelOne = RuntimeManager.CreateInstance("event:/Music/LevelMusic/Level1");
        levelTwo = RuntimeManager.CreateInstance("event:/Music/LevelMusic/Level2");
        levelThree = RuntimeManager.CreateInstance("event:/Music/LevelMusic/Level3");
        
        RuntimeManager.AttachInstanceToGameObject(levelOne, cam.GetComponent<Transform>(), cam.GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(levelTwo, cam.GetComponent<Transform>(), cam.GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(levelThree, cam.GetComponent<Transform>(), cam.GetComponent<Rigidbody>());
    }
    
    private void Update()
    {
        sfxBus.setVolume(sfxVolume);
        musicBus.setVolume(musicVolume);
        
        if (Input.GetKeyDown("space"))
        {
            SelectRandomPunchSound();
        }
    }

    private void FixedUpdate()
    {
        if (controllerManager.leftVelMagnitude > velocityFloor && windPlaying == false)
        {
            StartCoroutine(LeftHandWind());
            windPlaying = false;
        }
       
        if (controllerManager.rightVelMagnitude > velocityFloor && windPlaying == false)
        {
            StartCoroutine(RightHandWind());
            windPlaying = false;
        }
    }
    private IEnumerator LeftHandWind()
    {
        PlayOneShot("event:/SFX/PlayerSounds/HandSounds/HandWind", controllerManager.leftVelMagnitude * wooshVolume, Left.transform.position);
        yield return new WaitForSeconds(1);
        windPlaying = true;
    }
    private IEnumerator RightHandWind()
    {
        PlayOneShot("event:/SFX/PlayerSounds/HandSounds/HandWind", controllerManager.leftVelMagnitude * wooshVolume, Right.transform.position);
        yield return new WaitForSeconds(1);
        windPlaying = true;
    }

    /*how to use:
     1: using FMODUnity
     2: FMODManager.instance.PlayOneShot(string path, GameObject.Find("exampleGameObject").transform.position)*/
    public void PlayOneShot(string sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    
    public void PlayOneShot(string sound, float volume, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, volume, worldPos);
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
                RuntimeManager.PlayOneShotAttached("event:/Music/Stingers/LevelStart", cam.gameObject);
                levelTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                levelThree.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                levelOne.start();
                break;
            }
            case 2:
            {
                RuntimeManager.PlayOneShotAttached("event:/Music/Stingers/LevelStart", cam.gameObject);
                levelThree.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                levelOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                levelTwo.start();
                break;
            }
            case 3:
            {
                RuntimeManager.PlayOneShotAttached("event:/Music/Stingers/LevelStart", cam.gameObject);
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
    
    void SelectRandomPunchSound()
    {
        if (ShouldRun())
        {
            Debug.Log("The if statement ran!");
            // Checks if the Array is empty
            if (soundPaths == null || soundPaths.Length == 0)
            {
                Debug.LogError("Array is null or empty.");
                return;
            }

            var randomIndex = Random.Range(0, soundPaths.Length);

            selectedSoundPath = soundPaths[randomIndex];

            // Now you can use the selected sound and subtitle paths as needed.
            Debug.Log("Selected Sound Path: " + selectedSoundPath);
            Debug.Log("Selected Subtitle: " + VoiceLinesLoader.GetValueForKey(key: "OnPunch", randomIndex));
            PlayOneShot(selectedSoundPath, this.transform.position);
            SubtitleEventManager.PlaySubtitle(VoiceLinesLoader.GetValueForKey(key: "OnPunch", randomIndex));
        }
        else
        {
            Debug.Log("The if statement did not run.");
        }
        static bool ShouldRun()
        {
            // Generates a random number between 0 and 1
            float randomValue = UnityEngine.Random.value;

            // Checks if the random value is less than 0.1 (10% chance)
            return randomValue < 0.1;
        }
    }
}
