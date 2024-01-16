using System;
using UnityEngine;

namespace FinalScripts.Fish.Spawning
{
    public static class FishTrajectory
    {
        // TODO: When using angle for trajectory fix why 3% chance of error
        
        #region ---Debugging---
        private static bool _isDebugging = true;
        private static void Log(string message)
        {
            if(_isDebugging) Debug.Log(message);
        }
        #endregion
        
        #region ---LaunchRequirements---
        private static Vector2 SpacialDifference(Vector3 objPos, Vector3 targetPos)
        {
            var objectTargetDifference = new Vector2(targetPos.x - objPos.x, targetPos.z - objPos.z);
            var directDistance = Hypotenuse(objectTargetDifference.x, objectTargetDifference.y);
            var altitudeDifference = objPos.y - targetPos.y;
            return new Vector2(directDistance, -altitudeDifference);
        }

        private static float RangeFromVelocityAndAltitude(float velocity, float alt)
        {
            return velocity / -Physics.gravity.y * Mathf.Sqrt(Mathf.Pow(velocity, 2) + 2 * -Physics.gravity.y * alt);
        }
        #endregion
        
        #region ---TrajectoryCalculations---
        public static Vector2 TrajectoryVelocity2DFromPeakHeight(Vector3 objPos, Vector3 targetPos,float height)
        {
            var displacement = SpacialDifference(objPos, targetPos);
            if (height - 1 < displacement.y)
            {
                Debug.LogWarning("Peak trajectory height set lower than target altitude");
                height = displacement.y + 1;
            }
            var velocityForward = Mathf.Sqrt(-2 * Physics.gravity.y * height);
            var velocityUpwards = displacement.x / (Mathf.Sqrt(-((2 * height) / Physics.gravity.y)) + Mathf.Sqrt((2 * (displacement.y - height)) / Physics.gravity.y));

            Log($"Fish Launched With: Height: {height}, Dist: {displacement.x}, Alt: {displacement.y} | " + 
                $"VelocityForward: {velocityForward}, VelocityUpwards: {velocityUpwards}");
            
            return new Vector2(velocityUpwards, velocityForward);
        }
        
        public static Vector2 TrajectoryVelocity2DFromInitialAngle(Vector3 objPos, Vector3 targetPos, float angle)
        {
            var displacement = SpacialDifference(objPos, targetPos);
            var velocityTotal = Math.Sqrt((Math.Pow(displacement.x, 2) * -Physics.gravity.y) / 
                                          (displacement.x * Math.Abs(Mathf.Sin(2 * angle)) - 2 * 
                                              displacement.y * Math.Pow(Math.Abs(Mathf.Cos(angle)), 2)));
            
            var velocityForward = (float)Math.Abs(velocityTotal * Mathf.Cos(angle));
            var velocityUpwards = (float)Math.Abs(velocityTotal * Mathf.Sin(angle));
            
            Log($"Fish Launched With: Angle: {angle}, Dist: {displacement.x}, Alt: {displacement.y} | " +
                $"VelocityForward: {velocityForward}, VelocityUpwards: {velocityUpwards}");
            
            return new Vector2(velocityForward, velocityUpwards);
        }
        
        public static Vector2 TrajectoryVelocity2DFromInitialSpeed(Vector3 objPos, Vector3 targetPos, float speed, bool tall = false)
        {
            var displacement = SpacialDifference(objPos, targetPos);

            if (RangeFromVelocityAndAltitude(speed, displacement.y) < displacement.x)
            {
                Debug.LogWarning("Speed is slower than needed to reach target");
                return Vector2.zero;
            }
            
            var angleInRadians = tall ? (float)Math.Atan((Math.Pow(speed, 2) + Math.Sqrt(Math.Pow(speed, 4) - -Physics.gravity.y * 
                    ((-Physics.gravity.y * Math.Pow(displacement.x, 2)) + (2 * displacement.y * Math.Pow(speed, 2))))) / (-Physics.gravity.y * displacement.x))
                    : (float)Math.Atan((Math.Pow(speed, 2) - Math.Sqrt(Math.Pow(speed, 4) - -Physics.gravity.y * 
                    ((-Physics.gravity.y * Math.Pow(displacement.x, 2)) + (2 * displacement.y * Math.Pow(speed, 2))))) / (-Physics.gravity.y * displacement.x));
            
            var velocityForward = Mathf.Cos(angleInRadians) * speed;
            var velocityUpwards = Mathf.Sin(angleInRadians) * speed;
            
            Log($"Fish Launched With: Speed: {speed}, Dist: {displacement.x}, Alt: {displacement.y} | " +
                $"VelocityForward: {velocityForward}, VelocityUpwards: {velocityUpwards}");
            
            return new Vector2(velocityForward, velocityUpwards);
        }
        #endregion
        
        #region ---Math Formulas---
        private static float Hypotenuse(float a, float b) => (float)Math.Sqrt(a * a + b * b);
        #endregion
    }
}