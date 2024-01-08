using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZenMetreManager : MonoBehaviour
{
    public static ZenMetreManager Instance;
    
    public bool tripleScoreActive;
    public bool zenAttackActive;
    private float _tripleScoreTimer = 10f;
    public bool attackFieldsActive;
    private float _attackFieldsActiveTime = 11f;
    
    public float zenMetreValue;
    public int zenLevel;
    
    private float _slowdownFactor = 0.001f;
    private float _slowdownTime = 0.5f;
    
    private List<ParticleSystem> _particleSystems = new List<ParticleSystem>();
    private List<float> _originalSimulationSpeeds = new List<float>();
    
    private void Awake()
    {
        zenMetreValue = 0;
        zenLevel = 1;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Update()
    {
        //If the zen metre is full, move on to the next level of zen
        if (zenMetreValue >= 100)
        {
            zenLevel++;
            SetZenLevel();
        }
    }
    
    #region -- Zen Score Methods --
    //Method that adds zen to the zen metre based on the velocity of the fist
    public void AddHitZen(float score)
    {
        zenMetreValue += score;
    }
    
    public void AddAttackFieldZen(float attackFieldSize)
    {
        zenMetreValue += attackFieldSize;
    }
    #endregion
    
    #region -- Zen Level Methods --
    private void SetZenLevel()
    {
        switch (zenLevel)
        {
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
    private void LevelOne()
    {
        Time.timeScale = 1f;
        for (int i = 0; i < _particleSystems.Count; i++)
        {
            var mainModule = _particleSystems[i].main;
            mainModule.simulationSpeed = _originalSimulationSpeeds[i];
        }
        zenMetreValue = 0;
    }
    
    //Method that moves on to the second level of zen
    private void LevelTwo()
    {
        zenMetreValue = 0;
        StartCoroutine(TimeStop());
        attackFieldsActive = true;
        StartCoroutine(AttackFieldSpawnTimer());
    }
    
    //Method that moves on to the third level of zen
    private void LevelThree()
    {
        StopCoroutine(AttackFieldSpawnTimer());
        attackFieldsActive = false;
        DestroyAllAttackFields();
        zenMetreValue = 0;
        tripleScoreActive = true;
        StartCoroutine(TripleScoreTimer());
    }
    
    //Level four is the last level of zen and is the level where you unlock your ultimate move.
    private void LevelFour()
    {
        zenMetreValue = 0;
        zenAttackActive = true;
    }
    #endregion
    
    #region -- Zen Event Methods --
    private IEnumerator TripleScoreTimer()
    {
        yield return new WaitForSecondsRealtime(_tripleScoreTimer);
        tripleScoreActive = false;
        if (zenMetreValue >= 100)
        {
            zenLevel++;
            LevelFour();
        }
        else
        {
            zenLevel = 1;
            SetZenLevel();
        }
    }
    
    private IEnumerator AttackFieldSpawnTimer()
    {
        yield return new WaitForSecondsRealtime(_attackFieldsActiveTime);
        attackFieldsActive = false;
        zenLevel = 1;
        SetZenLevel();
        DestroyAllAttackFields();
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
        _particleSystems = FindObjectsOfType<ParticleSystem>().ToList();
        
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
}
