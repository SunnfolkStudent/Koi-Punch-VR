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
        FishTrajectory.LaunchObjectAtTargetWithInitialSpeed(_rigidbody, fishPosition, playerPosition, fishSpeed);
        FishTrajectory.LaunchObjectAtTargetWithInitialSpeed(_rigidbody, fishPosition, playerPosition, fishSpeed, true); // Optional fifth parameter that chooses alternative tall arc
        
        FishTrajectory.LaunchObjectAtTargetWithInitialAngle(_rigidbody, fishPosition, playerPosition, fishAngle);
        */
        #endregion

        #region ---LaunchOptions---
        public static void LaunchObjectAtTargetWithInitialSpeed(Rigidbody objRigidbody, Vector3 objPos, Vector3 targetPos, float speed, bool tall = false)
        {
            var spacialDifference = SpacialDifference(objPos, targetPos);
            var fishVelocity = TrajectoryVelocityFromSpeedDistanceAltitude(speed, spacialDifference.distance, spacialDifference.altitude, tall);

            LaunchObjectAt(objRigidbody, objPos, targetPos, fishVelocity);
        }
        
        public static void LaunchObjectAtTargetWithInitialAngle(Rigidbody objRigidbody, Vector3 objPos, Vector3 targetPos, float angle)
        {
            var spacialDifference = SpacialDifference(objPos, targetPos);
            var fishVelocity = TrajectoryVelocityFromAngleDistanceAltitude(angle, spacialDifference.distance, spacialDifference.altitude);
            
            LaunchObjectAt(objRigidbody, objPos, targetPos, fishVelocity);
        }
        #endregion

        #region ---LaunchRequirements---
        private static (float distance, float altitude) SpacialDifference(Vector3 objPos, Vector3 targetPos)
        {
            var objectTargetDifference = new Vector2(targetPos.x - objPos.x, targetPos.z - objPos.z);
            var directDistance = Hypotenuse(objectTargetDifference.x, objectTargetDifference.y);
            var altitudeDifference = objPos.y - targetPos.y;
            return (directDistance, -altitudeDifference);
        }

        private static void LaunchObjectAt(Rigidbody objRigidbody, Vector3 objPos, Vector3 targetPos, (float forward, float upwards) fishVelocity)
        {
            var targetDirection = (targetPos - objPos).normalized;
            targetDirection *= fishVelocity.forward;
            objRigidbody.velocity = new Vector3(targetDirection.x, fishVelocity.upwards, targetDirection.z);
        }
        #endregion

        #region ---TrajectoryCalculations---(UnFinished)
        private static (float forward, float upwards) TrajectoryVelocityFromSpeedDistanceAltitude(float speed, float dist, float alt, bool tall)
        {
            var angleInRadians = tall ? (float)Math.Atan((NumberExponent(speed, 2) + Math.Sqrt(NumberExponent(speed, 4) - Gravity * 
                    ((Gravity * NumberExponent(dist, 2)) + (2 * alt * NumberExponent(speed, 2))))) / (Gravity * dist))
                    : (float)Math.Atan((NumberExponent(speed, 2) - Math.Sqrt(NumberExponent(speed, 4) - Gravity * 
                    ((Gravity * NumberExponent(dist, 2)) + (2 * alt * NumberExponent(speed, 2))))) / (Gravity * dist));
            
            var velocityForward = Mathf.Cos(angleInRadians) * speed;
            var velocityUpwards = Mathf.Sin(angleInRadians) * speed;
            
            return (velocityForward, velocityUpwards);
        }
        
        private static (float forward, float upwards) TrajectoryVelocityFromAngleDistanceAltitude(float angle, float dist, float alt)
        {
            // TODO: Fix why certain angles dont work

            var velocityTotal = Mathf.Sqrt((NumberExponent(dist, 2) * Gravity) /
                                   (dist * Mathf.Sin(2 * angle) - 2 * alt * NumberExponent(Mathf.Cos(angle), 2)));
            
            var velocityForward = Mathf.Abs(velocityTotal * Mathf.Cos(angle));
            var velocityUpwards = Mathf.Abs(velocityTotal * Mathf.Sin(angle));
            
            Debug.Log($"angle: {angle}, dist: {dist}, alt: {alt} | " +
                      $"velocityTotal: {velocityTotal}, velocityForward: {velocityForward}, velocityUpwards: {velocityUpwards}");
            
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