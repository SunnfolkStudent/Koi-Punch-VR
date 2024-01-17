using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCheckSigns : TransitionAnimation
{
    [SerializeField] private GameObject _nextPrefab;
    [SerializeField] private bool isBreaking = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !isBreaking)
        {
            HitSign();
        }
        
        if (other.gameObject.CompareTag("Player") && isBreaking)
        {
            DestroySign();
        }
    }

    private void HitSign()
    {
        Debug.Log("hit once");
        Instantiate(_nextPrefab, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
        Debug.Log("hit once after");
    }

    private void DestroySign()
    {
        Debug.Log("hit twice");
        
        Instantiate(_nextPrefab,gameObject.transform.position,Quaternion.identity);
        
        //TODO play break audio
        
        MenuEventManager.ExplodeTransition();
        Destroy(gameObject);
    }
}
