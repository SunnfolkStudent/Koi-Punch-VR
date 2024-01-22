using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class SceneController : MonoBehaviour
{
    #region Definitions
    [SerializeField] private string[] Levels;
    [SerializeField] private Transform Goal;
    [SerializeField] private GameObject Title;
    [SerializeField] private AudioClip musicLength;
    [SerializeField] private AnimationCurve titleSpeed;
    private float speed;
    private float time;
    private bool ReadyToStart;
    
    //private GameObject _fadeScreenObj;
    [SerializeField] private FadeScreenScript _fadeScreen;
    //private GameObject _fishScreenObj;
    [SerializeField] private FadeScreenScript _fishScreen;
    
    private bool canChangeScene = true;
    
    private static SceneController instance;

    private int startScene;

    [SerializeField] private GameObject secondStartSign;
    #endregion
    
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
        //Play intro music
    }

    private void Update()
    {
        if (!Title || !Goal)
            return;
        if (ReadyToStart)
        {
            speed = titleSpeed.Evaluate(time);
            time += Time.deltaTime;
            Title.transform.position = Vector3.MoveTowards(Title.transform.position, Goal.transform.position, speed);
        }
        if (Title.transform.position == Goal.transform.position && ReadyToStart)
        {
            // TODO FMODManager.instance.menuTheme.setParameterByName("PunchedThing", 1);
            Instantiate(secondStartSign);
            ReadyToStart = false;
        }
    }


    private IEnumerator TitleCard()
    {
        print("Play Music"); //Play music here

        ReadyToStart = true;
        
        //yield return new WaitForSeconds(musicLength.length);
        yield return new WaitForSeconds(10);

        if(canChangeScene)
            StartCoroutine(ChangeLevelStart());
    }

    public void ChangeScenes(int scene)
    {
        if(canChangeScene)
            StartCoroutine(ChangeLevelFish(scene));
    }

    public void StartGame()
    {
        StartCoroutine(TitleCard());
    }

    public void StartGameAfterIntro()
    {
        StartCoroutine(ChangeLevelStart());
    }

    private IEnumerator ChangeLevelStart()
    {
        Debug.Log("change scene");
        canChangeScene = false;
        //_fadeScreenObj = GameObject.FindGameObjectWithTag("FadeScreen");
        //_fadeScreen = _fadeScreenObj.GetComponent<FadeScreenScript>();
        _fadeScreen.FadeOut();
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Changing scenes");
        CheckStartScene();
        // TODO FMODManager.instance.menuTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        // TODO FMODManager.instance.ambientOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(Levels[startScene]);
        _fadeScreen.FadeIn();
        canChangeScene = true;
    }

    private void CheckStartScene()
    {
        if (PlayerPrefs.GetInt("HighScoreLevelThree") > 0)
        {
            startScene = 1;
        }
        else if(PlayerPrefs.GetInt("HighScoreLevelTwo") > 0)
        {
            startScene = 3;
        }
        else if (PlayerPrefs.GetInt("HighScoreLevelOne") > 0)
        {
            startScene = 2;
        }
        else
        {
            startScene = 1;
        }
    }

    private IEnumerator ChangeLevelFish(int scene)
    {
        canChangeScene = false;
        //_fishScreenObj = GameObject.FindGameObjectWithTag("FishTransition");
        //_fishScreen = _fishScreenObj.GetComponent<FadeScreenScript>();
        _fishScreen.FadeOut();
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Changing scenes");
        // TODO FMODManager.instance.menuTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        // TODO FMODManager.instance.ambientOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(Levels[scene]);
        _fishScreen.FadeIn();
        canChangeScene = true;
    }
}
