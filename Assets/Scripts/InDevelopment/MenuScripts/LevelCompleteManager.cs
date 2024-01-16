using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.VFX.UI;

public class NewBehaviourScript : MonoBehaviour
{
   private GameObject _scoreManagerObj;
   private ScoreManager _scoreManager;
   
   public int punchPoints, distancePoints, bonusPoints, zenPoints, penaltyPoints;
   private int punchPointsEnd, distancePointsEnd, bonusPointsEnd, zenPointsEnd, penaltyPointsEnd, totalPoints;
   [SerializeField] private TMP_Text punchScore, distanceScore, bonusScore, zenScore, penaltyScore, totalScore;
   private bool gameOver;
   private int growthRate = 1;
   public TMP_Text highScoreText;

   [SerializeField] private GameObject _newHighScorePrefab;
   private TMP_Text _newHighScoreText;
   private bool scoringOver;
   private Animator _animator;

   //public float numberSpeed = 0.01f;

   private void Start()
   {
      /*if (PlayerPrefs.HasKey("HighScore"))
      {
         PlayerPrefs.GetInt("HighScore");
      }
      else
      {
         PlayerPrefs.SetInt("HighScore", 0);
      }*/
      
      PlayerPrefs.SetInt("HighScore", 200);
      
      highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString("0");

      _animator = GetComponent<Animator>();
      
      _scoreManagerObj = GameObject.FindGameObjectWithTag("ScoreManager");
      _scoreManager = _scoreManagerObj.GetComponent<ScoreManager>();

      punchPoints = _scoreManager.fishPunchPoints;
      distancePoints = _scoreManager.distancePoints;
      bonusPoints = _scoreManager.bonusPoints;
      zenPoints = _scoreManager.zenPoints;
      penaltyPoints = _scoreManager.hitByFish;
      totalPoints = 0;
      
      if(!scoringOver)
         InvokeRepeating("CalculateScore", 1f, 0.01f);
   }

   private void Update()
   {
      punchScore.text = punchPointsEnd.ToString("0");
      distanceScore.text = distancePointsEnd.ToString("0");
      bonusScore.text = bonusPointsEnd.ToString("0");
      zenScore.text = zenPointsEnd.ToString("0");
      penaltyScore.text = penaltyPointsEnd.ToString("0");
      totalScore.text = totalPoints.ToString("0");
      
      if(totalPoints < 1000)
         totalScore.color = Color.gray;
      else if (totalPoints < 3000)
         totalScore.color = Color.red;
      else if (totalPoints < 5000)
         totalScore.color = Color.green;
      else if(totalPoints < 7000)
         totalScore.color = Color.blue;
      else if (totalPoints >= 9000)
         totalScore.color = Color.magenta;

   }

   private void CalculateScore()
   {
      if (punchPoints != punchPointsEnd && punchPoints > punchPointsEnd)
      {
         punchPointsEnd += growthRate;
         totalPoints += growthRate;
         
         //TODO play points going up audio
      }
      else if (distancePoints != distancePointsEnd && distancePoints > distancePointsEnd)
      {
         distancePointsEnd += growthRate;
         totalPoints += growthRate;
         
         //TODO play points going up audio
      }
      else if (bonusPoints != bonusPointsEnd && bonusPoints > bonusPointsEnd)
      {
         bonusPointsEnd += growthRate;
         totalPoints += growthRate;
         
         //TODO play points going up audio
      }
      else if (zenPoints != zenPointsEnd && zenPoints > zenPointsEnd)
      {
         zenPointsEnd += growthRate;
         totalPoints += growthRate;
         
         //TODO play points going up audio
      }
      else if (penaltyPoints != penaltyPointsEnd && penaltyPoints < penaltyPointsEnd)
      {
         penaltyPointsEnd -= growthRate;
         totalPoints -= growthRate;
         
         //TODO play points going up audio
      }
      else if (totalPoints > PlayerPrefs.GetInt("HighScore"))
      {
         StartCoroutine(NewHighScore());
         scoringOver = true;
      }
   }

   private IEnumerator NewHighScore()
   {
      PlayerPrefs.SetInt("HighScore", totalPoints);
      _animator.SetTrigger("NewHighScore");
      yield return new WaitForSeconds(3);
      Instantiate(_newHighScorePrefab);
   }
   
   //new high score function
   // make last high score fall
   //new high score with "New high score" panel fall
}
