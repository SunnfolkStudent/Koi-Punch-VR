using FinalScripts.Fish.Spawning;
using UnityEngine;
using UnityEngine.Serialization;

namespace FinalScripts.Fish
{
    public class Fish : MonoBehaviour
    {
        public FishObjectPool.Fish fish { get; set; }
        
        [Header("Despawn")]
        [Tooltip("Determines how long it takes for the fish to despawn")]
        [SerializeField] private float despawnTime = 10f;
        [Tooltip("Determines how low the fish has to be before it despawns")]
        [SerializeField] private float despawnAltitude = -5f;
        private float _startTime;
        
        [Header("FishChild")]
        [Tooltip("A higher value will apply more force to the object after it is punched in addition to the force the speed of the punch itself applies.")]
        public float punchVelMultiplier;
        [FormerlySerializedAs("velocityNeededForSuccessfulHit")]
        [Range(0f, 5f)]
        [Tooltip("The punch velocity need to exceed this value for the punch to count. This value can go from 0 to 5 inclusive.")]
        public float SuccessfulPunchThreshold = 3f;
        [Tooltip("If set to true then the object cannot be punched. It is automatically set to true after being punched")]
        public bool hasBeenPunched;
        [Tooltip("If set to true then the object cannot be punched. It is automatically set to true after hitting the ground")]
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
            Invoke(nameof(Despawn), 2.5f);
            // Despawn();
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