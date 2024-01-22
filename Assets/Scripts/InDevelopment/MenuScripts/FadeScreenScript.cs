using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreenScript : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool isFishTransition;

    private Image _image;

    public bool transitionOnStart;

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
        
        if (transitionOnStart)
        {
            _image.enabled = true;
        }
    }

    public void FadeOut()
    {
        _image.enabled = false;
        
        if (!isFishTransition)
        {
            _image.enabled = true;
            _animator.Play("CircleTransitionAnimationExit");
        }
        else
        {
            _image.enabled = true;
            _animator.Play("FishTransitionAnimationExit");
        }
    }
}
