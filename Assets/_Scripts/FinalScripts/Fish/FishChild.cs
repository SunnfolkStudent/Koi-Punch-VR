using Unity.Mathematics;
using UnityEngine;

namespace FinalScripts.Fish
{
    public class FishChild : MonoBehaviour, IPunchable
    {
        public Fish fish;
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        #region ---Collision---
        private void OnCollisionEnter(Collision other)
        {
            switch (other.transform.tag)
            {
                case "MainCamera":
                    fish.HitPlayer();
                    break;
                case "Ground":
                    fish.HitGround();
                    break;
                case "Bird":
                    fish.HitBird();
                    break;
                case "LeftFist":
                    HapticManager.leftFishPunch = true;
                    break;
                case "RightFist":
                    HapticManager.rightFishPunch = true;
                    break;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                fish.HitWater(_rigidbody.velocity);
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
        
        #region ---IPunchable---
        public void PunchObject(ControllerManager controllerManager, string fistUsed)
        {
            var v = fistUsed == "LeftFist" ? controllerManager.leftControllerVelocity : controllerManager.rightControllerVelocity;
            if (math.abs(v.magnitude) >= fish.fish.FishPool.FishRecord.FishScrub.successfulPunchThreshold) PunchObject(v);
            else
            {
                fish.PunchedUnsuccessful();
                fish.Log("Punch Velocity was too weak");
            }
        }

        private void PunchObject(Vector3 velocity)
        {
            if (fish.hasBeenPunchedSuccessfully || fish.hasHitGround)
            {
                fish.Log("Punch does not qualify as it has already been punched or hit the ground");
                return;
            }
            
            fish.PunchedSuccessful();
            
            var direction = velocity.normalized;
            var punchForce = velocity.magnitude * fish.fish.FishPool.FishRecord.FishScrub.punchVelMultiplier;
            
            var forceDebuff = (velocity.magnitude - fish.fish.FishPool.FishRecord.FishScrub.successfulPunchThreshold) + 0.70f;
            forceDebuff = forceDebuff >= 1f ?  1f : forceDebuff;
            punchForce *= forceDebuff;
            
            var fishLaunch = direction * punchForce;

            fish.Log($"PunchForce: {punchForce} | Direction: {direction} | Debuff: {forceDebuff}");
            _rigidbody.AddForce(fishLaunch, ForceMode.VelocityChange);
        }
        #endregion
    }
}