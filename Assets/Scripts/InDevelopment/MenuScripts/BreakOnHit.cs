using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnHit : TransitionAnimation
{
    [SerializeField] private GameObject breakPrefab;
    [SerializeField] private int LevelToGoTo;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player") return;
        Debug.Log("collision detected");

        HittingSign();
    }

    protected void HittingSign()
    {
        Instantiate(breakPrefab,gameObject.transform.position,Quaternion.identity);
        
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