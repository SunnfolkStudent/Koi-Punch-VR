using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace FinalScripts.Zen
{
    public class AttackFieldSpawner : MonoBehaviour
    {
        [Header("Spawning")]
        [SerializeField] private GameObject weakPointPrefab;
        [SerializeField] private float distanceBetweenSpawns = 1f;
        [SerializeField] private  float spawnInterval = 2f;
        private bool _isSpawning;
        private float _timeSinceLastSpawn;
        
        private GameObject _boss;
        private CapsuleCollider _bossCollider;
        // private Camera _mainCamera;

        private void Start()
        {
            _boss = GameObject.FindGameObjectWithTag("Boss");
            _bossCollider = _boss.GetComponent<CapsuleCollider>();
            // _mainCamera = Camera.main;
            
            InternalZenEventManager.spawnWeakPoints += SpawnWeakPoints;
            InternalZenEventManager.stopSpawnWeakPoints += StopSpawningWeakPoints;
        }
    
        private void SpawnWeakPoints()
        {
            StartCoroutine(Spawning());
        }
    
        private void StopSpawningWeakPoints()
        {
            _isSpawning = false;
            StopAllCoroutines();
        }

        private IEnumerator SpawnObjectOnSurface(GameObject[] attackFields)
        {
            var maxWhileCount = 5;
            var whileCount = 0;
            
            var randomPoint = Vector3.zero;
            var isReady = false;
            while (!isReady)
            {
                randomPoint = GetRandomPointOnVisibleSide(_boss);
                isReady = true;
                
                foreach (var attackField in attackFields)
                {
                    if (Vector3.Distance(randomPoint, attackField.transform.position) < distanceBetweenSpawns)
                    {
                        isReady = false;
                    }
                }
                
                if (whileCount++ < maxWhileCount) continue;
                isReady = false;
                Debug.LogError("WhileLoop looped more times than allowed");
            }
            
            Instantiate(weakPointPrefab, randomPoint, Quaternion.identity);
            yield return null;
        }
        
        private Vector3 GetRandomPointOnVisibleSide(GameObject target)
        {
            // if (_mainCamera == null)
            // {
            //     Debug.LogError("Main camera not found in the scene.");
            //     return Vector3.zero;
            // }
            
            var maxWhileCount = 5;
            var whileCount = 0;
            Vector3 randomPoint;
            while (true)
            {
                if (whileCount++ >= maxWhileCount)
                {
                    Debug.LogError("WhileLoop looped more times than allowed");
                    randomPoint = Vector3.zero;
                    break;
                }
                
                var randomDirection = Random.onUnitSphere;

                if (!_bossCollider.Raycast(new Ray(target.transform.position + randomDirection * _bossCollider.bounds.extents.magnitude * 2f, -randomDirection), out var hit, _bossCollider.bounds.extents.magnitude * 4f)) continue;
                // var surfaceNormal = hit.normal;
                // var cameraToSurface = hit.point - _mainCamera.transform.position;
                // 
                // Check if the dot product between camera direction and surface normal is positive (facing camera)
                // if (Vector3.Dot(cameraToSurface.normalized, surfaceNormal.normalized) >= 0) continue;
                randomPoint = hit.point;
                break;
            }

            return randomPoint;
        }

        private IEnumerator Spawning()
        {
            _isSpawning = true;
            Debug.Log("Spawning attack fields");
            while (_isSpawning)
            {
                _timeSinceLastSpawn += Time.unscaledDeltaTime;
                if (_timeSinceLastSpawn >= spawnInterval)
                {
                    _timeSinceLastSpawn = 0f;
                    
                    var attackFields = GameObject.FindGameObjectsWithTag("AttackField");
                    StopCoroutine(SpawnObjectOnSurface(attackFields));
                    // StartCoroutine(SpawnObjectOnSurface(attackFields));
                }

                yield return null;
            }
        }
    }
}