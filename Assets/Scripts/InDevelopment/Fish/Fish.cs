using System;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class Fish : MonoBehaviour
    {
        // TODO: Fish collision?
        public FishObjectPool.Fish fish { get; set; }
        private float _startTime;
        [SerializeField] private float despawnTime = 5f;
        [SerializeField] private float despawnAltitude = -5f;
        
        private void OnTriggerEnter(Collider other)
        {
            var initialPunchPosition = Vector3.zero;
            if (other.gameObject.CompareTag("LeftFist") || other.gameObject.CompareTag("RightFist"))
            {
                Debug.Log("InitialPunchPosition:" + initialPunchPosition);
            }
            if (other.gameObject.CompareTag("Ground"))
            {
                Debug.Log("Distance Travelled:" + (transform.position-initialPunchPosition));
                Despawn();
            }
        }

        private void OnEnable()
        {
            _startTime = Time.time;
        }
        
        private void Update()
        {
            if (transform.position.y < despawnAltitude || _startTime < Time.time - despawnTime)
            {
                Despawn();
            }
        }
        
        // TODO: Punch script needs to make the fish call this function
        private void AddZen()
        {
            ZenMetreManager.Instance.AddHitZen(fish.FishPool.Prefab.ZenAmount);
        }
        
        private void Despawn()
        {
            FishObjectPool.DespawnFish(fish);
        }
    }
}