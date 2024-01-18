using System.Collections;
using Unity.Mathematics;
using UnityEngine;
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
            // _mainCamera = Camera.main;
            InternalZenEventManager.spawnWeakPoints += SpawnWeakPoints;
            InternalZenEventManager.stopSpawnWeakPoints += StopSpawningWeakPoints;
        }
        
        private void SpawnWeakPoints()
        {
            if (_boss == null) GetBoss();
            StartCoroutine(Spawning());
        }

        private void GetBoss()
        {
            _boss = GameObject.FindGameObjectWithTag("Boss");
            _bossCollider = _boss.GetComponent<CapsuleCollider>();
        }
    
        private void StopSpawningWeakPoints()
        {
            _isSpawning = false;
            // StopAllCoroutines();
        }

        private IEnumerator SpawnObjectOnSurface(GameObject[] attackFields)
        {
            var maxWhileCount = 5;
            var whileCount = 0;
            
            var randomPoint = Vector3.zero;
            var isReady = false;
            while (!isReady)
            {
                randomPoint = GetRandomPointOnGameObject(_boss);
                isReady = true;
                
                foreach (var attackField in attackFields)
                {
                    if (Vector3.Distance(randomPoint, attackField.transform.position) < distanceBetweenSpawns)
                    {
                        isReady = false;
                    }
                }
                
                if (whileCount++ < maxWhileCount) continue;
                Debug.LogError("WhileLoop looped more times than allowed");
                isReady = false;
            }
            
            Instantiate(weakPointPrefab, randomPoint, Quaternion.identity);
            yield return null;
        }
        
        private Vector3 GetRandomPointOnGameObject(GameObject target)
        {
            var maxWhileCount = 5;
            var whileCount = 0;
            Vector3 randomPoint;
            while (true)
            {
                if (whileCount++ >= maxWhileCount)
                {
                    Debug.LogError("WhileLoop looped more times than allowed");
                    randomPoint = _boss.transform.position;
                    break;
                }

                if (!_bossCollider.Raycast(new Ray(target.transform.position, Random.onUnitSphere), out var hit, math.abs(_bossCollider.bounds.extents.magnitude))) continue;
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
                    var attackFields = GameObject.FindGameObjectsWithTag("AttackField");
                    StopCoroutine(SpawnObjectOnSurface(attackFields));
                    _timeSinceLastSpawn = 0f;
                }

                yield return null;
            }
        }
    }
}