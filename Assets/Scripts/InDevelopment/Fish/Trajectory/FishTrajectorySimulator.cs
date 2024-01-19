using UnityEngine;
using UnityEngine.Serialization;

namespace InDevelopment.Fish.Trajectory
{
    public class FishTrajectorySimulator : MonoBehaviour
    {
        // Fish object to simulate
        public GameObject fishObject;
        public GameObject landingMarkPrefab;

        // Punch parameters
        public float punchSpeed = 10f;
        public float launchAngle = 45f;

        // Gravity constant
        private const float Gravity = -9.81f;

        // Start is called before the first frame update

        public void SimulateTrajectory()
        {
            // Calculate launch data
            LaunchData launchData = CalculateLaunchData();

            // Simulate trajectory
            float timeInterval = launchData.TimeToTarget / 10f; // Adjust interval as needed
            for (float t = 0; t <= launchData.TimeToTarget; t += timeInterval)
            {
                // Calculate position at each time interval
                float x = punchSpeed * Mathf.Cos(Mathf.Deg2Rad * launchAngle) * t;
                float y = punchSpeed * Mathf.Sin(Mathf.Deg2Rad * launchAngle) * t + 0.5f * Gravity * Mathf.Pow(t, 2);
                float z = punchSpeed * Mathf.Cos(Mathf.Deg2Rad * launchAngle) * t;

                // Instantiate fish object at calculated position
                Vector3 fishPosition = new Vector3(x, y, z);
                Instantiate(landingMarkPrefab, fishPosition, Quaternion.identity);
            }
        }

        LaunchData CalculateLaunchData()
        {
            float radianAngle = Mathf.Deg2Rad * launchAngle;
            float totalTime = (2f * punchSpeed * Mathf.Sin(radianAngle)) / Mathf.Abs(Gravity);
            float maxHeight = (punchSpeed * punchSpeed * Mathf.Pow(Mathf.Sin(radianAngle), 2)) / (2 * Mathf.Abs(Gravity));

            LaunchData launchData = new LaunchData(totalTime, maxHeight);
            return launchData;
        }
    }

    public struct LaunchData
    {
        public readonly float TimeToTarget;
        public readonly float ApexHeight;

        public LaunchData(float timeToTarget, float apexHeight)
        {
            TimeToTarget = timeToTarget;
            ApexHeight = apexHeight;
        }
    }
}