using System;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace InDevelopment.Fish.Trajectory
{
    public interface IPunchable2
    {
        void PunchObject(BoxHandVelocity boxHandVelScript);
    }
    
    public class PunchedFishScript : MonoBehaviour, IPunchable2
    {
        public FinalScripts.Fish.Fish fish;
        public GameObject travelMarkPrefab;
        public GameObject landingMarkPrefab;
        public LineRenderer lineRenderer;
        public LayerMask fishCollisionMask;

        [SerializeField] [Range(10, 100)] private int linePoints = 25;
        [SerializeField] [Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;

        private Rigidbody _rbFish;
        private Vector3 _startPos;
        private Vector3 _landingPos;
        
        private float _timer;
        private float _landingTimer = 1.5f;

        private bool _punched;
        [SerializeField] private bool fishAsleep;

        // Gravity constant
        private const float Gravity = -9.81f;

        private void Awake()
        {
            _rbFish = GetComponent<Rigidbody>();

            int fishLayer = fish.gameObject.layer;
            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(fishLayer, i))
                {
                    fishCollisionMask |= 1 << i;
                }
                
            }
        }

        void Start()
        {
            _startPos = _rbFish.position;
            print($"StartPos in worldSpace: {_startPos} | StartPos Reset: {_startPos - _startPos}");

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

        private void Update()
        {
            _timer += Time.deltaTime;
            _landingTimer += Time.deltaTime;
        }

        private void OnCollisionEnter(Collision other)
        {
            switch (other.transform.tag)
            {
                case "Player":
                    fish.FishHitPlayer();
                    break;
                case "Ground":
                    // fish.FishHitGround();
                    FishHitGround();
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
                    // We're updating the startPos based on when fish leaves the punch.
                    _startPos = _rbFish.position;
                    print($"New StartPos in worldSpace: {_startPos} | StartPos Reset: {_startPos - _startPos}");
                    break;
                case "RightFist":
                    HapticManager.rightFishPunch = false;
                    _startPos = _rbFish.position;
                    print($"New StartPos in worldSpace: {_startPos} | StartPos Reset: {_startPos - _startPos}");
                    break;
            }
            
        }

        private void FishHitGround()
        {
            lineRenderer.enabled = false;
            if (_landingTimer > 1f)
            {
                Instantiate(landingMarkPrefab, _rbFish.position, Quaternion.identity);
            }
            var fishGroundPosition = _rbFish.position;
            print($"Distance: {fishGroundPosition - _startPos} | LandingPos: {fishGroundPosition}");
            print($"Distance.magnitude: {(fishGroundPosition - _startPos).magnitude}");
            _landingTimer = 0;
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

            if (fish.hasBeenPunchedSuccessfully || fish.hasHitGround)
            {
                fish.Log("Punch does not qualify as it has already been punched");
                return;
            }

            fish.FishPunchedSuccessful();

            var direction = punchVelocity.normalized;
            var punchForce = punchVelocity.magnitude * fish.punchVelMultiplier;

            var forceDebuff = (punchVelocity.magnitude - fish.successfulPunchThreshold) + 0.70f;
            forceDebuff = forceDebuff >= 1f ? 1f : forceDebuff;
            punchForce *= forceDebuff;

            var fishLaunch = direction * punchForce;

            fish.Log($"PunchForce: {punchForce} | Direction: {direction} | Debuff: {forceDebuff}");
            _rbFish.AddForce(fishLaunch, ForceMode.VelocityChange);
            Debug.Log($"Fish Self-Launch-Force: {fishLaunch}");

            SimulateTrajectory(fishLaunch);
        }
        
        // TODO: Get the right variables updated from when launching fish.
        private void SimulateTrajectory(Vector3 fishLaunch)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
            Vector3 startPosition = _startPos;
            Vector3 startVelocity = fishLaunch / _rbFish.mass; 
            int i = 0;
            lineRenderer.SetPosition(i, startPosition);
            for (float time = 0; time < linePoints; time += timeBetweenPoints)
            {
                i++;
                Vector3 point = startPosition + time * startVelocity;
                point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

                lineRenderer.SetPosition(i, point);

                Vector3 lastPosition = lineRenderer.GetPosition(i - 1);

                if (Physics.Raycast(lastPosition, (point-lastPosition).normalized, 
                        out RaycastHit hit, (point - lastPosition).magnitude, fishCollisionMask))
                {
                    lineRenderer.SetPosition(i, hit.point);
                    lineRenderer.positionCount = i + 1;
                    return;
                }
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
