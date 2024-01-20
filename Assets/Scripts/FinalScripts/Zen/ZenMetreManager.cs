using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FinalScripts.Fish;
using UnityEngine;

public class ZenMetreManager : MonoBehaviour
{
    public static ZenMetreManager Instance;
    
    [Header("Variables for zen events")]
    [HideInInspector] public bool zenAttackActive;
    private float _tripleScoreTimer = 10f;
    private float _attackFieldsActiveTime = 15f;
    
    [Header("Zen Metre Values")]
    public float zenMetreValue;
    public int zenLevel;
    [HideInInspector] public int zenLevelCheckpoint;
    private bool _zenBossSpawnInvoked;
    [SerializeField] private float zenFromWeakpoint = 10f;
    
    [Header("Time Stop Values")]
    private float _slowdownFactor = 0.001f;
    private float _slowdownTime = 0.1f;
    
    [Header("Particle systems and original simulation speeds for time stop")]
    private List<ParticleSystem> _particleSystems = new List<ParticleSystem>();
    private List<float> _originalSimulationSpeeds = new List<float>();
    
    [Header("Prefab")] 
    [SerializeField] private GameObject bossPrefab;
    
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

    private void Start()
    {
        EventManager.BossDefeated += ResetTime;
        EventManager.StartBossPhase0 += ResetTime;
        EventManager.StartBossPhase0 += LevelZero;
        EventManager.SpawnBoss += SpawnBoss;  
        EventManager.BossPhase0Completed += StopTime;
        EventManager.BossPhase0Completed += Phase0Over;
        EventManager.StartBossPhase1 += LevelOne;
        EventManager.StartBossPhase2 += LevelTwo;
        EventManager.StartBossPhase3 += LevelThree;
    }

    
    private void Update()
    {
        AddHitZen(1);
        if (zenMetreValue >= 100)
        {
            zenLevel++;
        }
        
        if (zenMetreValue >= 100 && zenLevel == 0 && !_zenBossSpawnInvoked)
        {
            _zenBossSpawnInvoked = true;
            EventManager.SpawnBoss.Invoke();
        }
    }

    private void SpawnBoss()
    {
        Instantiate(bossPrefab);
    }
    
    private void Phase0Over()
    {
    }

    #region -- Zen Score Methods --
    //Method that adds zen to the zen metre
    public void AddHitZen(float zen)
    {
        zenMetreValue += zen;
        
        if (zenMetreValue > 100)
        {
            zenMetreValue = 100;
        }

        InternalZenEventManager.updateVisualZenBar.Invoke();
        CheckForMaxZen();
    }
    
    public void AddAttackFieldZen()
    {
        zenMetreValue += zenFromWeakpoint;
        
        if (zenMetreValue > 100)
        {
            zenMetreValue = 100;
        }
        
        InternalZenEventManager.updateVisualZenBar.Invoke();
    }
    
    public void RemoveZen(float zen)
    {
        zenMetreValue -= zen;
        
        if (zenMetreValue < 0)
        {
            zenMetreValue = 0;
        }
        
        InternalZenEventManager.updateVisualZenBar.Invoke();
    }
    #endregion
    
    #region -- Zen Level Methods --
    
    //Level zero of zen is the start level. It is the level before anything happens with the zen.
    private void LevelZero()
    {
        InternalZenEventManager.updateVisualZenBar.Invoke();
    }
    
    //Method that moves on to the second level of zen
    private void LevelOne()
    {
        Debug.Log("Level1");
        zenLevel = 1;
        zenLevelCheckpoint = 1;
        zenMetreValue = 0;
        InternalZenEventManager.updateVisualZenBar.Invoke();
        StartCoroutine(AttackFieldSpawnTimer());
    }
    
    //Method that moves on to the third level of zen
    private void LevelTwo()
    {
        zenMetreValue = 0;
        zenLevel = 2;
        zenLevelCheckpoint = 2;
        InternalZenEventManager.updateVisualZenBar.Invoke();
        //Start of level 2
        StartCoroutine(TripleScoreTimer());
    }
    
    //Level four is the last level of zen and is the level where you unlock your ultimate move.
    private void LevelThree()
    {
        zenMetreValue = 0;
        
        //Start of level 3
        zenAttackActive = true;
        InternalZenEventManager.showPromptText.Invoke();
    }
    #endregion
    
    #region -- Zen Event Methods --
    //This event is called in phase one when attack field weak points are supposed to spawn.
    //attackFieldsActive is set to true and the attack fields are spawned.
    //Then wait for a certain amount of time and then set attackFieldsActive to false and destroy all attack fields.
    private IEnumerator AttackFieldSpawnTimer()
    {
        Debug.Log("AttackFieldSpawnTimer");
        InternalZenEventManager.spawnWeakPoints.Invoke();
        yield return new WaitForSecondsRealtime(_attackFieldsActiveTime);
        InternalZenEventManager.stopSpawnWeakPoints.Invoke();
        
        DestroyAllAttackFields();
        
        if (zenMetreValue >= 100)
        {
            EventManager.BossPhaseSuccessful.Invoke();
        }
        else
        {
            zenLevel = 0;
            zenMetreValue = 100;
            EventManager.StartBossPhase0.Invoke();
        }
    }
    
    //Destroys all weak points. Called after phase 1 is over either because you failed or because you succeeded.
    private void DestroyAllAttackFields()
    {
        GameObject[] attackFields = GameObject.FindGameObjectsWithTag("AttackField");
        foreach (GameObject attackField in attackFields)
        {
            Destroy(attackField);
        }
    }
    
    //This event is called in phase two when triple score is supposed to be activated.
    //tripleScoreActive is set to true and the timer starts.
    //Then wait for a certain amount of time and then set tripleScoreActive to false.
    private IEnumerator TripleScoreTimer()
    {
        yield return new WaitForSecondsRealtime(_tripleScoreTimer);
        
        if (zenMetreValue >= 100)
        {
            EventManager.BossPhaseSuccessful.Invoke();
        }
        else
        {
            zenLevel = 0;
            zenMetreValue = 100;
            EventManager.StartBossPhase0.Invoke();
        }
    }

    //Method that starts time stop coroutine. Workaround because you cant add coroutines to events.
    private void StopTime()
    {
        StartCoroutine(TimeStop());
    }
    
    //Time-stop coroutine. Slows down time for everything, but lets particles move at 1/10 their regular speed.
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
    
    //Method that resets time back to normal. Called when you fail a phase or when you do your final move.
    private void ResetTime()
    {
        Time.timeScale = 1f;
        for (int i = 0; i < _particleSystems.Count; i++)
        {
            var mainModule = _particleSystems[i].main;
            mainModule.simulationSpeed = _originalSimulationSpeeds[i];
        }
    }
    #endregion
    
    #region -- Visual --

    private void CheckForMaxZen()
    {
        if (zenMetreValue >= 100 && zenLevel == 2)
        {
            InternalZenEventManager.showSparkles.Invoke();
        }
    }
    
    #endregion
}
