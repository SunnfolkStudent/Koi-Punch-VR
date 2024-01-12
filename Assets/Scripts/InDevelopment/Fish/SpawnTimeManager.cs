using System.Collections;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class SpawnTimeManager : MonoBehaviour
    {
        [Header("FishSpawnFrequencyTimer")]
        [SerializeField] private float maxSpawnRate = 5;
        [SerializeField] private float minSpawnRate = 1;
        [SerializeField] private float timeToMaxSpawnRate = 30f;
        [SerializeField] private float timeTillEnd = 60f;
        
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
                //Debug.Log("nextSpawnTime: " + nextSpawnTime);

                //nextSpawnTime = 1.5f;
                
                yield return new WaitForSeconds(nextSpawnTime);
                EventManager.SpawnFish.Invoke();
            }
            EventManager.LevelOver.Invoke();
        }
        #endregion
    }
}