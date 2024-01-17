using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InDevelopment
{
    public class FieldOfView : MonoBehaviour
    {
        public float viewRadius = 15;
        [Range(0, 360)] public float viewAngle = 92;

        public LayerMask spawnAreaMask;
        public LayerMask obstacleMask;

        [HideInInspector] public List<Transform> visibleSpawnAreas = new List<Transform>();

        private void Start()
        {
            StartCoroutine(FindSpawnAreaWithDelay(0.2f));
        }
        
        IEnumerator FindSpawnAreaWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleSpawnAreas();
            }
        }
        
        private void FindVisibleSpawnAreas()
        {
            visibleSpawnAreas.Clear();
            Collider[] spawnAreasInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, spawnAreaMask);

            for (int i = 0; i < spawnAreasInViewRadius.Length; i++)
            {
                Transform spawnArea = spawnAreasInViewRadius[i].transform;
                Vector3 dirToSpawnArea = (spawnArea.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToSpawnArea) < viewAngle / 2)
                {
                    float distanceToSpawnArea = Vector3.Distance(transform.position, spawnArea.position);

                    if (!Physics.Raycast(transform.position, dirToSpawnArea, distanceToSpawnArea, obstacleMask))
                    {
                        visibleSpawnAreas.Add(spawnArea);
                    }
                }
            }
        }
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}
