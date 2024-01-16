using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
   private GameObject _scoreManagerObj;
   private ScoreManager _scoreManager;
   
   public int punchPoints, distancePoints, bonusPoints, zenPoints, penaltyPoints;
   private int punchPointsEnd, distancePointsEnd, bonusPointsEnd, zenPointsEnd, penaltyPointsEnd, totalPoints;
   [SerializeField] private TMP_Text punchScore, distanceScore, bonusScore, zenScore, penaltyScore, totalScore;
   private bool gameOver;
   private int growthRate = 1;
   public int highScore = 300;
   public TMP_Text highScoreText;

   private void Start()
   {
      _scoreManagerObj = GameObject.FindGameObjectWithTag("ScoreManager");
      _scoreManager = _scoreManagerObj.GetComponent<ScoreManager>();

      punchPoints = _scoreManager.fishPunchPoints;
      distancePoints = _scoreManager.distancePoints;
      bonusPoints = _scoreManager.bonusPoints;
      zenPoints = _scoreManager.zenPoints;
      penaltyPoints = _scoreManager.hitByFish;
      totalPoints = 0;
      
      InvokeRepeating("CalculateScore", 5f, 0.025f);
   }

   private void Update()
   {
      punchScore.text = punchPointsEnd.ToString("0");
      distanceScore.text = distancePointsEnd.ToString("0");
      bonusScore.text = bonusPointsEnd.ToString("0");
      zenScore.text = zenPointsEnd.ToString("0");
      penaltyScore.text = penaltyPointsEnd.ToString("0");
      totalScore.text = totalPoints.ToString("0");
      highScoreText.text = highScore.ToString("0");

   }

   private void CalculateScore()
   {
      if (punchPoints != punchPointsEnd && punchPoints > punchPointsEnd)
      {
         punchPointsEnd += growthRate;
         totalPoints += growthRate;
      }
      else if (distancePoints != distancePointsEnd && distancePoints > distancePointsEnd)
      {
         distancePointsEnd += growthRate;
         totalPoints += growthRate;
      }
      else if (bonusPoints != bonusPointsEnd && bonusPoints > bonusPointsEnd)
      {
         bonusPointsEnd += growthRate;
         totalPoints += growthRate;
      }
      else if (zenPoints != zenPointsEnd && zenPoints > zenPointsEnd)
      {
         zenPointsEnd += growthRate;
         totalPoints += growthRate;
      }
      else if (penaltyPoints != penaltyPointsEnd && penaltyPoints < penaltyPointsEnd)
      {
         penaltyPointsEnd -= growthRate;
         totalPoints -= growthRate;
      }
      else if (totalPoints > highScore)
      {
         highScore = totalPoints;
      }
   }
   
   //new high score function
   // make last high score fall
   //new high score with "New high score" panel fall
}
