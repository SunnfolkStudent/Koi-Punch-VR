using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnHit : TransitionAnimation
{
    [SerializeField] private GameObject breakPrefab;
    [SerializeField] private int LevelToGoTo;
    
    protected void HittingSign()
    {
        Instantiate(breakPrefab,gameObject.transform.position,Quaternion.identity);
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