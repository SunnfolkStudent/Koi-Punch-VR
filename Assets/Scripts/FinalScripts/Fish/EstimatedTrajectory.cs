using UnityEngine;
using UnityEngine.Serialization;

namespace FinalScripts.Fish
{
    public class EstimatedTrajectory : MonoBehaviour
    {
        public Fish fish;
        public GameObject landingMarkPrefab;
        [SerializeField] private LineRenderer lineRenderer;
        public LayerMask fishCollisionMask;
        
        [SerializeField] [Range(10, 100)] private int linePoints = 30;
        [SerializeField] [Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.075f;

        private Rigidbody _rbFish;
        private Vector3 _startPos;
        private Vector3 _landingPos;
        
        [Header("Enable visibility of trajectory line:")]
        [SerializeField] private bool enableTrajectoryLine = true;
        
        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;

            int fishLayer = gameObject.layer;
            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(fishLayer, i))
                {
                    fishCollisionMask |= 1 << i;
                }
            }
        }
        
        public void FishMeetsGround()
        {
            lineRenderer.enabled = false;
            var fishGroundPosition = transform.position;
            print($"Distance: {fishGroundPosition - _startPos} | LandingPos: {fishGroundPosition}");
            print($"Distance.magnitude: {(fishGroundPosition - _startPos).magnitude}");
        }
        
        public void SimulateTrajectory(Vector3 fishLaunch, Vector3 startPosition)
        {
            lineRenderer.enabled = true;
            _startPos = startPosition;
            print($"StartPos in worldSpace: {_startPos} | StartPos Reset: {_startPos - _startPos}");
            
            if (!enableTrajectoryLine)
            {
                lineRenderer.material = null;
            }
            lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
            int i = 0;
            lineRenderer.SetPosition(i, startPosition);
            for (float time = 0; time < linePoints; time += timeBetweenPoints)
            {
                i++;
                Vector3 point = startPosition + time * fishLaunch;
                point.y = startPosition.y + fishLaunch.y * time + (Physics.gravity.y / 2f * time * time);

                print("EstimatedPoint Y" + point.y);
                print("Starting Position Y:" + startPosition.y);
                
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