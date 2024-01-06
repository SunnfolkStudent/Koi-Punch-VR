using UnityEngine;

namespace InDevelopment.Fish
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(LineRenderer))]
    public class SpawnAreaCircle : MonoBehaviour
    {
        [Range(0.1f, 20)] public float spawnAreaRadius;
        
        private readonly int _segments = 180;
        
        LineRenderer _line;

        void Start ()
        {
            _line = gameObject.GetComponent<LineRenderer>();

            _line.positionCount = (_segments + 1);
            _line.useWorldSpace = false;
            CreatePoints ();
        }

        private void Update()
        {
            CreatePoints();
        }

        void CreatePoints ()
        {
            float x;
            float y;
            // float z;

            float angle = 20f;

            for (int i = 0; i < (_segments + 1); i++)
            {
                x = Mathf.Sin (Mathf.Deg2Rad * angle) * spawnAreaRadius;
                y = Mathf.Cos (Mathf.Deg2Rad * angle) * spawnAreaRadius;

                _line.SetPosition (i,new Vector3(x,y,0) );

                angle += (360f / _segments);
            }
        }
    }
}
