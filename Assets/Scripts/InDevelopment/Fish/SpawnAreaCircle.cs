using System;
using UnityEngine;

namespace InDevelopment.Fish
{ 
    [RequireComponent(typeof(LineRenderer))]
    public class SpawnAreaCircle : MonoBehaviour 
    {
        [Range(0,50)]
        public int segments = 50;
        [Range(0,5)]
        public float xradius = 5;
        [Range(0,5)]
        public float yradius = 5;
        LineRenderer _line;

        void Start ()
        {
            _line = gameObject.GetComponent<LineRenderer>();

            _line.positionCount = (segments + 1);
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
            float z;

            float angle = 20f;

            for (int i = 0; i < (segments + 1); i++)
            {
                x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
                y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

                _line.SetPosition (i,new Vector3(x,y,0) );

                angle += (360f / segments);
            }
        }
    }
}