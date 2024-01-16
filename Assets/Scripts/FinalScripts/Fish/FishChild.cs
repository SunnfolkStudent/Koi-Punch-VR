using System;
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

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Ground"))
                fish.FishHitGround();
            
            if (other.transform.CompareTag("LeftFist"))
                HapticManager.leftFishPunch = true;
            if (other.transform.CompareTag("RightFist"))
                HapticManager.rightFishPunch = true;
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.transform.CompareTag("LeftFist"))
                HapticManager.leftFishPunch = false;
            
            if (other.transform.CompareTag("RightFist"))
                HapticManager.rightFishPunch = false;
        }

        public void PunchObject(ControllerManager controllerManager, string fistUsed)
        {
            var v = fistUsed == "LeftFist"
                ? controllerManager.leftControllerVelocity : controllerManager.rightControllerVelocity;
            
            if (v.magnitude >= fish.velocityNeededForSuccessfulHit)
                PunchObject(v);
            else 
                fish.Log("Punch Velocity was too weak");
        }

        private void PunchObject(Vector3 velocity)
        {
            if (fish.hasBeenPunched || fish.hasHitGround)
            { fish.Log("Punch does not qualify as it has already been punched or hit the ground"); return; }
            
            fish.FishPunched();
            
            var direction = velocity.normalized;
            var punchForce = velocity.magnitude * fish.punchVelMultiplier;
            
            var forceDebuff = (velocity.magnitude - fish.velocityNeededForSuccessfulHit) + 0.70f;
            forceDebuff = forceDebuff >= 1f ?  1f : forceDebuff;
            punchForce *= forceDebuff;
            fish.Log("Force Debuff: " + forceDebuff);
            
            var fishLaunch = direction * punchForce;

            _rigidbody.AddForce(fishLaunch, ForceMode.VelocityChange);
            fish.Log("Punched with Force of " + punchForce + "\nand a Direction of " + direction);
        }
    }
}