using System.Collections;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class FishSpawnTimeManager : MonoBehaviour
    {
        public static bool isSpawningFish{ get; private set; }
        
        [Header("FishSpawnFrequencyTimer")]
        [SerializeField] private float maxSpawnRate = 1.5f;
        [SerializeField] private float minSpawnRate = 0.5f;
        [SerializeField] private float timeToMaxSpawnRate = 20f;
        
        #region ---Initialization---
        private void Awake()
        {
            EventManager.FishSpawning += StartSpawning;
            EventManager.FishSpawningAtMaxRate += StartSpawningAtMaxRate;
            EventManager.StopFishSpawning += StopSpawning;
            
            EventManager.FishSpawning.Invoke(); // TODO: remove this after setting event to invoke from start level
        }
        #endregion
        
        #region ---FishSpawnFrequencyControls---
        private void StartSpawning()
        {
            StartCoroutine(SpawnFish());
        }
        
        private void StartSpawningAtMaxRate()
        {
            StartCoroutine(SpawnFishMaxRate());
        }

        private static void StopSpawning()
        {
            isSpawningFish = false;
        }
        #endregion
        
        #region ---FishFrequencyTimer---
        private IEnumerator SpawnFish()
        {
            isSpawningFish = true;
            var minSpawnTime = 1 / maxSpawnRate;
            var maxSpawnTime = 1 / minSpawnRate;
            while (isSpawningFish)
            {
                var nextSpawnTime = Mathf.Lerp(maxSpawnTime, minSpawnTime, Time.time / timeToMaxSpawnRate);
                yield return new WaitForSeconds(nextSpawnTime);
                EventManager.SpawnFish.Invoke();
            }
        }
        
        private IEnumerator SpawnFishMaxRate()
        {
            var spawnTime = 1 / maxSpawnRate;
            while (isSpawningFish)
            {
                yield return new WaitForSeconds(spawnTime);
                EventManager.SpawnFish.Invoke();
            }
        }
        #endregion
    }
}