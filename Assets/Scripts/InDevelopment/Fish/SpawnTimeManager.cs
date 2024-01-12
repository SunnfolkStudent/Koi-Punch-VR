using System.Collections;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class SpawnTimeManager : MonoBehaviour
    {
        [SerializeField] private float maxSpawnRate = 5;
        [SerializeField] private float minSpawnRate = 1;
        [SerializeField] private float timeToMaxSpawnRate = 30f;
        [SerializeField] private float timeTillEnd = 60f;
        
        private void Start()
        {
            StartCoroutine(SpawnFish());
        }
        
        private IEnumerator SpawnFish()
        {
            var startTime = Time.time;
            var minSpawnTime = 1 / maxSpawnRate;
            var maxSpawnTime = 1 / minSpawnRate;
            while (true)//startTime <= timeTillEnd)
            {
                var nextSpawnTime = Mathf.Lerp(maxSpawnTime, minSpawnTime, Time.time / timeToMaxSpawnRate);
                Debug.Log("nextSpawnTime: " + nextSpawnTime);

                nextSpawnTime = 1.5f;//
                
                yield return new WaitForSeconds(nextSpawnTime);
                FishSpawnManager.SpawnFish.Invoke();
            }
            Debug.Log("Level Ended");
        }
    }
}