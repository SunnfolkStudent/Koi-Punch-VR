using UnityEngine;

namespace InDevelopment.Fish.EditorScripts
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(LineRenderer))]
    public class SpawnAreaCircle : MonoBehaviour
    {
        [Range(0, 20)] public float spawnAreaRadius;
        [SerializeField] private int segments = 180;
        private LineRenderer _line;
        
        private void Start ()
        {
            _line = GetComponent<LineRenderer>();

            _line.positionCount = (segments + 1);
            _line.useWorldSpace = false;
            CreatePoints();
        }
        
        private void Update()
        {
            CreatePoints();
        }
        
        private void CreatePoints()
        {
            var angle = 20f;
            
            for (var i = 0; i < (segments + 1); i++)
            {
                var x = Mathf.Sin (Mathf.Deg2Rad * angle) * spawnAreaRadius;
                var y = Mathf.Cos (Mathf.Deg2Rad * angle) * spawnAreaRadius;
                
                _line.SetPosition (i,new Vector3(x,y,0) );
                
                angle += (360f / segments);
            }
        }
    }
}
