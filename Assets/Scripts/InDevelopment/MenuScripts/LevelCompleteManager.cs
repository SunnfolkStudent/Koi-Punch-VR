using System.Collections;
using UnityEngine;
using TMPro;


public class LevelCompleteManager : MonoBehaviour
{
   private GameObject _scoreManagerObj;
   private ScoreManager _scoreManager;
   
   [SerializeField] private int punchPoints, distancePoints, bonusPoints, zenPoints, penaltyPoints;
   private int punchPointsEnd, distancePointsEnd, bonusPointsEnd, zenPointsEnd, penaltyPointsEnd, totalPoints;
   [SerializeField] private TMP_Text punchScore, distanceScore, bonusScore, zenScore, penaltyScore, totalScore;
   private bool gameOver;
   private int growthRate = 1;
   public TMP_Text highScoreText;

   [SerializeField] private GameObject _newHighScoreParent;
   [SerializeField] private TMP_Text _newHighScoreText;
   private bool scoringOver;

   [SerializeField] private GameObject _oldHighScorePanel;
   private Animator _oldHighScoreAnimator;

   [SerializeField] private int arenaNumber;
   private int highScore;

   private void Start()
   {
      if (arenaNumber == 1)
      {
         /*if (PlayerPrefs.HasKey("HighScoreLevelOne"))
         {
            highScore = PlayerPrefs.GetInt("HighScoreLevelOne");
         }
         else*/
         {
            PlayerPrefs.SetInt("HighScoreLevelOne", 0);
            highScore = PlayerPrefs.GetInt("HighScoreLevelOne");
         }

         highScoreText.text = highScore.ToString("0");
      }

      if (arenaNumber == 2)
      {
         if (PlayerPrefs.HasKey("HighScoreLevelTwo"))
         {
            highScore = PlayerPrefs.GetInt("HighScoreLevelTwo");
         }
         else
         {
            PlayerPrefs.SetInt("HighScoreLevelTwo", 0);
            highScore = PlayerPrefs.GetInt("HighScoreLevelTwo");
         }
         
         highScoreText.text = highScore.ToString("0");
      }

      if (arenaNumber == 3)
      {
         if (PlayerPrefs.HasKey("HighScoreLevelThree"))
         {
            highScore = PlayerPrefs.GetInt("HighScoreLevelThree");
         }
         else
         {
            PlayerPrefs.SetInt("HighScoreLevelThree", 0);
            highScore = PlayerPrefs.GetInt("HighScoreLevelThree");
         }
         
         highScoreText.text = highScore.ToString("0");
      }

      _oldHighScoreAnimator = _oldHighScorePanel.GetComponent<Animator>();
      
      _scoreManagerObj = GameObject.FindGameObjectWithTag("ScoreManager");
      _scoreManager = _scoreManagerObj.GetComponent<ScoreManager>();

      punchPoints = _scoreManager.fishPunchPoints;
      distancePoints = _scoreManager.distancePoints;
      bonusPoints = _scoreManager.bonusPoints;
      zenPoints = _scoreManager.zenPoints;
      penaltyPoints = _scoreManager.hitByFish;
      totalPoints = 0;
      
      if(!scoringOver)
         InvokeRepeating("CalculateScore", 5f, .005f);
   }

   private void Update()
   {
      
      if(!scoringOver)
      {
         punchScore.text = punchPointsEnd.ToString("0");
         distanceScore.text = distancePointsEnd.ToString("0");
         bonusScore.text = bonusPointsEnd.ToString("0");
         zenScore.text = zenPointsEnd.ToString("0");
         penaltyScore.text = penaltyPointsEnd.ToString("0");
         totalScore.text = totalPoints.ToString("0");
      }
      
      if(totalPoints < 1000)
         totalScore.color = Color.gray;
      else if (totalPoints < 2000)
         totalScore.color = Color.red;
      else if (totalPoints < 5000)
         totalScore.color = Color.green;
      else if(totalPoints < 7000)
         totalScore.color = Color.blue;
      else if (totalPoints < 9000)
         totalScore.color = Color.magenta;
      else
         totalScore.color = Color.yellow;

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
      else if (totalPoints > highScore)
      {
         StartCoroutine(NewHighScore());
         scoringOver = true;
      }
   }

   private IEnumerator NewHighScore()
   {
      _oldHighScoreAnimator.SetTrigger("NewHighScore");
      yield return new WaitForSeconds(3);
      _newHighScoreParent.SetActive(true);
      if (arenaNumber == 1)
      {
         PlayerPrefs.SetInt("HighScoreLevelOne", totalPoints);
         _newHighScoreText.text = PlayerPrefs.GetInt("HighScoreLevelOne").ToString("0");
      }

      if (arenaNumber == 2)
      {
         PlayerPrefs.SetInt("HighScoreLevelTwo", totalPoints);
         _newHighScoreText.text = PlayerPrefs.GetInt("HighScoreLevelTwo").ToString("0");
      }

      if (arenaNumber == 3)
      {
         PlayerPrefs.SetInt("HighScoreLevelThree", totalPoints);
         _newHighScoreText.text = PlayerPrefs.GetInt("HighScoreLevelThree").ToString("0");
      }
   }
}
