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
    
    [SerializeField] private GameObject _fadeScreen;
    private Animator _fadeScreenAnim;
    [SerializeField] private GameObject _fishScreen;
    private Animator _fishScreenAnim;
    [SerializeField] private GameObject _background;
    
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
        _fadeScreenAnim = _fadeScreen.GetComponent<Animator>();
        _fishScreenAnim = _fishScreen.GetComponent<Animator>();
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
            //FMODManager.instance.menuTheme.setParameterByName("PunchedThing", 1);
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
        StartCoroutine("StartMenuMusic");
    }
    public void StartGameAfterIntro()
    {
        StartCoroutine(ChangeLevelStart());
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
    private IEnumerator ChangeLevelStart()
    {
        _fadeScreen.SetActive(true);
        _fadeScreenAnim.Play("CircleTransitionAnimationExit");
        yield return new WaitForSeconds(3f);
        CheckStartScene();
        FMODManager.instance.menuTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODManager.instance.ambientOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(Levels[startScene]);
        _fadeScreenAnim.Play("CircleTransitionAnimationStart");
    }
    private IEnumerator ChangeLevelFish(int scene)
    {
        //_fishScreen.SetActive(true);
        _fishScreenAnim.Play("FishTransitionAnimationExit");
        yield return new WaitForSeconds(3f);
        Debug.Log("Changing scenes");
        FMODManager.instance.menuTheme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODManager.instance.ambientOne.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(Levels[scene]);
        //_background.SetActive(false);
        //_background.SetActive(true);
        _fishScreenAnim.Play("FishTransitionAnimationStart");
    }

    private IEnumerator StartMenuMusic()
    {
        yield return new WaitForSeconds(.9f);
        FMODManager.instance.menuTheme.setParameterByName("PunchedThing", 1);
    }
}
