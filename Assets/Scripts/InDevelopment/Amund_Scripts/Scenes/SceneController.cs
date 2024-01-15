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
    [SerializeField] private float fadeTime;
    [SerializeField] private AudioClip musicLength;
    [SerializeField] private AnimationCurve titleSpeed;
    private float speed;
    private float time;
    public static int LevelSelected;
    
    #endregion
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
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
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(Levels[LevelSelected]);
        SceneManager.LoadScene(LightingForLevels[LevelSelected], LoadSceneMode.Additive);
    }
}
