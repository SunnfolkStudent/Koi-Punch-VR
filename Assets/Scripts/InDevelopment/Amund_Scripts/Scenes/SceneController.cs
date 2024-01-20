using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private GameObject _fadeScreenObj;
    private FadeScreenScript _fadeScreen;
    private bool canChangeScene = true;
    
    private static SceneController instance;

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
        
        _fadeScreenObj = GameObject.FindGameObjectWithTag("FadeScreen");
        _fadeScreen = _fadeScreenObj.GetComponent<FadeScreenScript>();
    }

    private void Update()
    {
        if (ReadyToStart)
        {
            speed = titleSpeed.Evaluate(time);
            time += Time.deltaTime;
            Title.transform.position = Vector3.MoveTowards(Title.transform.position, Goal.transform.position, speed);
        }
    }


    private IEnumerator TitleCard()
    {
        print("Play Music"); //Play music here

        ReadyToStart = true;
        
        Debug.Log("second start sign appear!");

        yield return new WaitForSeconds(5);
        
        Instantiate(secondStartSign);
        
        //yield return new WaitForSeconds(musicLength.length - 5);
        yield return new WaitForSeconds(10);

        ChangeScenes(1);
    }

    public void ChangeScenes(int scene)
    {
        if(canChangeScene)
            StartCoroutine(ChangeLevel(scene));
    }

    public void StartGame()
    {
        StartCoroutine(TitleCard());
    }

    private IEnumerator ChangeLevel(int scene)
    {
        canChangeScene = false;
        _fadeScreenObj = GameObject.FindGameObjectWithTag("FadeScreen");
        _fadeScreen = _fadeScreenObj.GetComponent<FadeScreenScript>();
        _fadeScreen.FadeOut();
        ReadyToStart = false;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(Levels[scene]);
        canChangeScene = true;
    }
}
