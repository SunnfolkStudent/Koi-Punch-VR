using System;
using System.Collections;
using System.Collections.Generic;
using InDevelopment.Fish;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField] private GameObject BirdToSpawn;
    [SerializeField] private float spawnTime;
    
    private void Start()
    {
        StartCoroutine(SpawnBird());
    }

    private IEnumerator SpawnBird()
    {
        yield return new WaitForSeconds(spawnTime);
        Instantiate(BirdToSpawn);
        StartCoroutine(SpawnBird());
    }
}
