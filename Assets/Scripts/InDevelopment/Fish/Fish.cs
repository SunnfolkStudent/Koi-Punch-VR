using UnityEngine;

namespace InDevelopment.Fish
{
    public class Fish : MonoBehaviour
    {
        public FishObjectPool.Fish fish { get; set; }
        [SerializeField] private float despawnTime = 5f;
        [SerializeField] private float despawnAltitude = -5f;
        private float _startTime;
        
        #region ---Debugging---
        private static bool _isDebugging;
        private static void Log(string message)
        {
            if(_isDebugging) Debug.Log(message);
        }
        #endregion
        
        private void OnTriggerEnter(Collider other)
        {
<<<<<<< Updated upstream
            var initialPunchPosition = Vector3.zero;
            if (other.gameObject.CompareTag("LeftFist") || other.gameObject.CompareTag("RightFist"))
            {
                Debug.Log("InitialPunchPosition:" + initialPunchPosition);
            }
            if (other.gameObject.CompareTag("Ground"))
            {
                Debug.Log("Distance Travelled:" + (transform.position-initialPunchPosition));
=======
            if (other.gameObject.CompareTag("LeftFist") || other.gameObject.CompareTag("RightFist"))
            {
                GainZen();
            }
            if (other.gameObject.CompareTag("Ground"))
            {
>>>>>>> Stashed changes
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
                Log("De-spawned either to time or y altitude to low");
                Despawn();
            }
        }
        
        // TODO: Punch script needs to make the fish call this function
        private void GainZen()
        {
            ZenMetreManager.Instance.AddHitZen(fish.FishPool.Prefab.ZenAmount);
            Log("Zen gained: " + fish.FishPool.Prefab.ZenAmount);
        }
        
        private void Despawn()
        {
            FishObjectPool.DespawnFish(fish);
            Log("DespawnFish");
        }
    }
}