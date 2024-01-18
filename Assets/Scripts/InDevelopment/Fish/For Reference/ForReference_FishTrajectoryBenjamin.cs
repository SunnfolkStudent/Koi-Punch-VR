using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace InDevelopment.Fish.For_Reference
{
    public class ForReference_FishTrajectoryBenjamin : MonoBehaviour
    {
        public Rigidbody ball;
        public Rigidbody fish;
        public Transform estimatedTarget;
        public Transform target;

        public float h = 25;
        // the reason gravity is 18, is due to 18m/s.
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
            // Figure out the vertical velocity and the horizontal velocity - this is needed for the curve the fish will be launched at
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
            Vector3 velocityXZ = displacementXZ / time;

            return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
        }

        LaunchData2 CalculateLaunchData2(float punchForce, Vector3 fishLaunchDir)
        {
            var fishPosition = fish.position;
            var landingPosition = estimatedTarget.position;

            float displacementY = landingPosition.y - fishPosition.y;
            Vector3 displacementXZ =
                new Vector3(landingPosition.x - fishPosition.x, 0, landingPosition.z - fishPosition.z);
            
            // TODO: Figure out where displacementY goes in.
            
            float timeToReachVerticalApex = (2*punchForce*fishLaunchDir.normalized.y)/gravity + (1);
            float timeFromApexToLandingPos = 2;
            float timeFromStartToEnd = timeToReachVerticalApex + timeFromApexToLandingPos;

            Vector3 velocityY2 = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
            Vector3 velocityXZ2 = displacementXZ / timeFromStartToEnd;

            return new LaunchData2(velocityXZ2 + velocityY2 * -Mathf.Sign(gravity), timeFromStartToEnd);
        }

        struct LaunchData2
        {
            public readonly Vector3 InitialVelocity2;
            public readonly float TimeToTarget2;
            
            public LaunchData2(Vector3 initialVelocity2, float timeToTarget2)
            {
                InitialVelocity2 = initialVelocity2;
                TimeToTarget2 = timeToTarget2;
            }
        }

        void DebugDrawPath()
        {
            LaunchData launchData = CalculateLaunchData();
            Vector3 previousDrawPoint = ball.position;
            
            // linePoints are the points the line will be connected through, DrawLine is a series of points to targetPos.
            int linePoints = 30;
            for (int i = 1; i <= linePoints; i++)
            {
                float simulationTime = i / (float)linePoints * launchData.TimeToTarget;
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
