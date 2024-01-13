using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleCard : MonoBehaviour
{
    private Keyboard _keyboard;
    [SerializeField] private Transform Goal;
    private float speed;
    private float time;
    private bool startMoving;
    [SerializeField] private AudioClip musicLength;

    [SerializeField] private AnimationCurve titleSpeed;
    
    private void Start()
    {
        _keyboard = new Keyboard();
    }
    
    private void Update()
    {
        _keyboard = Keyboard.current;
        //Change to something automatic
        if (_keyboard.enterKey.wasPressedThisFrame)
        {
            print("Moving");
            startMoving = true;
        }

        if (!startMoving) return;
        speed = titleSpeed.Evaluate(time);
        time += Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Goal.transform.position, speed);
        if (transform.position == Goal.transform.position)
        {
            StartCoroutine(PlayIntro());
        }
    }
    

    private IEnumerator PlayIntro()
    {
        print("Play Music"); //Play music here
        yield return new WaitForSeconds(musicLength.length);
        //Send to main menu
    }
}
