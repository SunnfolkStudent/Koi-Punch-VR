using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace FinalScripts.Fish.BossBattle
{
    public class WeakPointSpawner : MonoBehaviour
    {
        private GameObject _boss;
        private CapsuleCollider _bossCollider;
        private Camera _mainCamera;
        
        private bool _isSpawning;
        private float _timeSinceLastSpawn;
        
        #region ---InspectorSettings---
        [Header("Spawning")]
        [SerializeField] private GameObject weakPointPrefab;
        [SerializeField] private float spawnTightness = 0.5f;
        [SerializeField] private float minDistBetweenPoints = 0.75f;
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

        #region >>>---GetSpawningPositions---
        private Vector3 GetWeakPointSpawnPosition(GameObject[] attackFields)
        {
            if (_bossCollider == null) return _boss.transform.position;
            var whileCount = 0;
            while (whileCount < maxWhileLooping)
            {
                var randomPoint = GetRandomPointOnGameObject(_boss);
                
                if (attackFields.Any(attackField => Vector3.Distance(randomPoint, attackField.transform.position) > minDistBetweenPoints))
                {
                    return randomPoint;
                }
                
                whileCount++;
            }
            
            Debug.LogWarning($"WhileLoop couldn't find a spawnPoint with more than {minDistBetweenPoints} " +
                             $"distance to another for weakPoints spawning within {maxWhileLooping} iterations.");
            
            return _boss.transform.position;
        }
        
        private Vector3 GetRandomPointOnGameObject(GameObject target)
        {
            var randomDirection = Random.onUnitSphere;
            var dir = -_mainCamera.transform.forward;
            var biasedRandomDirection = Vector3.Lerp(randomDirection, dir, spawnTightness).normalized;
            
            var ray = new Ray(target.transform.position + biasedRandomDirection * _bossCollider.bounds.extents.magnitude * 2f, -biasedRandomDirection);
            if (!_bossCollider.Raycast(ray, out var hit, _bossCollider.bounds.extents.magnitude * 4f))
                Debug.LogError("WeakPointSpawner couldn't find a spawnPoint within the bounds of the boss.");
            
            return hit.point;
        }
        #endregion
        #endregion
    }
}