using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackFieldSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // Prefab of the object to spawn
    private GameObject _targetObject; // The game object onto which to spawn objects
    public float spawnInterval = 2f; // Interval between spawns

    private bool isSpawning = false;
    float timeSinceLastSpawn = 0f;

    private void Update()
    {
        if (ZenMetreManager.Instance.attackFieldsActive && !isSpawning)
        {
            isSpawning = true;
            _targetObject = GameObject.FindGameObjectWithTag("Boss");
            StartCoroutine(Spawning());
        }
        else if (!ZenMetreManager.Instance.attackFieldsActive && isSpawning)
        {
            isSpawning = false;
        }
    }

    private void SpawnObjectOnSurface()
    {
        bool isReady = false;
        Vector3 randomPoint = Vector3.zero;
        
        while (!isReady)
        {
            // Get a random point on the surface of the target object
            randomPoint = GetRandomPointOnVisibleSide(_targetObject);

            isReady = true;
            
            GameObject[] attackFields = GameObject.FindGameObjectsWithTag("AttackField");
            
            if (attackFields.Length > 0)
            {
                foreach (GameObject attackField in attackFields)
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
        Instantiate(objectToSpawn, randomPoint, Quaternion.identity);
    }

    private Vector3 GetRandomPointOnVisibleSide(GameObject target)
    {
        MeshCollider meshCollider = target.GetComponent<MeshCollider>();

        if (meshCollider == null || meshCollider.sharedMesh == null)
        {
            Debug.LogError("MeshCollider or mesh not found on the target object.");
            return Vector3.zero;
        }

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in the scene.");
            return Vector3.zero;
        }

        Vector3 randomPoint = Vector3.zero;

        while (true)
        {
            Vector3 randomDirection = Random.onUnitSphere;

            RaycastHit hit;
            if (meshCollider.Raycast(new Ray(target.transform.position + randomDirection * meshCollider.bounds.extents.magnitude * 2f, -randomDirection), out hit, meshCollider.bounds.extents.magnitude * 4f))
            {
                Vector3 surfaceNormal = hit.normal;
                Vector3 cameraToSurface = hit.point - mainCamera.transform.position;

                // Check if the dot product between camera direction and surface normal is positive (facing camera)
                if (Vector3.Dot(cameraToSurface.normalized, surfaceNormal.normalized) < 0)
                {
                    randomPoint = hit.point;
                    break;
                }
            }
        }

        return randomPoint;
    }

    private IEnumerator Spawning()
    {
        while (isSpawning)
        {
            timeSinceLastSpawn += Time.unscaledDeltaTime;
            if (timeSinceLastSpawn >= spawnInterval)
            {
                timeSinceLastSpawn = 0f;
                SpawnObjectOnSurface();
            }

            yield return null;
        }
    }
}