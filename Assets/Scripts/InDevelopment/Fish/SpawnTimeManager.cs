using System.Collections;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class SpawnTimeManager : MonoBehaviour
    {
        [SerializeField] private float maxSpawnRate;
        [SerializeField] private float minSpawnRate;
        [SerializeField] private float timeToMaxSpawnRate;
        [SerializeField] private float timeTillEnd;
        
        private void Start()
        {
            StartCoroutine(SpawnFish());
        }
        
        private IEnumerator SpawnFish()
        {
            var startTime = Time.time;
            var minSpawnTime = 1 / maxSpawnRate;
            var maxSpawnTime = 1 / minSpawnRate;
            while (true)// _startTime <= timeTillEnd)
            {
                var nextSpawnTime = Mathf.Lerp(maxSpawnTime, minSpawnTime, Time.time / timeToMaxSpawnRate);
                Debug.Log("nextSpawnTime: " + nextSpawnTime);

                nextSpawnTime = 1.5f;
                
                yield return new WaitForSeconds(nextSpawnTime);
                FishSpawnManager.SpawnFish.Invoke();
            }
            // Debug.Log("Level Ended");
        }
    }
}