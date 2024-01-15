using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFieldScript : MonoBehaviour, IPunchable
{
    private float _timeUntilDeath = 5f;
    private bool _dead = false;
    private float _minVelocityToDestroy = 0.5f;
    
    
    void Start()
    { 
        StartCoroutine(DeathTimer());
    }
    
    private IEnumerator DeathTimer()
    {
        yield return new WaitForSecondsRealtime(_timeUntilDeath);
        _dead = true;
        Destroy(gameObject);
    }
    
    public void PunchObject(ControllerManager controllerManager, String fistUsed)
    {
        if (fistUsed == "LeftFist")
        {
            if (controllerManager.leftControllerVelocity.magnitude > _minVelocityToDestroy)
            {
                Destroy(gameObject);
            }
        }
        else if (fistUsed == "RightFist")
        {
            if (controllerManager.rightControllerVelocity.magnitude > _minVelocityToDestroy)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if (!_dead)
        {
            ZenMetreManager.Instance.AddAttackFieldZen();
        }
    }
}
