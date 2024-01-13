using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TransitionAnimation : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private string _explodingAnimClip;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        MenuEventManager.ExplodeEvent += ExplodeTransition;
    }
    private void ExplodeTransition()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("TransitionAnimation");
        }
        Destroy(gameObject,5f);
    }
    
    private void OnDisable()
    {
        MenuEventManager.ExplodeEvent -= ExplodeTransition;
    }


}
