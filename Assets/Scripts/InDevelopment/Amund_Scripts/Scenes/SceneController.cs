using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region Definitions
    [SerializeField] private string[] Levels;
    [SerializeField] private string[] LightingForLevels;
    [SerializeField] private Transform Goal;
    [SerializeField] private GameObject Title;
    [SerializeField] private AudioClip musicLength;
    [SerializeField] private AnimationCurve titleSpeed;
    private float speed;
    private float time;
    //private bool ReadyToStart;
    private GameObject _fadeScreenObj;
    private FadeScreenScript _fadeScreen;
    
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


    private IEnumerator TitleCard()
    {
        print("Play Music"); //Play music here
        
        speed = titleSpeed.Evaluate(time);
        time += Time.deltaTime;
        Title.transform.position = Vector3.MoveTowards(Title.transform.position, Goal.transform.position, speed);

        Debug.Log("second start sign appear!");
        Instantiate(secondStartSign);
        
        yield return new WaitForSeconds(musicLength.length);

        StartCoroutine(ChangeLevel(1));

        //ReadyToStart = true;

    }

    public void ChangeScenes(int scene)
    {
        StartCoroutine(ChangeLevel(scene));
    }

    public void StartGame()
    {
        StartCoroutine(TitleCard());
    }

    private IEnumerator ChangeLevel(int scene)
    {
        _fadeScreenObj = GameObject.FindGameObjectWithTag("FadeScreen");
        _fadeScreen = _fadeScreenObj.GetComponent<FadeScreenScript>();
        _fadeScreen.FadeOut();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(Levels[scene]);
        if (scene is 1 or 2 or 3)
        {
            SceneManager.LoadScene(LightingForLevels[scene], LoadSceneMode.Additive);
        }
    }
}
