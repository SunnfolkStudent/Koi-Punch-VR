using System;
using System.Collections;
using FinalScripts.Fish.Spawning;
using UnityEngine;
using UnityEngine.Serialization;

namespace FinalScripts.Fish
{
    public class Fish : MonoBehaviour
    {
        public FishObjectPool.Fish fish { get; set; }
        
        #region ---InspectorSettings---
        [Header("Despawn")] 
        [Tooltip("Despawn after this amount of seconds when fish hits the ground")]
        [SerializeField] private float despawnDelay = 2.5f;
        [Tooltip("Determines how long it takes for the fish to despawn")]
        [SerializeField] private float despawnTime = 10f;
        [Tooltip("Determines how low the fish has to be before it despawns")]
        [SerializeField] private float despawnAltitude = -5f;
        private float _startTime;
        
        [Header("FishChild")]
        [Tooltip("A higher value will apply more force to the object after it is punched")]
        public float punchVelMultiplier;
        [Tooltip("The punch velocity need to exceed this value for the punch to count as successful.")]
        [Range(0f, 5f)]public float successfulPunchThreshold = 3f;

        private Vector3 _punchedPosition;
        
        [HideInInspector] public bool hasBeenPunched;
        [HideInInspector] public bool hasHitGround;
        [HideInInspector] public bool hasHitPlayer;
        public bool hasEmergedFromWater;
        
        [Header("debug")]
        public bool isDebugging;
        #endregion
        
        #region ---Initialization---
        private void Start()
        {
            var c = GetComponentsInChildren<Transform>();
            foreach (var child in c)
            {
                var fishChild = child.gameObject.AddComponent<FishChild>();
                fishChild.fish = this;
            }
        }

        private void OnEnable()
        {
            _startTime = Time.time;
            hasBeenPunched = false;
            hasHitGround = false;
            StopCoroutine(DespawnAfterTime(despawnDelay));
        }
        #endregion
        
        #region ---Debugging---
        public void Log(string message)
        {
            if(isDebugging) Debug.Log(message);
        }
        #endregion
        
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

        public void FishHitWater()
        {
            if (!hasEmergedFromWater)
            {
                // TODO: Play Water Exit VFX (From Water)
                hasEmergedFromWater = true;
                return;
            }
            
            var canSkipp = CheckIfCanSkipp();
            if (canSkipp)
            {
                // TODO: Skipp on Water
                return;
            }
            // TODO: Play Water Entry VFX (From Water)
        }
        
        private bool CheckIfCanSkipp()
        {
            return false;
        }
        
        public void FishHitGround()
        {
            hasHitGround = true;
            if (hasBeenPunched)
            {
                GainPoints();
            }
            Log("De-spawning: hit ground");
            StartCoroutine(DespawnAfterTime(despawnDelay));
        }

        private void GainPoints()
        {
            var points = Vector3.Distance(transform.position, _punchedPosition) * fish.FishPool.FishRecord.ScoreMultiplierDistance;
            EventManager.GainScore(points);
        }
        
        private IEnumerator DespawnAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Despawn();
        }
        
        public void FishHitPlayer()
        {
            hasHitPlayer = true;
            ZenMetreManager.Instance.RemoveZen(fish.FishPool.FishRecord.ZenAmount * 2);
        }
        
        public void FishPunched()
        {
            hasBeenPunched = true;
            _punchedPosition = transform.position;
            GainZen();
        }
        
        private void GainZen()
        {
            ZenMetreManager.Instance.AddHitZen(fish.FishPool.FishRecord.ZenAmount);
            Log("Zen gained: " + fish.FishPool.FishRecord.ZenAmount);
        }
        
        private void Despawn()
        {
            FishObjectPool.DespawnFish(fish);
        }
    }
}