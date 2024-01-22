using System;
using System.Collections;
using FinalScripts.Fish.Spawning;
using UnityEngine;

namespace FinalScripts.Fish
{
    public class Fish : MonoBehaviour
    {
        public FishObjectPool.Fish fish { get; set; } // Reference to itself in the FishPool
        private Vector3 _punchedPosition; // Compared with landing position to calculate distance
        private float _startTime;
        
        #region ---States---
        [HideInInspector] public bool hasBeenPunchedSuccessfully;
        [HideInInspector] public bool hasBeenPunchedUnsuccessfully;
        [HideInInspector] public bool hasHitGround;
        private bool _hasHitPlayer;
        private bool _hasEmergedFromWater;
        private bool _hasEnteredWater;
        private bool _hasHitBird;
        #endregion
        
        #region ---InspectorSettings---
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
            StopCoroutine(DespawnAfterTime(0));
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/FishTalk/KoiTalk", transform.position);
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
            if (transform.position.y > fish.FishPool.FishRecord.FishScrub.despawnAltitude && 
                _startTime > Time.time - fish.FishPool.FishRecord.FishScrub.despawnTime) return;
            Log("De-spawned: either to time or y altitude to low");
            if (hasBeenPunchedSuccessfully || hasBeenPunchedUnsuccessfully)
            {
                var dist = Vector3.Distance(transform.position, _punchedPosition);
                EventManager.FishScore(dist, hasBeenPunchedSuccessfully);
            }
            Despawn();
        }
        
        private void Despawn()
        {
            // TODO: Play Smoke explosion VFX
            FishObjectPool.DespawnFish(fish);
        }

        public void FishHitBird()
        {
            if (_hasHitBird) return;
            _hasHitBird = true;
            FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/FishSlap", transform.position);
            // TODO: Play Obstacle VFX
            EventManager.BonusScore.Invoke(fish.FishPool.FishRecord.FishScrub.scoreFromHittingBird);
        }
        
        #region >>>---Water---
        public void FishHitWater(Vector3 velocity)
        {
            if (_hasEnteredWater) return;
            if (!_hasEmergedFromWater)
            {
                Log("Emerging from water");
                _hasEmergedFromWater = true;
                FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/FishSplash", transform.position);
                // TODO: Play Water Exit VFX
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
            FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/FishSplash", transform.position);
            // TODO: Play Water Entry VFX
        }
        
        private bool CheckIfCanSkipp(Vector3 velocity)
        {
            var nor = velocity.normalized;
            var attackAngle = AngleBetweenVectors(new Vector3(nor.x,0, nor.z), new Vector3(0, nor.y, 0));
            var ySpeed = velocity.y;
            var fSpeed = Mathf.Abs(velocity.magnitude - ySpeed);
            
            return attackAngle < fish.FishPool.FishRecord.FishScrub.attackAngleMaximum && fSpeed > ySpeed * fish.FishPool.FishRecord.FishScrub.fSpeedNeededMultiplier;
        }
        
        private void Skip()
        {
            Log("Skipp fish");
            // TODO: Skipping SFX and VFX
            foreach (var child in fish.Children)
            {
                child.Rigidbody.AddForce(new Vector3(0, fish.FishPool.FishRecord.FishScrub.ySkippSpeedAmount, 0));
            }
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
            if (hasHitGround) return;
            hasHitGround = true;
            FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/FishSlap", transform.position);
            if (hasBeenPunchedSuccessfully || hasBeenPunchedUnsuccessfully)
            {
                var dist = Vector3.Distance(transform.position, _punchedPosition);
                EventManager.FishScore(dist, hasBeenPunchedSuccessfully);
            }
            Log("De-spawning: hit ground");
            StartCoroutine(DespawnAfterTime(fish.FishPool.FishRecord.FishScrub.despawnDelay));
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
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/PlayerHit");
            FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/FishImpact", transform.position);
            // TODO: Add Slime shader to camera
            _hasHitPlayer = true;
            EventManager.PenaltyScore(fish.FishPool.FishRecord.FishScrub.damageAmount);
            ZenMetreManager.Instance.RemoveZen(fish.FishPool.FishRecord.FishScrub.zenLostByHit);
        }
        
        public void FishPunchedSuccessful()
        {
            _punchedPosition = transform.position;
            FMODManager.instance.SelectRandomPunchSound();
            FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/FishImpact", _punchedPosition);
            FMODManager.instance.PlayOneShot("event:/SFX/PlayerSounds/HandSounds/SuccessfulPunch", _punchedPosition);
            // TODO: Play FishScaleVFX
            hasBeenPunchedSuccessfully = true;
            GainZen();
        }

        public void FishPunchedUnsuccessful()
        {
            hasBeenPunchedUnsuccessfully = true;
            FMODManager.instance.PlayOneShot("event:/SFX/PlayerSounds/HandSounds/FailedPunch", transform.position);
        }
        
        private void GainZen()
        {
            ZenMetreManager.Instance.AddHitZen(fish.FishPool.FishRecord.FishScrub.zenGainedFromPunched);
            Log("Zen gained: " + fish.FishPool.FishRecord.FishScrub.zenGainedFromPunched);
        }
        #endregion
        #endregion
    }
}