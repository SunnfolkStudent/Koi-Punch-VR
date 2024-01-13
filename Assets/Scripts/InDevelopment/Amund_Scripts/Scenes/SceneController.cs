using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string IntroScene;
    [SerializeField] private Transform Goal;
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject IntroTrigger;
    private float speed;
    private float time;
    private bool startMoving;
    [SerializeField] private AudioClip musicLength;
    [SerializeField] private AnimationCurve titleSpeed;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(IntroScene, LoadSceneMode.Additive);
    }

    private void Update()
    {
        
        if (IntroTrigger == null)
        {
            print("Moving");
            startMoving = true;
        }

        if (!startMoving) return;
        speed = titleSpeed.Evaluate(time);
        time += Time.deltaTime;
        Title.transform.position = Vector3.MoveTowards(Title.transform.position, Goal.transform.position, speed);
        if (Title.transform.position == Goal.transform.position)
        {
            StartCoroutine(PlayIntro());
        }
    }
    
    private IEnumerator PlayIntro()
    {
        print("Play Music"); //Play music here
        yield return new WaitForSeconds(musicLength.length);
        SceneManager.UnloadSceneAsync(IntroScene);
    }
}
