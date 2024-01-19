using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace FinalScripts.Fish.Spawning
{
    public class FishSpawnTimeManager : MonoBehaviour
    {
        public static bool isSpawningFish{ get; private set; }
        
        [Header("FishSpawnFrequencyTimer")]
        [SerializeField] private float maxSpawnRate = 1.5f;
        [SerializeField] private float minSpawnRate = 0.5f;
        [SerializeField] private float timeToMaxSpawnRate = 20f;
        [SerializeField] private AnimationCurve animationCurve;
        
        #region ---Initialization---
        private void Start()
        {
            EventManager.FishSpawning += StartSpawning;
            EventManager.FishSpawningAtMaxRate += StartSpawningAtMaxRate;
            EventManager.StopFishSpawning += StopSpawning;
            EventManager.SpawnBoss += StopSpawning;

            EventManager.FishSpawning.Invoke();
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
                // TODO: Set up AnimationCurve in response to fishSpawnTimes over time... (looks something like the below line)
                // var nextSpawnTime = Mathf.Lerp(minSpawnTime, maxSpawnTime, (animationCurve.Evaluate(maxSpawnTime / minSpawnTime)));
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