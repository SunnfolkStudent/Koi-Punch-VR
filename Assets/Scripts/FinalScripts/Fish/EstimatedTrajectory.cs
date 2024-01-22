using UnityEngine;

namespace FinalScripts.Fish
{
    public class EstimatedTrajectory : MonoBehaviour
    {
        public Fish fish;
        public GameObject landingMarkPrefab;
        [SerializeField] private LineRenderer lineRenderer;
        public LayerMask fishCollisionMask;
        
        [SerializeField] [Range(10, 100)] private int linePoints = 25;
        [SerializeField] [Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;

        private Rigidbody _rbFish;
        private Vector3 _startPos;
        private Vector3 _landingPos;
        
        private float _landingTimer = 1.5f;
        
        [SerializeField] private bool createLandingMark = true;
        [SerializeField] private bool enableTrajectoryLine = true;
        
        private void Awake()
        {
            _rbFish = GetComponent<Rigidbody>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;

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
        }
        
        private void FixedUpdate()
        {
            _landingPos += _rbFish.position;
        }

        private void Update()
        {
            _landingTimer += Time.deltaTime;
        }
        
        public void FishMeetsGround()
        {
            lineRenderer.enabled = false;
            if (_landingTimer > 5f && createLandingMark)
            {
                Instantiate(landingMarkPrefab, _rbFish.position, Quaternion.identity);
            }
            var fishGroundPosition = _rbFish.position;
            print($"Distance: {fishGroundPosition - _startPos} | LandingPos: {fishGroundPosition}");
            print($"Distance.magnitude: {(fishGroundPosition - _startPos).magnitude}");
            _landingTimer = 0;
            
            fish.FishHitGround();
        }
        
        private void SimulateTrajectory(Vector3 fishLaunch)
        {
            lineRenderer.enabled = true;
            if (!enableTrajectoryLine)
            {
                lineRenderer.material = null;
            }
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
    }
}
