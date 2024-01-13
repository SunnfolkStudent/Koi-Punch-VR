using System.Collections;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class FishSpawnTimeManager : MonoBehaviour
    {
        [Header("FishSpawnFrequencyTimer")]
        [SerializeField] private float maxSpawnRate = 1.5f;
        [SerializeField] private float minSpawnRate = 0.5f;
        [SerializeField] private float timeToMaxSpawnRate = 20f;
        
        public static bool isSpawningFish{ get; private set; }
        
        #region ---Initialization---
        private void Start()
        {
            EventManager.StartLevel += StartSpawning;
            EventManager.LevelOver += StopSpawning;
            
            EventManager.StartLevel.Invoke();
        }
        #endregion
        
        #region ---FishSpawnFrequencyControls---
        private void StartSpawning()
        {
            isSpawningFish = true;
            StartCoroutine(SpawnFish());
        }

        private static void StopSpawning()
        {
            isSpawningFish = false;
        }
        #endregion
        
        #region ---FishFrequencyTimer---
        private IEnumerator SpawnFish()
        {
            var minSpawnTime = 1 / maxSpawnRate;
            var maxSpawnTime = 1 / minSpawnRate;
            while (isSpawningFish)
            {
                var nextSpawnTime = Mathf.Lerp(maxSpawnTime, minSpawnTime, Time.time / timeToMaxSpawnRate);
                yield return new WaitForSeconds(nextSpawnTime);
                EventManager.SpawnFish.Invoke();
            }
        }
        #endregion
    }
}