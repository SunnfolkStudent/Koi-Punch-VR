using System.Collections;
using UnityEngine;

public class AttackFieldSpawnerCapsule : MonoBehaviour
{
    public GameObject objectToSpawn; // Prefab of the object to spawn
    private GameObject _targetObject; // The game object onto which to spawn objects
    public float spawnInterval = 2f; // Interval between spawns
    private float _distanceBetweenSpawns = 1f; // Distance between spawned objects

    private bool _isSpawning;
    private float _timeSinceLastSpawn;

    private void Update()
    {
        if (ZenMetreManager.Instance.attackFieldsActive && !_isSpawning)
        {
            _isSpawning = true;
            _targetObject = GameObject.FindGameObjectWithTag("Boss");
            Debug.Log("Spawning attack fields");
            StartCoroutine(Spawning());
        }
        else if (!ZenMetreManager.Instance.attackFieldsActive && _isSpawning)
        {
            _isSpawning = false;
            StopAllCoroutines();
        }
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
        CapsuleCollider capsuleCollider = target.GetComponent<CapsuleCollider>();

        if (capsuleCollider == null)
        {
            Debug.LogError("CapsuleCollider not found on the target object.");
            return Vector3.zero;
        }

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in the scene.");
            return Vector3.zero;
        }

        Vector3 randomPoint;

        while (true)
        {
            float randomU = Random.Range(0f, 1f);
            float randomV = Random.Range(0f, 1f);

            Vector3 pointOnCapsule = GetPointOnCapsule(capsuleCollider, randomU, randomV);

            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, pointOnCapsule - mainCamera.transform.position, out hit))
            {
                // Check if the dot product between camera direction and surface normal is positive (facing camera)
                if (Vector3.Dot(hit.normal, mainCamera.transform.forward) > 0)
                {
                    randomPoint = hit.point;
                    break;
                }
            }
        }

        return randomPoint;
    }

    private Vector3 GetPointOnCapsule(CapsuleCollider capsuleCollider, float u, float v)
    {
        // u is the height parameter (0 to 1), and v is the angle parameter (0 to 1)
        float height = Mathf.Lerp(-capsuleCollider.height * 0.5f, capsuleCollider.height * 0.5f, u);
        float angle = v * 360f;

        // Convert to radians
        float radianAngle = angle * Mathf.Deg2Rad;

        // Calculate the position on the surface of the capsule
        float x = Mathf.Cos(radianAngle) * capsuleCollider.radius;
        float z = Mathf.Sin(radianAngle) * capsuleCollider.radius;

        return capsuleCollider.transform.TransformPoint(new Vector3(x, height, z));
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
