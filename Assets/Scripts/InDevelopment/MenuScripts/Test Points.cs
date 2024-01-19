using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoints : MonoBehaviour
{
    private GameObject _scoreManagerObj;
    private ScoreManager _scoreManager;
    public int pointsGiven;
    public int distance;
    public int typeOfPoint;
    public bool successPunch;
    public GameObject scoringGatePrefab;

    private void Start()
    {
       _scoreManagerObj = GameObject.FindGameObjectWithTag("ScoreManager");
       _scoreManager = _scoreManagerObj.GetComponent<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(typeOfPoint == 0)
        {
            _scoreManager.BonusHit(pointsGiven);
        }
        if(typeOfPoint == 1)
        {
            _scoreManager.DistancePoints(distance);
        }

        if (typeOfPoint == 2)
        {
            _scoreManager.HitByFish();
        }

        if (typeOfPoint == 3)
        {

            _scoreManager.FishPunch(successPunch);
        }

        if (typeOfPoint == 4)
        {
            Instantiate(scoringGatePrefab);
            gameObject.SetActive(false);
        }

        if (typeOfPoint == 5)
        {
            _scoreManager.ShowZenPoints(90);
        }
    }
}
