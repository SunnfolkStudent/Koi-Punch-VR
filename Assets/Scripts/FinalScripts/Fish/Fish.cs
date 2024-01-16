using FinalScripts.Fish.Spawning;
using UnityEngine;

namespace FinalScripts.Fish
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
            var punchPosition = new Vector3();
            if (other.gameObject.CompareTag("LeftFist") || other.gameObject.CompareTag("RightFist"))
            {
                Debug.Log("Calculating distance from punch...");
                GainZen();
                CalculateFishLaunch();
            }
            
            if (other.gameObject.CompareTag("Ground"))
            {
                Debug.Log("Distance Travelled:" + (transform.position-punchPosition));
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
        
        FishLaunchData CalculateFishLaunch()
        {
            /* var gravity = 9.81f;
            var time = (2*punchForceMultipliercubeLaunchDir.normalized)/gravity;
            var startPos = transform.position;
            var landingPos = new Vector3((startPos.x + time.x punchForceMultiplier), 
            (startPos.y + time.y * punchForceMultiplier), 
            (startPos.z + time.z * punchForceMultiplier));
            //var landingPos = startPos + time * punchForceMultiplier;
            Debug.DrawLine(startPos, landingPos, Color.red, time.y);*/

            return new FishLaunchData();
        }

        struct FishLaunchData
        {
            public readonly Vector3 InitialVelocity;
            public readonly float TimeToTarget;

            public FishLaunchData(Vector3 initialVelocity, float timeToTarget)
            {
                this.InitialVelocity = initialVelocity;
                this.TimeToTarget = timeToTarget;
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