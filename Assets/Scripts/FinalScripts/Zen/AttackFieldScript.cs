using System;
using System.Collections;
using UnityEngine;

public class AttackFieldScript : MonoBehaviour, IPunchable
{
    private float _timeUntilDeath = 6f;
    private bool _dead;
    private float _minVelocityToDestroy = 0f;
    
    
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
        Debug.Log($"Punched L: {controllerManager.leftControllerVelocity.magnitude} R: {controllerManager.rightControllerVelocity.magnitude}", this);
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
