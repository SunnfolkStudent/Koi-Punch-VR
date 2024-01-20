using System;
using System.Collections;
using System.Collections.Generic;
using FinalScripts.Fish;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int fishPunchPoints, distancePoints, bonusPoints, penaltyPoints, zenPoints;
    
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private GameObject textSpawner;
    [SerializeField] private GameObject multiplierVisual;
    //[SerializeField] public float moveSpeed { get; private set; } = 5f;

    public float multiplierTime;
    public bool multiplierOn;

    private int visableZenPoints;
    private bool zenModeOn;
    [SerializeField] private GameObject zenTextPrefab;
    private TMP_Text zenText;

    private void Start()
    {
        fishPunchPoints = distancePoints = bonusPoints = penaltyPoints = zenPoints = 0;
        zenModeOn = false;
        //EventManager.BossDefeatedTotalScore += ZenEnd();
    }

    /*Added to the beginning of each script that adds to ScoreManager
     *
     * private GameObject _scoreManagerObj;
        private ScoreManager _scoreManager;
        
        private void Start()
    {
       _scoreManagerObj = GameObject.FindGameObjectWithTag("ScoreManager");
       _scoreManager = _scoreManagerObj.GetComponent<ScoreManager>();
    }

     */
    
    /******************************************/
    
    //Successful or Failed Fish punch
    //successful punch = 10 points
    //failed punch = ? point
    //add to fishPunchPoints int (check if multiplier on)
    public void FishPunch(bool successful)
    {
        if (successful)
        {
            if (multiplierOn)
                fishPunchPoints += 20;
            else
                fishPunchPoints += 10;
        }
        else
        {
            penaltyPoints -= 5;
        }
    }
    
    /*Function on fish/fist to get successful/unsuccessful punch points
     *
     * _scoreManager.FishPunch(success bool);
     * 
     */

    /*********************************************/
    
    //Player hit by fish
    //each hit is subtracted from hitByFish int
    public void HitByFish()
    {
        penaltyPoints -= 1;
    }
    
    /*Function on Fish/body to show hit by fish
     *
     * _scoreManager.HitByFish();
     */
    
    /*********************************************/

    //Fish hitting ground after being hit + distance from player
    //distance from player converted into points (check if multiplier on)
    //distance number, location and color of text sent to floating text
    //points added to distancePoints int
    public void DistancePoints(int distance)
    {
        if (multiplierOn)
            distance *= 2;

        distancePoints += distance;
        
        FloatingText("+" + distance, Color.green);
    }
    
    /*Function on Fish? script to get distance points
     *
     *_scoreManager.DistancePoints(distance);
     * 
     */
    
    /*************************************************/

    //Fish hit bird/other multiplier started
    //multiplier multiplier IEnumerator turned on (added visuals turned on)
    //points for hitting other calculated (check if multiplier on)
    //points number, location and color of text sent to floating text
    //points added to bonusPoints int
    public void BonusHit(int pointsGiven)
    {
        multiplierTime += 10;
        if(!multiplierOn)
            StartCoroutine(Multiplier());
        
        if (multiplierOn)
            pointsGiven *= 2;

        bonusPoints += pointsGiven;

        FloatingText("+" + pointsGiven, new Color(255, 215, 0) );
    }
    
    /*Function on Fish/bird/? to get points and start multiplier
     *
     * _scoreManager.BonusHit(pointsGiven);
     */
    
    /************************************************/

    //Zen points are added to the zen score

    public void ZenEnd(int allZenPoints)
    {
        zenPoints += allZenPoints;
    }
    
    /*****************************************/
    
    //Floating text
    //takes number/text, location and color and creates a floating text object
    
    private void FloatingText(string msg, Color txtColor)
    {
        GameObject instantiated = Instantiate(textPrefab, textSpawner.transform.position,
            textSpawner.transform.rotation, textSpawner.transform);
        TextMeshPro FloatingTxt = instantiated.GetComponent<TextMeshPro>();
        FloatingTxt.text = msg;
        FloatingTxt.color = txtColor;
    }
    
    /*********************************************/
    
    //multiplier
    //turn multiplier bool true
    //wait for multiplier timer to go down
    //turn multiplier bool false

    private IEnumerator Multiplier()
    {
        multiplierOn = true;
        multiplierVisual.SetActive(true);
        while(multiplierTime > 0)
        {
            multiplierTime -= Time.deltaTime;
            yield return null;
        }
        
        multiplierTime = 0;
        multiplierOn = false;
        multiplierVisual.SetActive(false);
    }
        
    //multiplier visuals
}
