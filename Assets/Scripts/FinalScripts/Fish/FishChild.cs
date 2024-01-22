using System;
using Unity.Mathematics;
using UnityEngine;

namespace FinalScripts.Fish
{
    public class FishChild : MonoBehaviour, IPunchable
    {
        public Fish fish;
        public EstimatedTrajectory estimatedTrajectoryScript;
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            estimatedTrajectoryScript = fish.estimatedTrajectoryScript;
        }

        #region ---Collision---
        private void OnCollisionEnter(Collision other)
        {
            switch (other.transform.tag)
            {
                case "Player":
                    fish.FishHitPlayer();
                    break;
                case "Ground":
                    fish.FishHitGround();
                    break;
                case "Bird":
                    fish.FishHitBird();
                    break;
                case "LeftFist":
                    HapticManager.leftFishPunch = true;
                    break;
                case "RightFist":
                    HapticManager.rightFishPunch = true;
                    break;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            switch (other.transform.tag)
            {
                case "LeftFist":
                    HapticManager.leftFishPunch = false;
                    break;
                case "RightFist":
                    HapticManager.rightFishPunch = false;
                    break;
            }
        }
        #endregion
        
        #region ---Trigger---
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                fish.FishHitWater(_rigidbody.velocity);
            }
        }
        #endregion
        
        #region ---IPunchable---
        public void PunchObject(ControllerManager controllerManager, string fistUsed)
        {
            var v = fistUsed == "LeftFist" ? controllerManager.leftControllerVelocity : controllerManager.rightControllerVelocity;
            if (math.abs(v.magnitude) >= fish.fish.FishPool.FishRecord.FishScrub.successfulPunchThreshold) LaunchObject(v);
            else
            {
                fish.FishPunchedUnsuccessful();
                fish.Log("Punch Velocity was too weak");
            }
        }

        private void LaunchObject(Vector3 velocity)
        {
            if (fish.hasBeenPunchedSuccessfully || fish.hasHitGround)
            {
                fish.Log("Punch does not qualify as it has already been punched or hit the ground");
                return;
            }
            
            fish.FishPunchedSuccessful();
            
            var direction = velocity.normalized;
            var punchForce = velocity.magnitude * fish.fish.FishPool.FishRecord.FishScrub.punchVelMultiplier;
            
            var forceDebuff = (velocity.magnitude - fish.fish.FishPool.FishRecord.FishScrub.successfulPunchThreshold) + 0.70f;
            forceDebuff = forceDebuff >= 1f ?  1f : forceDebuff;
            punchForce *= forceDebuff;
            
            var fishLaunch = direction * punchForce;

            fish.Log($"PunchForce: {punchForce} | Direction: {direction} | Debuff: {forceDebuff}");
            _rigidbody.AddForce(fishLaunch, ForceMode.VelocityChange);
            
            estimatedTrajectoryScript.SimulateTrajectory(fishLaunch);
        }
        #endregion
    }
}