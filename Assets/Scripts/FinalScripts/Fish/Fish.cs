using FinalScripts.Fish.Spawning;
using UnityEngine;

namespace FinalScripts.Fish
{
    public class Fish : MonoBehaviour
    {
        public FishObjectPool.Fish fish { get; set; }
        
        [Header("Despawn")]
        [SerializeField] private float despawnTime = 10f;
        [SerializeField] private float despawnAltitude = -5f;
        private float _startTime;
        
        [Header("FishChild")]
        public float punchVelMultiplier;
        public float velocityNeededForSuccessfulHit = 3f;
        public bool hasBeenPunched;
        public bool hasHitGround;
        
        [Header("debug")]
        public bool isDebugging = true;
        
        private void OnEnable()
        {
            _startTime = Time.time;
            hasBeenPunched = false;
            hasHitGround = false;
        }
        
        #region ---Debugging---
        public void Log(string message)
        {
            if(isDebugging) Debug.Log(message);
        }
        #endregion
        
        private void Start()
        {
            var c = GetComponentsInChildren<Transform>();
            foreach (var child in c)
            {
                var fishChild = child.gameObject.AddComponent<FishChild>();
                fishChild.fish = this;
            }
        }
        
        private void Update()
        {
            DespawnIfOutOfTimeOrTooLow();
        }

        private void DespawnIfOutOfTimeOrTooLow()
        {
            if (transform.position.y > despawnAltitude && _startTime > Time.time - despawnTime) return;
            Log("De-spawned: either to time or y altitude to low");
            Despawn();
        }
        
        public void FishHitGround()
        {
            hasHitGround = true;
            Log("De-spawned: hit ground");
            Despawn();
        }
        
        public void FishPunched()
        {
            hasBeenPunched = true;
            GainZen();
        }
        
        private void GainZen()
        {
            ZenMetreManager.Instance.AddHitZen(fish.FishPool.Prefab.ZenAmount);
            Log("Zen gained: " + fish.FishPool.Prefab.ZenAmount);
        }
        
        private void Despawn()
        {
            FishObjectPool.DespawnFish(fish);
        }
    }
}