using System.Collections;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class FishSpawnTimeManager : MonoBehaviour
    {
        [Header("FishSpawnFrequencyTimer")]
        [SerializeField] private float maxSpawnRate = 1.5f;
        [SerializeField] private float minSpawnRate = 0.5f;
        [SerializeField] private float timeToMaxSpawnRate = 30f;
        [SerializeField] private float timeTillEnd = 360f;
        
        private void Start()
        {
            StartCoroutine(SpawnFish());
        }
        
        #region ---FishFrequencyTimer---
        private IEnumerator SpawnFish()
        {
            var startTime = Time.time;
            var minSpawnTime = 1 / maxSpawnRate;
            var maxSpawnTime = 1 / minSpawnRate;
            while (startTime <= timeTillEnd)
            {
                var nextSpawnTime = Mathf.Lerp(maxSpawnTime, minSpawnTime, Time.time / timeToMaxSpawnRate);
                yield return new WaitForSeconds(nextSpawnTime);
                EventManager.SpawnFish.Invoke();
            }
            EventManager.LevelOver.Invoke();
        }
        #endregion
    }
}