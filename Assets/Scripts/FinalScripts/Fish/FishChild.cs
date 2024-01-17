using UnityEngine;

namespace FinalScripts.Fish
{
    public class FishChild : MonoBehaviour, IPunchable
    {
        public Fish fish;
        private Rigidbody _rigidbody;
        [SerializeField] private bool showDebugLines = true;
        
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
            
            CalculatePunchedTrajectory(punchForce, fishLaunchDir:direction);
        }

        private void CalculatePunchedTrajectory(float punchForce, Vector3 fishLaunchDir)
        {
            // Use the following variables for calculating trajectory:
            // Landing position = origin + time * velocity a.k.a. _rigidbody.position + timeToTarget * punchForce.
            
            // Velocity (v) = punchForceMultiplier,
            // Direction (d) = cubeLaunchDir.normalized,
            // Angle = arcSin(direction.y)
            // Gravity = 9.81 = Newton.
        
            // Time = (2*v*sin (a))/gravity.
        
            // Sin x where is x = arcSin x, a.k.a. "sin (arcSin x)" is just x.
            // Therefore we can just write in fishLaunchDir.normalized.y.
            // t = (2*punchForce*fishLaunchDir.normalized.y)/9.81.
            
            // does this gravity have to match the fish's own gravity? Probably.
            var gravity = 9.81f;
            var timeToTarget = (2 * punchForce * fishLaunchDir.normalized.y) / gravity;

            var startPosFish = transform.position;
            var landingPos = startPosFish + punchForce * fishLaunchDir.normalized * timeToTarget 
                             - 0.5f * gravity * Mathf.Pow(timeToTarget, 2) * Vector3.up;

            if (showDebugLines)
            {
                Debug.DrawLine(startPosFish, landingPos, Color.yellow, timeToTarget);
            }
        }
    }
}