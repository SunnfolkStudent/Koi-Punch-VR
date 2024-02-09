using FinalScripts.Fish.Spawning.RandomWeightedTables;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Serialization;

namespace FinalScripts.Fish.Spawning
{
    // Uncomment the below if you need to adjust the detection Radius, but otherwise keep commented, cuz it gets performance-heavy.
    // [ExecuteInEditMode]
    [RequireComponent(typeof(LineRenderer))]
    public class NeighbourRadiusCircle : MonoBehaviour
    {
        [Header("The detection radius have to be in the center of other spawnAreas:")]
        [Header("It also has to be adjusted from the object FishSpawnManager:")]
        [SerializeField] [Range(0, 20)] private float neighbourDetectionRange = 7f;
        private readonly int _segments = 180;
        private LineRenderer _neighbourRadiusLine;
        private GameObject _fishSpawnManager;
        private FishSpawnAreas _fishSpawnAreas;
        
        private void Start()
        { 
            // TODO: Optimize this script - to avoid searching for tag and making the float public. (do this if you have nothing else to do)
            
            _neighbourRadiusLine = GetComponentInChildren<LineRenderer>();
            _fishSpawnManager = GameObject.FindGameObjectWithTag("FishSpawnManager");
            _fishSpawnAreas = _fishSpawnManager.GetComponent<FishSpawnAreas>();
            
            _neighbourRadiusLine.positionCount = (_segments + 1);
            _neighbourRadiusLine.useWorldSpace = false;
            CreatePointsNeighbourRadius();
        }
        
        private void Update()
        {
            neighbourDetectionRange = _fishSpawnAreas.neighborDistanceSearchRadius;
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