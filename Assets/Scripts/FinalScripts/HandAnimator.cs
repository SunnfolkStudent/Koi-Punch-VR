using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    [SerializeField] private InputActionReference gripAction;
    
    [SerializeField] private float _grip;
    
    [Header("Animator")] 
    private Animator Anim;
    void Start()
    {
        Anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        _grip = gripAction.action.ReadValue<float>();
        
        Anim.SetFloat("Grip", _grip);
        
    }
}
