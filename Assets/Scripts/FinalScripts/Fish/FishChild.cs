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
        // TODO: Transfer LineRenderer to Fish Script, and reference it through the fishChild scripts.
        
        public Fish fish;
        public GameObject landingMarkPrefab;
        [SerializeField] private LineRenderer lineRenderer;
        public LayerMask fishCollisionMask;

        [SerializeField] [Range(10, 100)] private int linePoints = 25;
        [SerializeField] [Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;
        
        private Rigidbody _rbFishPart;
        private bool _rigidbodyFound;
        private Vector3 _startPos;
        private Vector3 _landingPos;

        private float _landingTimer = 1.5f;

        private bool _punched;
        [SerializeField] private bool enableTrajectoryLine;
        [SerializeField] private bool createLandingMark = true;
        [SerializeField] private bool fishAsleep = true;

        #region ---Initialization & Update---
        private void Awake()
        {
            if (TryGetComponent(out Rigidbody rigidbodyPart))
            {
                _rbFishPart = rigidbodyPart;
                _rigidbodyFound = true;
            }

            /*int fishLayer = fish.gameObject.layer;
            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(fishLayer, i))
                {
                    fishCollisionMask |= 1 << i;
                }
            }*/
        }

        void Start()
        {
            // Put the Rigidbody to sleep initially (no physics are being calculated)
            if (fishAsleep && _rigidbodyFound)
            {
                _rbFishPart.Sleep();
            }
        }

        private void Update()
        {
            _landingTimer += Time.deltaTime;
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
                    FishMeetsGround();
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

        private void FishMeetsGround()
        {
            /*if (_landingTimer > 5f && createLandingMark)
            {
                Instantiate(landingMarkPrefab, _rbFishPart.position, Quaternion.identity);
            }*/
            var fishGroundPosition = _rbFishPart.position;
            print($"Distance: {fishGroundPosition - _startPos} | LandingPos: {fishGroundPosition}");
            print($"Distance.magnitude: {(fishGroundPosition - _startPos).magnitude}");
            _landingTimer = 0;
            
            fish.FishHitGround();
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
            
            fish.SimulateTrajectory(fishLaunch);
        }

        #endregion

        #region ---Trajectory---
        private void SimulateTrajectory(Vector3 fishLaunch)
        {
            lineRenderer.enabled = true;
            if (!enableTrajectoryLine)
            {
                lineRenderer.material = null;
            }
            lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
            Vector3 startPosition = _startPos;
            Vector3 startVelocity = fishLaunch;
            int i = 0;
            lineRenderer.SetPosition(i, startPosition);
            for (float time = 0; time < linePoints; time += timeBetweenPoints)
            {
                i++;
                Vector3 point = startPosition + time * startVelocity;
                point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

                lineRenderer.SetPosition(i, point);

                Vector3 lastPosition = lineRenderer.GetPosition(i - 1);

                if (Physics.Raycast(lastPosition, (point - lastPosition).normalized,
                        out RaycastHit hit, (point - lastPosition).magnitude, fishCollisionMask))
                {
                    lineRenderer.SetPosition(i, hit.point);
                    lineRenderer.positionCount = i + 1;
                    print($"Estimated Landing Position: {lastPosition}");
                    return;
                }
            }
        }
        #endregion
    }
}