using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZenMetreManager : MonoBehaviour
{
    public static ZenMetreManager Instance;
    
    [Header("Variables for zen events")]
    public bool tripleScoreActive;
    public bool zenAttackActive;
    private float _tripleScoreTimer = 10f;
    public bool attackFieldsActive;
    private float _attackFieldsActiveTime = 11f;
    public bool timeStopActive;

    [Header("Multipliers for adding zen")]
    private float _attackFieldScoreMultiplier = 10f;
    private float _hitZenScoreMultiplier = 0.1f;
    
    [Header("Zen Metre Values")]
    public float zenMetreValue;
    public int zenLevel;
    private int _zenLevelCheckpoint;
    
    [Header("Time Stop Values")]
    private float _slowdownFactor = 0.001f;
    private float _slowdownTime = 0.5f;
    
    [Header("Particle systems and originl simulation speeds for time stop")]
    private List<ParticleSystem> _particleSystems = new List<ParticleSystem>();
    private List<float> _originalSimulationSpeeds = new List<float>();
    
    private void Awake()
    {
        zenMetreValue = 0;
        zenLevel = 0;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Update is for testing purposes only
    private void Update()
    {
        if (zenMetreValue >= 100 && zenLevel == 0)
        {
            zenLevel = 1;
            SetZenLevel();
        }
    }

    #region -- Zen Score Methods --
    //Method that adds zen to the zen metre based on score
    public void AddHitZen(float score)
    {
        zenMetreValue += score * _hitZenScoreMultiplier;
        
        if (zenMetreValue > 100)
        {
            zenMetreValue = 100;
        }

        ZenMetreVisualManager.Instance.UpdateZenBar(zenLevel, zenMetreValue);
        CheckForMaxZen();
    }
    
    public void AddAttackFieldZen(float attackFieldSize)
    {
        zenMetreValue += attackFieldSize * _attackFieldScoreMultiplier;
        
        if (zenMetreValue > 100)
        {
            zenMetreValue = 100;
        }
        
        ZenMetreVisualManager.Instance.UpdateZenBar(zenLevel, zenMetreValue);
    }
    #endregion
    
    #region -- Zen Level Methods --
    private void SetZenLevel()
    {
        switch (zenLevel)
        {
            case 0:
                LevelZero();
                break;
            case 1:
                LevelOne();
                break;
            case 2:
                LevelTwo();
                break;
            case 3:
                LevelThree();
                break;
        }
    }
    
    //Level one of zen is the start level. It is the level before anything happens with the zen.
    private void LevelZero()
    {
        if (_zenLevelCheckpoint <= 1)
            ZenMetreVisualManager.Instance.UpdateZenBar(1, 0f);
        
        ZenMetreVisualManager.Instance.UpdateZenBar(2, 0f);
        
        timeStopActive = false;
        
        Time.timeScale = 1f;
        for (int i = 0; i < _particleSystems.Count; i++)
        {
            var mainModule = _particleSystems[i].main;
            mainModule.simulationSpeed = _originalSimulationSpeeds[i];
        }
        
        //Reset music back to normal after zen mode is over
    }
    
    //Method that moves on to the second level of zen
    private void LevelOne()
    {
        _zenLevelCheckpoint = 1;
        timeStopActive = true;
        zenMetreValue = 0;
        StartCoroutine(TimeStop());
        attackFieldsActive = true;
        StartCoroutine(AttackFieldSpawnTimer());
        
        //Add music for the second level of zen
    }
    
    //Method that moves on to the third level of zen
    private void LevelTwo()
    {
        zenMetreValue = 0;
        
        //Start of level 3
        tripleScoreActive = true;
        _zenLevelCheckpoint = 1;
        StartCoroutine(TripleScoreTimer());
        
        //Add music for the third level of zen
    }
    
    //Level four is the last level of zen and is the level where you unlock your ultimate move.
    private void LevelThree()
    {
        zenMetreValue = 0;
        
        //Start of level 4
        zenAttackActive = true;
        _zenLevelCheckpoint = 2;
        ZenMetreVisualManager.Instance.ShowPromptText("Hold side button to charge punch!");
        //ControllerRumble.Instance.RightControllerRumbling(0.4f,5f);
        //ControllerRumble.Instance.LeftControllerRumbling(0.4f,5f);
        
        //Add music for the fourth level of zen
    }
    #endregion
    
    #region -- Zen Event Methods --
    private IEnumerator TripleScoreTimer()
    {
        yield return new WaitForSecondsRealtime(_tripleScoreTimer);
        
        tripleScoreActive = false;
        
        if (zenMetreValue >= 100)
        {
            zenLevel = 3;
            SetZenLevel();
        }
        else
        {
            zenLevel = 0;
            zenMetreValue = 100;
            SetZenLevel();
        }
    }
    
    private IEnumerator AttackFieldSpawnTimer()
    {
        yield return new WaitForSecondsRealtime(_attackFieldsActiveTime);
        
        attackFieldsActive = false;
        DestroyAllAttackFields();
        
        if (zenMetreValue >= 100)
        {
            zenLevel = 2;
            SetZenLevel();
        }
        else
        {
            zenLevel = 1;
            zenMetreValue = 100;
            SetZenLevel();
        }
    }
    
    private void DestroyAllAttackFields()
    {
        GameObject[] attackFields = GameObject.FindGameObjectsWithTag("AttackField");
        foreach (GameObject attackField in attackFields)
        {
            Destroy(attackField);
        }
    }

    private IEnumerator TimeStop()
    {
        _particleSystems = FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None).ToList();
        
        float currentTimeScale = Time.timeScale;
        float timePassed = 0f;

        while (timePassed < _slowdownTime)
        {
            Time.timeScale = Mathf.Lerp(currentTimeScale, _slowdownFactor, timePassed / _slowdownTime);
            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = _slowdownFactor;
        
        foreach (ParticleSystem ps in _particleSystems)
        {
            var mainModule = ps.main;
            _originalSimulationSpeeds.Add(mainModule.simulationSpeed);
            mainModule.simulationSpeed = 100.0f; // Change the speed value as needed
        }
    }
    #endregion
    
    #region -- Visual --

    public void CheckForMaxZen()
    {
        if (zenMetreValue >= 100 && zenLevel >= 2)
        {
            ZenMetreVisualManager.Instance.ShowSparkles();
        }
    }
    
    #endregion
}
