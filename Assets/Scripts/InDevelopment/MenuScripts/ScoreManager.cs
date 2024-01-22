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

    private int _distanceConverted;

    [SerializeField] private float multiplierTime;
    [SerializeField] private bool multiplierOn;

    private void Start()
    {
        fishPunchPoints = distancePoints = bonusPoints = penaltyPoints = zenPoints = 0;
        //EventManager.BossDefeatedTotalScore += ZenEnd;
        EventManager.FishScore += FishPunch;
    }
    public void FishPunch(float distance, bool successFullPunch)
    {
        if (successFullPunch)
        {
            fishPunchPoints += 10;
        }
        else
        {
            penaltyPoints -= 5;
        }

        //penalties for bad distance, bad hit, or both?
        
        _distanceConverted = (int)distance;
        
        distancePoints += _distanceConverted;
        
        FloatingText("+" + _distanceConverted, Color.green);
    }
    public void HitByFish(int minusScore)
    {
        penaltyPoints -= minusScore;
    }
    public void BonusHit(int pointsGiven)
    {
        bonusPoints += pointsGiven;

        FloatingText("+" + pointsGiven, new Color32(255, 215, 0,255) );
    }
    public void ZenEnd(int allZenPoints)
    {
        zenPoints += allZenPoints;
    }
    private void FloatingText(string msg, Color txtColor)
    {
        GameObject instantiated = Instantiate(textPrefab, textSpawner.transform.position,
            textSpawner.transform.rotation, textSpawner.transform);
        TextMeshPro FloatingTxt = instantiated.GetComponent<TextMeshPro>();
        FloatingTxt.text = msg;
        FloatingTxt.color = txtColor;
    }
    /*private IEnumerator Multiplier()
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
    }*/
        
    //multiplier visuals
}
