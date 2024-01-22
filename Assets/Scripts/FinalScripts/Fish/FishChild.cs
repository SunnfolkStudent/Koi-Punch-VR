using InDevelopment.Fish.Trajectory;
using Unity.Mathematics;
using UnityEngine;

namespace FinalScripts.Fish
{
    public interface IPunchable
     {
         void PunchObject(ControllerManager controllerManager, string fistUsed);
     }
    public class FishChild : MonoBehaviour, IPunchable
    {
        public Fish fish;
        
        private Rigidbody _rbFishPart;
        private bool _rigidbodyFound;
        private Vector3 _startPos;
        private Vector3 _landingPos;

        private bool _punched;
        [SerializeField] private bool fishAsleep = true;

        #region ---Initialization & Update---
        private void Awake()
        {
            if (TryGetComponent(out Rigidbody rigidbodyPart))
            {
                _rbFishPart = rigidbodyPart;
                _rigidbodyFound = true;
            }
        }

        void Start()
        {
            if (fishAsleep && _rigidbodyFound)
            {
                _rbFishPart.Sleep();
            }
        }
        
        #endregion
        
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
                    // Print the velocity of the punch on collision
                    Debug.Log(
                        $"Fish Collision Velocity: {other.relativeVelocity} | CollisionForce: {other.relativeVelocity.magnitude}");
                    break;
                case "RightFist":
                    HapticManager.rightFishPunch = true;
                    // Print the velocity of the punch on collision
                    Debug.Log(
                        $"Fish Collision Velocity: {other.relativeVelocity} | CollisionForce: {other.relativeVelocity.magnitude}");
                    break;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            switch (other.transform.tag)
            {
                case "LeftFist":
                    HapticManager.leftFishPunch = false;
                    // We're updating the startPos based on when fish leaves the punch.
                    if (_rigidbodyFound)
                    {
                        _startPos = _rbFishPart.position;
                        print($"New StartPos in worldSpace: {_startPos} | StartPos Reset: {_startPos - _startPos}"); 
                    }
                    break;
                case "RightFist":
                    HapticManager.rightFishPunch = false;
                    // We're updating the startPos based on when fish leaves the punch.
                    if (_rigidbodyFound)
                    {
                        _startPos = _rbFishPart.position;
                        print($"New StartPos in worldSpace: {_startPos} | StartPos Reset: {_startPos - _startPos}"); 
                    }
                    break;
            }
        }

        #endregion

        #region ---Trigger---

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                fish.FishHitWater(_rbFishPart.velocity);
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
            if (fishAsleep)
            {
                _rbFishPart.WakeUp();
                fishAsleep = false;
            }

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
            _rbFishPart.AddForce(fishLaunch, ForceMode.VelocityChange);
            Debug.Log($"Fish Self-Launch-Force: {fishLaunch}");
            
            // fish.SimulateTrajectory(fishLaunch);
        }

        #endregion
    }
}