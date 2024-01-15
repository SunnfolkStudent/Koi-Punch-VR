using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region Definitions
    [SerializeField] private string IntroScene;
    [SerializeField] private string[] Levels;
    [SerializeField] private string[] LightingForLevels;
    [SerializeField] private Transform Goal;
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject IntroTrigger;
    [SerializeField] private AudioClip musicLength;
    [SerializeField] private AnimationCurve titleSpeed;
    private float speed;
    private float time;
    public static int LevelSelected = 5;
    private GameObject _fadeScreenObj;
    private FadeScreenScript _fadeScreen;
    #endregion
    
    

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _fadeScreenObj = GameObject.FindGameObjectWithTag("FadeScreen");
        _fadeScreen = _fadeScreenObj.GetComponent<FadeScreenScript>();
        SceneManager.LoadScene(IntroScene, LoadSceneMode.Additive);
    }

    private void Update()
    {
        #region TitleCard
        if (IntroTrigger == null)
        {
            speed = titleSpeed.Evaluate(time);
            time += Time.deltaTime;
            Title.transform.position = Vector3.MoveTowards(Title.transform.position, Goal.transform.position, speed);
        }

        if (Title.transform.position == Goal.transform.position)
        {
            StartCoroutine(PlayIntro());
        }
        

        #endregion

        if (LevelSelected == 5) return;
        StartCoroutine(ChangeLevel());
        LevelSelected = 5;
    }
    
    private IEnumerator PlayIntro()
    {
        print("Play Music"); //Play music here
        yield return new WaitForSeconds(musicLength.length);
        SceneManager.UnloadSceneAsync(IntroScene);
        //Start Main Menu Stuff here
    }

    private IEnumerator ChangeLevel()
    {
        _fadeScreenObj = GameObject.FindGameObjectWithTag("FadeScreen");
        _fadeScreen.FadeOut();
        yield return new WaitForSeconds(_fadeScreen._fadeDuration);
        SceneManager.LoadScene(Levels[LevelSelected]);
        if (LevelSelected is 1 or 2 or 3)
        {
            SceneManager.LoadScene(LightingForLevels[LevelSelected], LoadSceneMode.Additive);
        }
    }
}
