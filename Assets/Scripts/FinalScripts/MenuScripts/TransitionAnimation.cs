using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TransitionAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        MenuEventManager.ExplodeEvent += ExplodeTransition;
    }
    private void ExplodeTransition()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("MenuTransition");
            
            //TODO play explosion audio
        }
        Destroy(gameObject,7f);
    }
    
    private void OnDisable()
    {
        MenuEventManager.ExplodeEvent -= ExplodeTransition;
    }


}
