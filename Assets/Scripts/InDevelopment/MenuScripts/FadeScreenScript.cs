using UnityEngine;

public class FadeScreenScript : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool isFishTransition;
    
    public void FadeOut()
    {
        if (!isFishTransition)
        {
            _animator.Play("CircleTransitionAnimationExit");
        }
        else
        {
            _animator.Play("FishTransitionAnimationExit");
        }
    }
}
