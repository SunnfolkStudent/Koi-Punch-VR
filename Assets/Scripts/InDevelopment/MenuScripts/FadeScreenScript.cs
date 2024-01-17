using UnityEngine;

public class FadeScreenScript : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    public void FadeOut()
    {
        _animator.Play("FadeOut");
    }
}
