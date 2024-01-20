using System;
using InDevelopment.Punch;
using Unity.Mathematics;
using UnityEngine;

namespace InDevelopment.Fish.Trajectory
{
    public interface IPunchable2
    {
        void PunchObject(BoxHandVelocity boxHandVelScript);
    }
    
    public class PunchedFishScript : MonoBehaviour, IPunchable2
    {
        public BoxHandVelocity boxHandVelocityScript;
        public FinalScripts.Fish.Fish fish;
        public GameObject landingMarkPrefab;

        private Rigidbody _rbFish;
        private Vector3 _startPos;
        private Vector3 _landingPos;

        private bool _punched;
        [SerializeField] private bool fishAsleep;

        // Gravity constant
        private const float Gravity = -9.81f;

        private void Awake()
        {
            _rbFish = GetComponent<Rigidbody>();
        }

        void Start()
        {
            _startPos = _rbFish.position;
            print("StartPos in worldSpace:" + _startPos);
            print("StartPos Reset:" + (_startPos - _startPos));

            // Put the Rigidbody to sleep initially (no physics are being calculated)
            if (fishAsleep)
            {
                _rbFish.Sleep();
            }
        }

        private void FixedUpdate()
        {
            _landingPos += _rbFish.position;
        }
        
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

        // TODO: Integrate this finished code into the below comment, which is from the original script.
        // TODO: Actually, make it a copy, cuz this testing set-up is good.
        
        /*public void PunchObject(ControllerManager controllerManager, string fistUsed)
        {
            var v = fistUsed == "LeftFist"
                ? controllerManager.leftControllerVelocity
                : controllerManager.rightControllerVelocity;
            if (math.abs(v.magnitude) >= fish.successfulPunchThreshold) LaunchObject(v);
            else fish.Log("Punch Velocity was too weak");
        }*/

        public void PunchObject(BoxHandVelocity boxHandVelScript)
        {
            var v = boxHandVelScript.punchVelocity;
            if (math.abs(v.magnitude) >= fish.successfulPunchThreshold) LaunchObject(v);
            else fish.Log("Punch Velocity was too weak");
        }

        private void LaunchObject(Vector3 punchVelocity)
        {
            if (fishAsleep)
            {
                _rbFish.WakeUp();
                fishAsleep = false;
            }

            if (fish.hasBeenPunched || fish.hasHitGround)
            {
                fish.Log("Punch does not qualify as it has already been punched");
                return;
            }

            fish.FishPunched();

            var direction = punchVelocity.normalized;
            var punchForce = punchVelocity.magnitude * fish.punchVelMultiplier;

            var forceDebuff = (punchVelocity.magnitude - fish.successfulPunchThreshold) + 0.70f;
            forceDebuff = forceDebuff >= 1f ? 1f : forceDebuff;
            punchForce *= forceDebuff;

            var fishLaunch = direction * punchForce;

            fish.Log($"PunchForce: {punchForce} | Direction: {direction} | Debuff: {forceDebuff}");
            _rbFish.AddForce(fishLaunch, ForceMode.VelocityChange);
            Debug.Log("Fish launches self with a force of:" + fishLaunch);

            SimulateTrajectory(fishLaunch);
        }

        private void SimulateTrajectory(Vector3 fishLaunch)
        {
            // Calculate launch data
            LaunchData launchData = CalculateLaunchData(fishLaunch);

            // TODO: Get the right variables updated from when launching fish.
            // TODO: The fish actually launches itself, the punch does not! Keep this in mind.

            // Simulate trajectory
            float timeInterval = launchData.TimeToTarget / 10f; // Adjust interval as needed
            for (float t = 0; t <= launchData.TimeToTarget; t += timeInterval)
            {
                // Instantiate fish object at calculated position
                var position = transform.position;
                Vector3 fishPosition = new Vector3(position.x, position.y, position.z);
                Instantiate(landingMarkPrefab, fishPosition, Quaternion.identity);
            }
        }

        LaunchData CalculateLaunchData(Vector3 fishLaunch)
        {
            var launchSpeed = fishLaunch.magnitude;
            var fishLaunchNormalized = fishLaunch.normalized;

            // TODO: Fix the below code to have 1 unified xz-Vector.
            float launchAngle = Mathf.Acos(fishLaunchNormalized.x * fishLaunchNormalized.y);

            float radianAngle = Mathf.Deg2Rad * launchAngle;
            float totalTime = (2f * launchSpeed * Mathf.Sin(radianAngle)) / Mathf.Abs(Gravity);
            float maxHeight = (launchSpeed * launchSpeed * Mathf.Pow(Mathf.Sin(radianAngle), 2)) /
                              (2 * Mathf.Abs(Gravity));

            LaunchData launchData = new LaunchData(totalTime, maxHeight);
            return launchData;
        }

        /*LaunchData CalculateLaunchData(float punchSpeed, float launchAngle)
        {
            float radianAngle = Mathf.Deg2Rad * launchAngle;
            float totalTime = (2f * punchSpeed * Mathf.Sin(radianAngle)) / Mathf.Abs(Gravity);
            float maxHeight = (punchSpeed * punchSpeed * Mathf.Pow(Mathf.Sin(radianAngle), 2)) /
                              (2 * Mathf.Abs(Gravity));

            LaunchData launchData = new LaunchData(totalTime, maxHeight);
            return launchData;
        }*/

        private struct LaunchData
        {
            public readonly float TimeToTarget;
            public readonly float ApexHeight;

            public LaunchData(float timeToTarget, float apexHeight)
            {
                TimeToTarget = timeToTarget;
                ApexHeight = apexHeight;
            }
        }
    }
}
