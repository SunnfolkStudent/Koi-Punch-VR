using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace InDevelopment.Fish.Trajectory
{
    public static class FishTrajectory
    {
        // TODO: When using speed for trajectory stop too low speeds from creating an error
        // TODO: When using angle for trajectory fix why 3% chance of error
        private static readonly float Gravity =  -Physics.gravity.y;
        
        #region ---LaunchOptions---
        public static void LaunchObjectAtTargetWithInitialSpeed(IEnumerable<Rigidbody> objRigidbody, Vector3 objPos, Vector3 targetPos, float speed, bool tall = false)
        {
            var spacialDifference = SpacialDifference(objPos, targetPos);
            var fishVelocity = TrajectoryVelocityFromSpeedDistanceAltitude(speed, spacialDifference.distance, spacialDifference.altitude, tall);

            LaunchObjectAt(objRigidbody, objPos, targetPos, fishVelocity);
        }
        
        public static void LaunchObjectAtTargetWithInitialAngle(IEnumerable<Rigidbody> objRigidbody, Vector3 objPos, Vector3 targetPos, float angle)
        {
            var spacialDifference = SpacialDifference(objPos, targetPos);
            var fishVelocity = TrajectoryVelocityFromAngleDistanceAltitude(angle, spacialDifference.distance, spacialDifference.altitude);
            
            LaunchObjectAt(objRigidbody, objPos, targetPos, fishVelocity);
        }
        
        public static void LaunchObjectAtTargetWithPeakHeight(IEnumerable<Rigidbody> objRigidbody, Vector3 objPos, Vector3 targetPos, float height)
        {
            var spacialDifference = SpacialDifference(objPos, targetPos);
            var fishVelocity = CalculateLaunchData(height, spacialDifference.distance, spacialDifference.altitude);
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

        private static void LaunchObjectAt(IEnumerable<Rigidbody> objRigidbody, Vector3 objPos, Vector3 targetPos, (float velocityForward, float velocityUpwards) fishVelocity)
        {
            var targetDirection = (targetPos - objPos).normalized;
            targetDirection *= fishVelocity.velocityForward;
            foreach (var rigidbody in objRigidbody)
            {
                rigidbody.velocity = new Vector3(targetDirection.x, fishVelocity.velocityUpwards, targetDirection.z);
            }
        }
        #endregion
        
        #region ---TrajectoryCalculations---
        private static (float velocityForward, float velocityUpwards) TrajectoryVelocityFromSpeedDistanceAltitude(float speed, float dist, float alt, bool tall)
        {
            var angleInRadians = tall ? (float)Math.Atan((NumberExponent(speed, 2) + Math.Sqrt(NumberExponent(speed, 4) - Gravity * 
                    ((Gravity * NumberExponent(dist, 2)) + (2 * alt * NumberExponent(speed, 2))))) / (Gravity * dist))
                    : (float)Math.Atan((NumberExponent(speed, 2) - Math.Sqrt(NumberExponent(speed, 4) - Gravity * 
                    ((Gravity * NumberExponent(dist, 2)) + (2 * alt * NumberExponent(speed, 2))))) / (Gravity * dist));
            
            var velocityForward = Mathf.Cos(angleInRadians) * speed;
            var velocityUpwards = Mathf.Sin(angleInRadians) * speed;
            
            Debug.Log($"speed: {speed}, dist: {dist}, alt: {alt} | velocityForward: {velocityForward}, velocityUpwards: {velocityUpwards}");
            
            return (velocityForward, velocityUpwards);
        }
        
        private static (float velocityForward, float velocityUpwards) TrajectoryVelocityFromAngleDistanceAltitude(float angle, float dist, float alt)
        {
            var velocityTotal = Mathf.Sqrt((NumberExponent(dist, 2) * Gravity) / 
                                           (dist * math.abs(Mathf.Sin(2 * angle)) - 2 * alt * NumberExponent(math.abs(Mathf.Cos(angle)), 2)));
            
            var velocityForward = math.abs(velocityTotal * Mathf.Cos(angle));
            var velocityUpwards = math.abs(velocityTotal * Mathf.Sin(angle));
            
            Debug.Log($"angle: {angle}, dist: {dist}, alt: {alt} | velocityForward: {velocityForward}, velocityUpwards: {velocityUpwards}");
            
            return (velocityForward, velocityUpwards);
        }
        
        private static (float velocityForward, float velocityUpwards) CalculateLaunchData(float height, float dist, float alt)
        {
            if (height < alt)
            {
                Debug.LogWarning("Peak trajectory height set lower than target altitude");
                height = alt + 1;
            }
            var velocityForward = Mathf.Sqrt(-2 * -Gravity * height);
            var velocityUpwards = dist / (Mathf.Sqrt(-((2 * height) / -Gravity)) + Mathf.Sqrt((2 * (alt - height)) / -Gravity));
            
            Debug.Log($"height: {height}, dist: {dist}, alt: {alt} | velocityForward: {velocityForward}, velocityUpwards: {velocityUpwards}");
            
            return (velocityUpwards, velocityForward);
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