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
        
        [Header("Trajectory Line:")]
        // [SerializeField] private LineRenderer lineRenderer; // LineRenderer to help calculate distance for fish
        public LayerMask fishCollisionMask;
        [SerializeField] [Range(10, 100)] private int linePoints = 25;
        [SerializeField] [Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;

        private Rigidbody _rbFish;
        private Vector3 _startPos;
        private Vector3 _landingPos;
        
        private float _landingTimer = 1.5f;
        [SerializeField] private bool enableTrajectoryLine;
        
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
            _rbFish = GetComponentInChildren<Rigidbody>();
            
            if (!TryGetComponent(out Rigidbody rigidbodyPart))
            { _rbFish = GetComponentInChildren<Rigidbody>(); }
            else
            { _rbFish = rigidbodyPart; }
            
            var c = GetComponentsInChildren<Transform>();
            
            foreach (var child in c)
            {
                var fishChild = child.gameObject.AddComponent<FishChild>();
                fishChild.fish = this;
            }
            
            // Put the below lines in Awake() if faulty:
            /*if (TryGetComponent(out LineRenderer lineRenderComponent))
            { lineRenderer = lineRenderComponent; }
            else
            { lineRenderer = GetComponent<LineRenderer>(); }
            
            lineRenderer.enabled = false;*/
            int fishLayer = gameObject.layer;
            
            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(fishLayer, i))
                {
                    fishCollisionMask |= 1 << i;
                }
            }
            _startPos = _rbFish.position;
            print($"StartPos in worldSpace: {_startPos} | StartPos Reset: {_startPos - _startPos}");
        }
        
        private void FixedUpdate()
        {
            _landingPos += _rbFish.position;
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
            _landingTimer += Time.deltaTime;
            // DespawnIfOutOfTimeOrTooLow();
        }
        
        /*private void DespawnIfOutOfTimeOrTooLow()
        {
            if (transform.position.y > fish.FishPool.FishRecord.FishScrub.despawnAltitude && 
                _startTime > Time.time - fish.FishPool.FishRecord.FishScrub.despawnTime) return;
            Log("De-spawned: either to time or y altitude to low");
            Despawn();
        }*/
        
        private void Despawn()
        {
            // TODO: Play Smoke explosion VFX
            FishObjectPool.DespawnFish(fish);
        }

        public void FishHitBird()
        {
            if (_hasHitBird) return;
            _hasHitBird = true;
            // TODO: FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/FishSlap", transform.position);
            // TODO: Play Obstacle VFX
            EventManager.GainScore.Invoke(fish.FishPool.FishRecord.FishScrub.scoreFromHittingBird);
        }
        #endregion
        
        #region >>>---Water---
        public void FishHitWater(Vector3 velocity)
        {
            if (_hasEnteredWater) return;
            if (!_hasEmergedFromWater)
            {
                Log("Emerging from water");
                _hasEmergedFromWater = true;
                // TODO: FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/FishSplash", transform.position);
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
            // TODO: FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/FishSplash", transform.position);
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
            // TODO: FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/FishSlap", transform.position);
            if (hasBeenPunchedSuccessfully || hasBeenPunchedUnsuccessfully)
            {
                var dist = Vector3.Distance(transform.position, _punchedPosition);
                EventManager.FishScore(dist, hasBeenPunchedSuccessfully);
            }
            Log("De-spawning: hit ground");
            // StartCoroutine(DespawnAfterTime(fish.FishPool.FishRecord.FishScrub.despawnDelay));
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
            // TODO: FMODManager.instance.PlayOneShot("event:/SFX/Voice/PlayerHit");
            // TODO: Add Slime shader to camera
            _hasHitPlayer = true;
            EventManager.GainScore(-fish.FishPool.FishRecord.FishScrub.damageAmount);
            ZenMetreManager.Instance.RemoveZen(fish.FishPool.FishRecord.FishScrub.zenLostByHit);
        }
        
        public void FishPunchedSuccessful()
        {
            // TODO: FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/FishImpact", transform.position);
            // TODO: FMODManager.instance.SelectRandomPunchSound();
            // TODO: Play FishScaleVFX
            hasBeenPunchedSuccessfully = true;
            _punchedPosition = transform.position;
            // EventManager.GainScore(fish.FishPool.FishRecord.FishScrub.baseScoreAmount);
            GainZen();
        }

        public void FishPunchedUnsuccessful()
        {
            hasBeenPunchedUnsuccessfully = true;
            // TODO: FMODManager.instance.PlayOneShot("event:/SFX/PlayerSounds/HandSounds/FailedPunch", transform.position);
        }
        
        private void GainZen()
        {
            ZenMetreManager.Instance.AddHitZen(fish.FishPool.FishRecord.FishScrub.zenGainedFromPunched);
            Log("Zen gained: " + fish.FishPool.FishRecord.FishScrub.zenGainedFromPunched);
        }
        #endregion
        
        #region ---SimulateTrajectory---
        /*public void SimulateTrajectory(Vector3 fishLaunch)
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
        }*/
        #endregion
    }
}