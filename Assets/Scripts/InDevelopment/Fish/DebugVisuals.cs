using System;
using System.Collections;
using System.Collections.Generic;
using InDevelopment.Fish.For_Reference;
using InDevelopment.Fish.Trajectory;
using UnityEngine;
using UnityEngine.UIElements;

namespace InDevelopment.Fish
{
    public class DebugInitialLaunchWhenPunched : MonoBehaviour
    {
        public PunchForReference punchScript;
        public Rigidbody fish;
        public Transform endPosition;
        
        public bool enableDebug;
        
        private bool _punchCollidingWithFish;
        private bool _fishTravels;
        
        
        // This is supposed to create a drawn line from the last frame the fish is punched, towards its landing destination. :)
        // TODO: Find startPosition, endPosition.

        private void FixedUpdate()
        {
            if (_punchCollidingWithFish)
            {
                StartCoroutine(CalculateDebugTrajectory());
            }
        }

        IEnumerator CalculateDebugTrajectory()
        {
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = Vector3.zero;

            yield return new WaitWhile(() => _punchCollidingWithFish);
            
            
            
            // Number of points the lines will go through in the "trajectory" before reaching its target...
            int linePoints = 30;
            for (int i = 1; i <= linePoints; i++)
            {
                var fishSpeed = 2f;
                var fishAngle = 2f;
                var fishAltitude = 2f;
                // float simulationTime = i / (float)linePoints * launchData.TimeToTarget;
                
                // The below function will give the total distance the fish is being launched at
                FishTrajectory.RangeFromSpeedAngleAltitude(fishSpeed, fishAngle, fishAltitude);
                Debug.DrawLine(startPos, endPos, Color.green);
            }
            yield return new WaitWhile(() => _fishTravels);
        }
    }
    
    /* public class DebugVisualsContinuous : MonoBehaviour
    {
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
    } */
}
