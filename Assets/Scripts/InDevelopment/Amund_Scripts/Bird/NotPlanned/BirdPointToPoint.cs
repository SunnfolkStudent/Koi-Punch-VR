using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPointToPoint : MonoBehaviour
{
    [SerializeField] private Transform[] Points;

    [SerializeField] private float moveSpeed;

    private int pointsIndex;

    private void Start()
    {
        transform.position = Points[pointsIndex].transform.position;
    }

    private void Update()
    {
        if (pointsIndex <= Points.Length - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, Points[pointsIndex].transform.position,
                moveSpeed * Time.deltaTime);
            if (transform.position == Points[pointsIndex].transform.position)
            {
                pointsIndex += 1;
            }

            if (pointsIndex == Points.Length)
            {
                pointsIndex = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Collision");
        Destroy(gameObject);
    }
}
