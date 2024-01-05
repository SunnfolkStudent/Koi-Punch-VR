using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenMetreManager : MonoBehaviour
{
    public static ZenMetreManager Instance;
    
    public bool tripleScoreActive;
    public bool zenAttackActive;
    private float _tripleScoreTimer = 10f;
    public bool attackFieldsActive;
    private float _attackFieldsActiveTime = 10f;
    
    public float zenMetre;
    private int _zenLevel;
    
    
    private void Start()
    {
        zenMetre = 0;
        _zenLevel = 1;

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
        if (zenMetre >= 100)
        {
            _zenLevel++;
            SetZenLevel();
        }
    }
    
    //Method that adds zen to the zen metre based on the velocity of the fist
    public void AddHitZen(float fistVelocity)
    {
        zenMetre += fistVelocity;
    }
    
    public void AddAttackFieldZen(float attackFieldSize)
    {
        zenMetre += attackFieldSize;
    }
    
    private void SetZenLevel()
    {
        switch (_zenLevel)
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
        zenMetre = 0;
    }
    
    //Method that moves on to the second level of zen
    private void LevelTwo()
    {
        zenMetre = 0;
        StartCoroutine(attackFieldSpawnTimer());
    }
    
    //Method that moves on to the third level of zen
    private void LevelThree()
    {
        StopCoroutine(attackFieldSpawnTimer());
        attackFieldsActive = false;
        zenMetre = 0;
        tripleScoreActive = true;
        StartCoroutine(TripleScoreTimer());
    }
    
    private void LevelFour()
    {
        zenMetre = 0;
        zenAttackActive = true;
    }
    
    private IEnumerator TripleScoreTimer()
    {
        yield return new WaitForSeconds(_tripleScoreTimer);
        tripleScoreActive = false;
        if (zenMetre >= 100)
        {
            _zenLevel++;
            LevelFour();
        }
        else
        {
            _zenLevel = 1;
            SetZenLevel();
        }
    }
    
    private IEnumerator attackFieldSpawnTimer()
    {
        yield return new WaitForSeconds(_attackFieldsActiveTime);
        attackFieldsActive = false;
        _zenLevel = 1;
        SetZenLevel();
    }
}
