using UnityEngine;

namespace FinalScripts.Fish.Spawning
{
    // Uncomment the below if you need to place SpawnAreas, but otherwise keep commented, cuz it gets performance-heavy.
    // [ExecuteInEditMode]
    [RequireComponent(typeof(LineRenderer))]
    public class SpawnAreaCircle : MonoBehaviour
    {
        [Range(0, 20)] public float spawnAreaRadius;
        private readonly int _segments = 180;
        private LineRenderer _spawnAreaRadiusLine;
        
        private void Start()
        {
            // First, spawnArea LineRenderer and respective component settings.
            _spawnAreaRadiusLine = GetComponent<LineRenderer>();

            _spawnAreaRadiusLine.positionCount = (_segments + 1);
            _spawnAreaRadiusLine.useWorldSpace = false;
            CreatePointsSpawnArea();
        }
        
        private void Update()
        {
            CreatePointsSpawnArea();
        }
        
        private void CreatePointsSpawnArea()
        {
            var angle = 20f;
            
            for (var i = 0; i < (_segments + 1); i++)
            {
                var x = Mathf.Sin (Mathf.Deg2Rad * angle) * spawnAreaRadius;
                var y = Mathf.Cos (Mathf.Deg2Rad * angle) * spawnAreaRadius;
                
                _spawnAreaRadiusLine.SetPosition (i,new Vector3(x,y,0) );
                
                angle += (360f / _segments);
            }
        }
    }
}
