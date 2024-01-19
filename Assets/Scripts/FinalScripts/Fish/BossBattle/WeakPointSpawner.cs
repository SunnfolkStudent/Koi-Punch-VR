using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FinalScripts.Fish.BossBattle
{
    public class WeakPointSpawner : MonoBehaviour
    {
        private bool _isSpawning;
        private float _timeSinceLastSpawn;
        
        private GameObject _boss;
        private CapsuleCollider _bossCollider;
        private Camera _mainCamera;

        #region ---InspectorSettings---
        [Header("Spawning")]
        [SerializeField] private GameObject weakPointPrefab;
        [SerializeField] private float distanceBetweenSpawns = 0.75f;
        [SerializeField] private  float spawnInterval = 2f;

        [Header("While-loops")]
        [SerializeField] private int maxWhileLooping = 10;
        #endregion

        #region ---Initialization---
        private void Start()
        {
            _mainCamera = Camera.main;
            InternalZenEventManager.spawnWeakPoints += SpawnWeakPoints;
            InternalZenEventManager.stopSpawnWeakPoints += StopSpawningWeakPoints;
        }
        #endregion
        
        #region ---Spawning---
        private void SpawnWeakPoints()
        {
            GetBoss();
            StartCoroutine(Spawning());
        }
    
        private void StopSpawningWeakPoints()
        {
            _isSpawning = false;
        }

        private void GetBoss()
        {
            _boss ??= GameObject.FindGameObjectWithTag("Boss");
            _bossCollider ??= _boss.GetComponent<CapsuleCollider>();
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
                    var attackFields = GameObject.FindGameObjectsWithTag("AttackField");
                    Instantiate(weakPointPrefab, GetWeakPointSpawnPosition(attackFields), Quaternion.identity);
                    _timeSinceLastSpawn = 0f;
                }
                
                yield return null;
            }
        }

        #region ---GetSpawningPositions---
        private Vector3 GetWeakPointSpawnPosition(GameObject[] attackFields)
        {
            var whileCount = 0;
            while (whileCount < maxWhileLooping)
            {
                var randomPoint = GetRandomPointOnGameObject(_boss);
                
                if (attackFields.Any(attackField => Vector3.Distance(randomPoint, attackField.transform.position) > distanceBetweenSpawns))
                {
                    return randomPoint;
                }
                
                whileCount++;
            }
            
            Debug.LogWarning($"WhileLoop couldn't find a spawnPoint with more than {distanceBetweenSpawns} distance to another for weakPoints spawning within {maxWhileLooping} iterations.");
            return _boss.transform.position;
        }
        
        private Vector3 GetRandomPointOnGameObject(GameObject target)
        {
            var whileCount = 0;
            while (whileCount < maxWhileLooping)
            {
                var randomDirection = Random.onUnitSphere;
                var ray = new Ray(target.transform.position + randomDirection * _bossCollider.bounds.extents.magnitude * 2f, -randomDirection);
                if (_bossCollider.Raycast(ray, out var hit, _bossCollider.bounds.extents.magnitude * 4f))
                {
                    var surfaceNormal = hit.normal;
                    var cameraDirection = hit.point - _mainCamera.transform.position;
                    
                    Debug.Log($"surfaceNormal: {surfaceNormal} | cameraDirection: {cameraDirection} | dot: {Vector3.Dot(cameraDirection, surfaceNormal.normalized)}");
                    
                    if (Vector3.Dot(cameraDirection.normalized, surfaceNormal.normalized) < 0)
                    {
                        return hit.point;
                    }
                }
                else Debug.Log("***Not hit***");
                
                whileCount++;
            }
            
            Debug.LogWarning($"WhileLoop couldn't find a hit.point on bossCollider visible by the camera within {maxWhileLooping} iterations.");
            return _boss.transform.position;
        }
        #endregion
        
        #endregion
    }
}