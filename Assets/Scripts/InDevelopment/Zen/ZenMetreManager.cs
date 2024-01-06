using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ZenMetreManager : MonoBehaviour
{
    public static ZenMetreManager Instance;
    
    public bool tripleScoreActive;
    public bool zenAttackActive;
    private float _tripleScoreTimer = 10f;
    public bool attackFieldsActive;
    private float _attackFieldsActiveTime = 10f;
    
    public float zenMetreValue;
    public int zenLevel;
    
    private float _slowdownFactor = 0.001f;
    private float _slowdownTime = 0.5f;
    
    
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
    
    //Method that adds zen to the zen metre based on the velocity of the fist
    public void AddHitZen(float score)
    {
        zenMetreValue += score;
    }
    
    public void AddAttackFieldZen(float attackFieldSize)
    {
        zenMetreValue += attackFieldSize;
    }
    
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
    
    private void LevelOne()
    {
        Time.timeScale = 1f;
        zenMetreValue = 0;
    }
    
    //Method that moves on to the second level of zen
    private void LevelTwo()
    {
        zenMetreValue = 0;
        StartCoroutine(TimeStop());
        attackFieldsActive = true;
        StartCoroutine(attackFieldSpawnTimer());
    }
    
    //Method that moves on to the third level of zen
    private void LevelThree()
    {
        StopCoroutine(attackFieldSpawnTimer());
        attackFieldsActive = false;
        zenMetreValue = 0;
        tripleScoreActive = true;
        StartCoroutine(TripleScoreTimer());
    }
    
    private void LevelFour()
    {
        zenMetreValue = 0;
        zenAttackActive = true;
    }
    
    private IEnumerator TripleScoreTimer()
    {
        yield return new WaitForSeconds(_tripleScoreTimer);
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
    
    private IEnumerator attackFieldSpawnTimer()
    {
        yield return new WaitForSeconds(_attackFieldsActiveTime);
        attackFieldsActive = false;
        zenLevel = 1;
        SetZenLevel();
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
        float currentTimeScale = Time.timeScale;
        float timePassed = 0f;

        while (timePassed < _slowdownTime)
        {
            Time.timeScale = Mathf.Lerp(currentTimeScale, _slowdownFactor, timePassed / _slowdownTime);
            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = _slowdownFactor;
    }
}
