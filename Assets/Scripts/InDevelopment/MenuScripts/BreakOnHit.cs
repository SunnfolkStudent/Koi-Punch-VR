using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnHit : TransitionAnimation
{
    [SerializeField] private GameObject _brokenPrefab;
    [SerializeField] private int LevelToGoTo;
    [SerializeField] private GameObject newMenuParent;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HittingSign();
        }
    }

    protected void HittingSign()
    {
        Instantiate(_brokenPrefab,gameObject.transform.position,Quaternion.identity);
        
        //TODO play break audio
        
        if (gameObject.CompareTag("SceneChanger"))
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            //fade screen here
            SceneController.LevelSelected = LevelToGoTo;
        }
        else
        {
            MenuEventManager.ExplodeTransition();
            Instantiate(newMenuParent);
            Destroy(gameObject);
        }
    }

}