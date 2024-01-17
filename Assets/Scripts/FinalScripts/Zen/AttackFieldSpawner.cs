using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackFieldSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // Prefab of the object to spawn
    private GameObject _targetObject; // The game object onto which to spawn objects
    public float spawnInterval = 2f; // Interval between spawns
    private float _distanceBetweenSpawns = 1f; // Distance between spawned objects

    private bool _isSpawning;
    private float _timeSinceLastSpawn;

    private void Start()
    {
        InternalZenEventManager.spawnWeakPoints += SpawnWeakPoints;
        InternalZenEventManager.stopSpawnWeakPoints += StopSpawningWeakPoints;
    }
    
    private void SpawnWeakPoints()
    {
        _isSpawning = true;
        _targetObject = GameObject.FindGameObjectWithTag("Boss");
        Debug.Log("Spawning attack fields");
        StartCoroutine(Spawning());
    }
    
    private void StopSpawningWeakPoints()
    {
        _isSpawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnObjectOnSurface()
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
                        if (Vector3.Distance(randomPoint, attackField.transform.position) < _distanceBetweenSpawns)
                        {
                            isReady = false;
                        }
                    }
                }
            }

            if (!isReady)
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
        
        // Spawn the object at the random point on the surface
        Instantiate(objectToSpawn, randomPoint, Quaternion.identity);
        yield return null;
    }

    private Vector3 GetRandomPointOnVisibleSide(GameObject target)
    {
        var collider = target.GetComponent<CapsuleCollider>();

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in the scene.");
            return Vector3.zero;
        }

        Vector3 randomPoint;

        while (true)
        {
            Vector3 randomDirection = Random.onUnitSphere;

            RaycastHit hit;
            if (collider.Raycast(new Ray(target.transform.position + randomDirection * collider.bounds.extents.magnitude * 2f, -randomDirection), out hit, collider.bounds.extents.magnitude * 4f))
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
        while (_isSpawning)
        {
            _timeSinceLastSpawn += Time.unscaledDeltaTime;
            if (_timeSinceLastSpawn >= spawnInterval)
            {
                _timeSinceLastSpawn = 0f;
                StopCoroutine(SpawnObjectOnSurface());
                StartCoroutine(SpawnObjectOnSurface());
            }

            yield return null;
        }
    }
}