using UnityEngine;

namespace InDevelopment.Fish
{
    [RequireComponent(typeof(LineRenderer))]
    public class SpawnAreaCircle : MonoBehaviour {

        [Range(0.1f, 100f)]
        public float radius = 1.0f;

        [Range(3, 256)]
        public int numSegments = 128;

        void Start ( ) {
            DoRenderer();
        }

        public void DoRenderer ( ) {
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            var lineColor = new Color(0.5f, 0.5f, 0.5f, 1);
            lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Particles/Lit"));
            lineRenderer.startColor = lineRenderer.endColor = lineColor;
            lineRenderer.endColor = lineColor;
            lineRenderer.startWidth = lineRenderer.endWidth = 5f;
            lineRenderer.positionCount = (numSegments + 1);
            lineRenderer.useWorldSpace = false;

            float deltaTheta = (float) (2.0 * Mathf.PI) / numSegments;
            float theta = 0f;

            for (int i = 0 ; i < numSegments + 1 ; i++) {
                float x = radius * Mathf.Cos(theta);
                float z = radius * Mathf.Sin(theta);
                Vector3 pos = new Vector3(x, 0, z);
                lineRenderer.SetPosition(i, pos);
                theta += deltaTheta;
            }
        }
    }
}