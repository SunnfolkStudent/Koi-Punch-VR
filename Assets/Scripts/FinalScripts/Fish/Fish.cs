using System;
using System.Collections;
using FinalScripts.Fish.Spawning;
using UnityEngine;
using UnityEngine.Serialization;

namespace FinalScripts.Fish
{
    public class Fish : MonoBehaviour
    {
        public FishObjectPool.Fish fish { get; set; } // Reference to itself in the FishPool
        private Vector3 _punchedPosition; // Compared with landing position to calculate points
        
        #region ---PublicStates---
        [FormerlySerializedAs("hasBeenPunched")] [HideInInspector] public bool hasBeenPunchedSuccessfully;
        [FormerlySerializedAs("hasBeenPunchedWeekly")] [HideInInspector] public bool hasBeenPunchedUnsuccessfully;
        [HideInInspector] public bool hasHitGround;
        private bool _hasHitPlayer;
        private bool _hasEmergedFromWater;
        private bool _hasEnteredWater;
        private bool _hasHitBird;
        #endregion
        
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
        
        [Header("Score")]
        [SerializeField] private int baseScoreOfPunch = 10;
        [SerializeField] private int scoreLostFromPlayerHit = 20;
        
        [Header("Skipping")]
        [SerializeField] private float fSpeedNeededMultiplier = 1f;
        [SerializeField] private float attackAngleMaximum = 15f;
        
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
            hasBeenPunchedSuccessfully = false;
            hasBeenPunchedUnsuccessfully = false;
            hasHitGround = false;
            _hasHitPlayer = false;
            _hasEnteredWater = false;
            _hasEmergedFromWater = false;
            _hasHitBird = false;
            StopCoroutine(DespawnAfterTime(despawnDelay));
        }
        #endregion
        
        #region ---Debugging---
        public void Log(string message)
        {
            if(isDebugging) Debug.Log(message);
        }
        #endregion
        
        #region ---FishActions---
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
        
        private void Despawn()
        {
            // TODO: Play Smoke explosion VFX
            FishObjectPool.DespawnFish(fish);
        }

        public void FishHitBird()
        {
            // TODO: implement in FishChild script
            if (_hasHitBird) return;
            _hasHitBird = true;
            // TODO: Play Obstacle VFX
            // Gain Score Modifier x2 for 10 Seconds
        }

        #region >>>---Water---
        public void FishHitWater(Vector3 velocity)
        {
            if (_hasEnteredWater) return;
            if (!_hasEmergedFromWater)
            {
                // TODO: Play Water Exit VFX and SFX (From Water)
                Log("Emerging from water");
                _hasEmergedFromWater = true;
                return;
            }
            
            if (CheckIfCanSkipp(velocity))
            {
                Skip();
                return;
            }
            
            if (velocity.y >= 0) return;
            Log("Entering Water");
            _hasEnteredWater = true;
            // TODO: Play Water Entry VFX and SFX (From Water)
        }
        
        private bool CheckIfCanSkipp(Vector3 velocity)
        {
            var nor = velocity.normalized;
            var attackAngle = AngleBetweenVectors(new Vector3(nor.x,0, nor.z), new Vector3(0, nor.y, 0));
            var ySpeed = velocity.y;
            var fSpeed = Mathf.Abs(velocity.magnitude - ySpeed);
            
            return attackAngle < attackAngleMaximum && fSpeed > ySpeed * fSpeedNeededMultiplier;
        }
        
        private void Skip()
        {
            Log("Skipp fish");
            // TODO: Skipp rigidbody
            // TODO: Skipping SFX and VFX
            // _rigidbody.AddForce(new Vector3(0, ySpeedAmount, 0));
        }
        
        #region >>>***---MathFormulas---
        private static double AngleBetweenVectors(Vector3 v1, Vector3 v2)
        {
            double dotProduct = Vector3.Dot(v1, v2);
            double normV1 = v1.magnitude;
            double normV2 = v2.magnitude;

            var angle = Math.Acos(dotProduct / (normV1 * normV2));
            return ToDegrees(angle);
        }
        
        private static double ToDegrees(double radians)
        {
            return radians * (180.0 / Math.PI);
        }
        #endregion
        #endregion

        #region >>>---Ground---
        public void FishHitGround()
        {
            hasHitGround = true;
            if (hasBeenPunchedSuccessfully || hasBeenPunchedUnsuccessfully)
            {
                var dist = Vector3.Distance(transform.position, _punchedPosition) * fish.FishPool.FishRecord.ScoreMultiplierDistance;
                EventManager.FishScore(dist, hasBeenPunchedSuccessfully);
            }
            Log("De-spawning: hit ground");
            StartCoroutine(DespawnAfterTime(despawnDelay));
        }
        
        private IEnumerator DespawnAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Despawn();
        }
        #endregion

        #region >>>---Player---
        public void FishHitPlayer()
        {
            if (_hasHitPlayer) return;
            // TODO: Play PlayerHit Voice Line
            // TODO: Add Slime shader to camera
            _hasHitPlayer = true;
            EventManager.GainScore(-scoreLostFromPlayerHit);
            ZenMetreManager.Instance.RemoveZen(fish.FishPool.FishRecord.ZenAmount * 2);
        }
        
        public void FishPunched()
        {
            // TODO: Play Fish Punched SFX
            // TODO: Play Fist Impact SFX
            // TODO: Play FishScaleVFX
            // TODO: Play Random Player Hit Fish Voice Line
            hasBeenPunchedSuccessfully = true;
            _punchedPosition = transform.position;
            EventManager.GainScore(baseScoreOfPunch);
            GainZen();
        }
        
        private void GainZen()
        {
            ZenMetreManager.Instance.AddHitZen(fish.FishPool.FishRecord.ZenAmount);
            Log("Zen gained: " + fish.FishPool.FishRecord.ZenAmount);
        }
        #endregion
        #endregion
    }
}