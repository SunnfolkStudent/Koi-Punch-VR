using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace InDevelopment.Fish.For_Reference
{
    public class ForReference_FishTrajectoryBenjamin : MonoBehaviour
    {
        public Rigidbody ball;
        public Transform target;

        public float h = 25;
        public float gravity = -18;

        public bool debugPath;

        // Start is called before the first frame update
        void Start()
        {
            ball.useGravity = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Launch();
            }

            if (debugPath)
            {
                DebugDrawPath();
            }
        }


        void Launch()
        {
            Physics.gravity = Vector3.up * gravity;
            ball.useGravity = true;
            ball.velocity = CalculateLaunchData().InitialVelocity;
            var velocity = TranslateToOtherTrajectories();
        }

        private (float,float) TranslateToOtherTrajectories()
        {
            var initialVelocity = CalculateLaunchData();
            var velocityForward = new Vector2(initialVelocity.InitialVelocity.x, initialVelocity.InitialVelocity.z).magnitude;
            var velocityUpwards = initialVelocity.InitialVelocity.y;
            return (velocityForward, velocityUpwards);
        }
        
        LaunchData CalculateLaunchData()
        {
            var ballPosition = ball.position;
            var targetPosition = target.position;
            float displacementY = targetPosition.y - ballPosition.y;
            Vector3 displacementXZ = new Vector3(targetPosition.x - ballPosition.x, 0, targetPosition.z - ballPosition.z);
            float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
            Vector3 velocityXZ = displacementXZ / time;

            return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
        }

        void DebugDrawPath()
        {
            LaunchData launchData = CalculateLaunchData();
            Vector3 previousDrawPoint = ball.position;

            int resolution = 30;
            for (int i = 1; i <= resolution; i++)
            {
                float simulationTime = i / (float)resolution * launchData.TimeToTarget;
                Vector3 displacement = launchData.InitialVelocity * simulationTime +
                                       Vector3.up * (gravity * simulationTime * simulationTime) / 2f;
                Vector3 drawPoint = ball.position + displacement;
                Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
                previousDrawPoint = drawPoint;
            }
        }

        struct LaunchData
        {
            public readonly Vector3 InitialVelocity;
            public readonly float TimeToTarget;

            public LaunchData(Vector3 initialVelocity, float timeToTarget)
            {
                this.InitialVelocity = initialVelocity;
                this.TimeToTarget = timeToTarget;
            }
        }
    }
}
