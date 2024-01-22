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
    }

    public void FadeIn()
    {
        if (!isFishTransition)
        {
            _image.enabled = true;
            _animator.Play("CircleTransitionAnimationStart");
        }
        else
        {
            _image.enabled = true;
            _animator.Play("FishTransitionAnimationStart");
        }
    }

    public void FadeOut()
    {
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
