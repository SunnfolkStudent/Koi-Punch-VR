using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] private InputActionReference gripAction;

    [SerializeField] private float _grip;

    [SerializeField] private GameObject openHand;
    [SerializeField] private GameObject closedHand;

    void Update()
    {
        GripValue();


        if (_grip !=0)
        {
           openHand.SetActive(false); 
           closedHand.SetActive(true);
        }
        else
        {
            openHand.SetActive(true); 
            closedHand.SetActive(false); 
        }
        
    }
    void GripValue()
    {
        _grip = gripAction.action.ReadValue<float>();
    }
}

