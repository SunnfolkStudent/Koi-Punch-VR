using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFieldScript : MonoBehaviour
{
    private float _timeUntilDeath = 5f;
    
    
    void Start()
    { 
        StartCoroutine(DeathTimer());
    }
    
    private IEnumerator DeathTimer()
    {
        yield return new WaitForSecondsRealtime(_timeUntilDeath);
        Destroy(gameObject);
    }
}
