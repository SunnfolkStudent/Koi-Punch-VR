using System;
using Unity.Mathematics;
using UnityEngine;

namespace InDevelopment.Fish.Trajectory
{
    public static class FishTrajectory
    {
        private const float Gravity = 9.81f;

        #region ---HowToUse---
        /*
        FishTrajectory.LaunchObjectAtTargetWithInitialSpeed(_rigidbody, fishPosition, targetPosition, fishSpeed);
        
        FishTrajectory.LaunchObjectAtTargetWithInitialAngle(_rigidbody, fishPosition, targetPosition, fishAngle);
        */
        #endregion

        #region ---LaunchOptions---
        public static void LaunchObjectAtTargetWithInitialSpeed(Rigidbody objRigidbody, Vector3 objPos, Vector3 targetPos, float speed)
        {
            var spacialDifference = SpacialDifference(objPos, targetPos);
            var fishVelocity = TrajectorySpeedFromAngleDistanceAltitude(speed, spacialDifference.distance, spacialDifference.altitude);
            
            LaunchObjectAt(objRigidbody, objPos, targetPos, fishVelocity);
        }
        
        public static void LaunchObjectAtTargetWithInitialAngle(Rigidbody objRigidbody, Vector3 objPos, Vector3 targetPos, float angle)
        {
            var spacialDifference = SpacialDifference(objPos, targetPos);
            var fishVelocity = TrajectoryAngleFromSpeedDistanceAltitude(angle, spacialDifference.distance, spacialDifference.altitude);
            
            LaunchObjectAt(objRigidbody, objPos, targetPos, fishVelocity);
        }
        #endregion

        #region ---LaunchRequirements---
        private static (float distance, float altitude) SpacialDifference(Vector3 objPos, Vector3 targetPos)
        {
            var objectTargetDifference = new Vector2(targetPos.x, targetPos.z) - new Vector2(objPos.x, objPos.z);
            var directDistance = Hypotenuse(objectTargetDifference.x, objectTargetDifference.y);
            var altitudeDifference = objPos.y - targetPos.y;
            return (directDistance, altitudeDifference);
        }

        private static void LaunchObjectAt(Rigidbody objRigidbody, Vector3 objPos, Vector3 targetPos, (float forward, float upwards) fishVelocity)
        {
            var targetDirection = (targetPos - objPos).normalized;
            targetDirection *= fishVelocity.forward;
            objRigidbody.velocity = new Vector3(targetDirection.x, fishVelocity.upwards, targetDirection.z);
        }
        #endregion

        #region ---TrajectoryCalculations---(TODO)
        private static (float forward, float upwards) TrajectorySpeedFromAngleDistanceAltitude(float speed, float dist, float alt)
        {
            var angleInRadians = (float)Math.Atan((NumberExponent(speed, 2) + Math.Sqrt(NumberExponent(speed, 4) - Gravity * 
                                ((Gravity * NumberExponent(dist, 2)) + (2 * alt * NumberExponent(speed, 2))))) / (Gravity * dist));
            
            var velocityForward = Mathf.Cos(angleInRadians) * speed;
            var velocityUpwards = Mathf.Sin(angleInRadians) * speed;
            
            return (velocityForward, velocityUpwards);
        }
        
        private static (float forward, float upwards) TrajectoryAngleFromSpeedDistanceAltitude(float angle, float dist, float alt)
        {
            // TODO: Rewrite formula in "TrajectoryAngleFromSpeedDistanceAltitude" to find "speed" from "angle" parameter
            
            var speed = 5;
            var velocityForward = speed;
            var velocityUpwards = speed;
            
            return (velocityForward, velocityUpwards);
        }
        #endregion

        #region ---Math Formulas---
        
        private static float Hypotenuse(float a, float b)
        {
            return math.sqrt(a * a + b * b);
        }

        private static float NumberExponent(float number, int exponent)
        {
            var num = number;
            for (var i = 1; i < exponent; i++) number *= num;
            return number;
        }

        #endregion
    }
}