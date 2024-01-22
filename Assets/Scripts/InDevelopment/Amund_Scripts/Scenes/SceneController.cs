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
    private Vector3 _goal = new (0, 2, 1);
    private GameObject _title;
    [SerializeField] private AnimationCurve titleSpeed;
    private float speed;
    private float time;
    private bool ReadyToStart;
    
    [SerializeField] private FadeScreenScript _fadeScreen;
    [SerializeField] private FadeScreenScript _fishScreen;
    
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

        _title = GameObject.FindWithTag("Title");
    }

    private void Update()
    {
        if (!_title)
            return;
        if (ReadyToStart)
        {
            speed = titleSpeed.Evaluate(time);
            time += Time.deltaTime;
            _title.transform.position = Vector3.MoveTowards(_title.transform.position, _goal, speed);
        }
        if (_title.transform.position == _goal && ReadyToStart)
        {
            FMODManager.instance.menuTheme.setParameterByName("PunchedThing", 1);
            Instantiate(secondStartSign);
            ReadyToStart = false;
        }
    }
    public void ChangeScenes(int scene)
    {
        StartCoroutine(ChangeLevelFish(scene));
    }
    public void StartGame()
    {
        ReadyToStart = true;
    }
    public void StartGameAfterIntro()
    {
        StartCoroutine(ChangeLevelStart());
    }

    private IEnumerator ChangeLevelStart()
    {
        _fadeScreen.FadeOut();
        yield return new WaitForSeconds(1.5f);
        CheckStartScene();
        FMODManager.instance.menuTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODManager.instance.ambientOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(Levels[startScene]);
        _fadeScreen.FadeIn();
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
        _fishScreen.FadeOut();
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Changing scenes");
        FMODManager.instance.menuTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODManager.instance.ambientOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(Levels[scene]);
        _fishScreen.FadeIn();
    }
}
