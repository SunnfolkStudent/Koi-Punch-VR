using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ZenTester : MonoBehaviour
{

    public bool zenUp;
    public bool zenDown;
    
    public Slider zenBar;
    private float currentZenBar;
    private float startingZenBar = 0f;

    private void Start()
    {

        currentZenBar = startingZenBar;
        zenBar.value = startingZenBar;
    }

    private void Update()
    {
        zenBar.value = currentZenBar;
    }

    private void OnTriggerEnter(Collider other)
    {
            SetZenBar();
    }

    public void SetZenBar()
    {
        if (zenUp)
        {
            currentZenBar += 0.03f;
        }

        if (zenDown)
        {
            currentZenBar -= 0.03f;
        }

        zenBar.value = currentZenBar;
    }
}
