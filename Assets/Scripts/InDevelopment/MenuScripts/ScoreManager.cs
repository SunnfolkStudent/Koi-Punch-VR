using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VHierarchy.Libs;

public class ScoreManager : MonoBehaviour
{
    public int fishPunchPoints, distancePoints, bonusPoints, hitByFish, zenPhaseOne, zenPhaseTwo, zenPoints;
    
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private GameObject textSpawner;
    [SerializeField] private GameObject multiplierVisual;
    //[SerializeField] public float moveSpeed { get; private set; } = 5f;

    public float multiplierTime;
    public bool multiplierOn;

    private void Start()
    {
        fishPunchPoints = distancePoints = bonusPoints = hitByFish = zenPhaseOne = zenPhaseTwo = zenPoints = 0;
    }

    /*Added to the beginning of each script that adds to ScoreManager
     *
     * [SerializeField] private GameObject _scoreManagerObj;
        private ScoreManager _scoreManager;
        
        private void Start()
    {
       //_scoreManagerObj = GameObject.FindGameObjectWithTag("ScoreManager");
       _scoreManager = _scoreManagerObj.GetComponent<ScoreManager>();
    }

     */
    
    /******************************************/
    
    //Successful or Failed Fish punch
    //successful punch = 5 points
    //failed punch = 1 point
    //add to fishPunchPoints int (check if multiplier on)
    public void FishPunch(bool successful)
    {
        if (successful)
        {
            if (multiplierOn)
                fishPunchPoints += 10;
            else
                fishPunchPoints += 5;
        }
        else
        {
            if (multiplierOn)
                fishPunchPoints += 2;
            else
                fishPunchPoints += 1;
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
        hitByFish -= 1;
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
        
        FloatingText(distance +" Distance!", Color.green );
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
        multiplierTime += 5;
        if(!multiplierOn)
            StartCoroutine(Multiplier());
        
        if (multiplierOn)
            pointsGiven *= 2;

        bonusPoints += pointsGiven;

        FloatingText(pointsGiven +" Points!", Color.yellow );
    }
    
    /*Function on Fish/bird/? to get points and start multiplier
     *
     * _scoreManager.BonusHit(pointsGiven);
     */
    
    /************************************************/

    //Zen mode phase 2
    //Successful punches are given points
    //points are added to zenPhaseOnePoints;

    public void ZenPhaseOne(int phaseOnePoints)
    {
        zenPhaseOne = 0;
        zenPhaseOne += phaseOnePoints;
    }

    //Zen mode phase 3
    //Successful punches are given points
    //points are added to zenPhaseTwoPoints;

    public void ZenPhaseTwo(int phaseTwoPoints)
    {
        zenPhaseTwo = 0;
        zenPhaseTwo += phaseTwoPoints;
    }

    //Zen mode phase 4
    //Zen and Velocity are calculated into points
    //points for phase four as well as all previous rounds are added into zenModePoints int;

    public void ZenEnd(int phaseThreePoints)
    {
        zenPoints += phaseThreePoints;
        zenPoints += zenPhaseTwo;
        zenPoints += zenPhaseOne;

        zenPhaseOne = 0;
        zenPhaseTwo = 0;
    }

    /*****************************************/
    
    //Floating text
    //takes number/text, location and color and creates a floating text object
    
    private void FloatingText(string msg, Color txtColor)
    {
        GameObject instantiated = Instantiate(textPrefab, textSpawner.transform.position,
            Quaternion.identity);
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
