using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackFieldSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // Prefab of the object to spawn
    public GameObject targetObject; // The game object onto which to spawn objects
    public float spawnInterval = 2f; // Interval between spawns
    
    private List<GameObject> _spawnedObjects = new List<GameObject>();

    private bool isSpawning = false;

    private void Update()
    {
        if (ZenMetreManager.Instance.attackFieldsActive && !isSpawning)
        {
            isSpawning = true;
            targetObject = GameObject.FindGameObjectWithTag("Boss");
            InvokeRepeating("SpawnObjectOnSurface", 0f, spawnInterval);
        }
        else if (!ZenMetreManager.Instance.attackFieldsActive && isSpawning)
        {
            isSpawning = false;
            CancelInvoke("SpawnObjectOnSurface");
        }
    }

    private void SpawnObjectOnSurface()
    {
        bool isReady = false;
        Vector3 randomPoint = Vector3.zero;
        
        while (!isReady)
        {
            // Get a random point on the surface of the target object
            randomPoint = GetRandomPointOnSurface(targetObject);

            isReady = true;
            if (_spawnedObjects.Count > 0)
            {
                foreach (GameObject attackField in _spawnedObjects)
                {
                    if (attackField != null)
                    {
                        if (Vector3.Distance(randomPoint, attackField.transform.position) < 1f)
                        {
                            isReady = false;
                        }
                    }
                }
            }
        }
        
        
        // Spawn the object at the random point on the surface
        GameObject spawnedObject = Instantiate(objectToSpawn, randomPoint, Quaternion.identity);
        _spawnedObjects.Add(spawnedObject);
    }
    
    private Vector3 GetRandomPointOnSurface(GameObject target)
    {
        MeshCollider meshCollider = target.GetComponent<MeshCollider>();

        if (meshCollider == null || meshCollider.sharedMesh == null)
        {
            Debug.LogError("MeshCollider or mesh not found on the target object.");
            return Vector3.zero;
        }

        Vector3 randomPoint = Vector3.zero;

        while (true)
        {
            Vector3 randomDirection = Random.onUnitSphere;

            RaycastHit hit;
            if (meshCollider.Raycast(new Ray(target.transform.position + randomDirection * meshCollider.bounds.extents.magnitude * 2f, -randomDirection), out hit, meshCollider.bounds.extents.magnitude * 4f))
            {
                randomPoint = hit.point;
                break;
            }
        }

        return randomPoint;
    }

}