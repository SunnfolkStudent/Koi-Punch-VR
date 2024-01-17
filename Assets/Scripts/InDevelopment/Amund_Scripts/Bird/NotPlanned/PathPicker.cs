using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

public class PathPicker : MonoBehaviour
{
    [SerializeField] private SplineContainer[] Splines;
    private int pointsIndex;
    private int currentPath;
    private BirdRandomPathing _birdRandomPathing;

    [SerializeField] private int minSpeed;
    [SerializeField] private int maxSpeed;

    private void Start()
    {
        _birdRandomPathing = gameObject.GetComponent<BirdRandomPathing>();
        pointsIndex = Splines.Length;
        _birdRandomPathing.MaxSpeed = Random.Range(minSpeed, maxSpeed);
        PickPath();
    }
    
    private void PickPath()
    {
        currentPath = Random.Range(0, pointsIndex);
        transform.position = Splines[currentPath].transform.position;
        _birdRandomPathing.m_Target = Splines[currentPath];
        
    }
    private void OnTriggerEnter(Collider other)
    {
        print("Collision");
        //Trigger VFX here
        Destroy(gameObject);
    }
}
