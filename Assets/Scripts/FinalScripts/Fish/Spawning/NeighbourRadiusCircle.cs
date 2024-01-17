using UnityEngine;

namespace FinalScripts.Fish.Spawning
{
    // Uncomment the below if you need to adjust the detection Radius, but otherwise keep commented, cuz it gets performance-heavy.
    // [ExecuteInEditMode]
    [RequireComponent(typeof(LineRenderer))]
    public class NeighbourRadiusCircle : MonoBehaviour
    {
        [Range(0, 10)] public float neighbourDetectionRange = 2f;
        private readonly int _segments = 180;
        private LineRenderer _neighbourRadiusLine;
        
        private void Start()
        { 
            // NeighbourRadius LineRenderer and respective component settings
            _neighbourRadiusLine = GetComponentInChildren<LineRenderer>();
            
            _neighbourRadiusLine.positionCount = (_segments + 1);
            _neighbourRadiusLine.useWorldSpace = false;
            CreatePointsNeighbourRadius();
        }
        
        private void Update()
        {
            CreatePointsNeighbourRadius();
        }
        
        private void CreatePointsNeighbourRadius()
        {
            var angle = 20f;
            
            for (var i = 0; i < (_segments + 1); i++)
            {
                var x = Mathf.Sin (Mathf.Deg2Rad * angle) * neighbourDetectionRange;
                var y = Mathf.Cos (Mathf.Deg2Rad * angle) * neighbourDetectionRange;
                
                _neighbourRadiusLine.SetPosition (i,new Vector3(x,y,0) );
                
                angle += (360f / _segments);
            }
        }
    }
}