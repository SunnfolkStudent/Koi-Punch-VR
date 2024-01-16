using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCheckSigns : TransitionAnimation
{
    [SerializeField] private GameObject _breakPrefab;
    [SerializeField] private GameObject _brokenPrefab;

    private bool isBreaking = false;
    
    [SerializeField] private int LevelToGoTo;


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
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        Instantiate(_breakPrefab, gameObject.transform.position, Quaternion.identity);
        isBreaking = true;
        Debug.Log("hit once after");
    }

    private void DestroySign()
    {
        Debug.Log("hit twice");
        
        _breakPrefab.gameObject.transform.localScale = new Vector3(0, 0, 0);
        Instantiate(_brokenPrefab,gameObject.transform.position,Quaternion.identity);
        
        //TODO play break audio
        
        MenuEventManager.ExplodeTransition();
        if (gameObject.CompareTag("SceneChanger"))
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            SceneController.LevelSelected = LevelToGoTo;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
