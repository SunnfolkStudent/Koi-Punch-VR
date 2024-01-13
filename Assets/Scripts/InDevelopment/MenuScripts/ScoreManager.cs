using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int fishPunchPoints;
    [SerializeField] private int distancePoints;
    [SerializeField] private int specialPoints;
    [SerializeField] private int hitByFish;

    private Vector3 distancePointsLocation = new Vector3(0, 0, 0);

    public float multiplierTime;
    public bool multiplierOn;


    //Successful or Failed Fish punch
    //successful punch = 5 points
    //failed punch = 1 point
    //add to fishPunchPoints int (check if multiplier on)
    private void FishPunch(bool successful)
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

    //Player hit by fish
    //each hit is subtracted from hitByFish int
    private void HitByFish()
    {
        hitByFish -= 1;
    }

    //Fish hitting ground after being hit + distance from player
    //distance from player converted into points (check if multiplier on)
    //distance number, location and color of text sent to floating text
    //points added to distancePoints int
    private void DistancePoints(float distance)
    {
        if (multiplierOn)
            distance *= 2;
        
        FloatingText(distance +" Distance!",distancePointsLocation, Color.blue );

        distance += distancePoints;
    }

    //Fish hit bird/other multiplier started
    //multiplier bool?/IEnumerator? turned on (added visuals turned on)
    //points for hitting other calculated (check if multiplier on)
    //points number, location and color of text sent to floating text
    //points added to specialPoints int
    private void SpecialHit(int typeObjectHit)
    {
        multiplierTime += 20;
        StartCoroutine(Multiplier());
        
    }
    //Test
    private void OnTriggerEnter(Collider other)
    {
        multiplierTime += 20;
        StartCoroutine(Multiplier());
    }

    //Zen mode phase 2
    //Successful punches are given points
    //points are added to zenPhaseOnePoints;

    //Zen mode phase 3
    //Successful punches are given points
    //points are added to zenPhaseTwoPoints;

    //Zen mode phase 4
    //Zen and Velocity are calculated into points
    //points for phase four as well as all previous rounds are added into zenModePoints int;

    //Floating text
    //takes number/text, location and color and creates a floating text object
    private void FloatingText(string msg, Vector3 location, Color color)
    {
        
    }
    
    //multiplier
    //turn multiplier bool true
    //wait for multiplier timer to go down
    //turn multiplier bool false

    private IEnumerator Multiplier()
    {
        multiplierOn = true;
        while(multiplierTime > 0)
        {
            //print("Multiplier Time : " + multiplierTime);
            multiplierTime -= Time.deltaTime;
            yield return null;
        }

        multiplierTime = 0;
        multiplierOn = false;
    }
        
    //multiplier visuals
}
