using System;
using UnityEngine;

namespace InDevelopment.Fish.Trajectory
{
    public static class FishTrajectory
    {
        // TODO: When using speed for trajectory stop too low speeds from creating an error
        // TODO: When using angle for trajectory fix why 3% chance of error
        
        #region ---Debugging---
        private static bool _isDebugging;
        private static void Log(string message) => Debug.Log(message);
        #endregion
        
        #region ---LaunchRequirements---
        private static Vector2 SpacialDifference(Vector3 objPos, Vector3 targetPos)
        {
            var objectTargetDifference = new Vector2(targetPos.x - objPos.x, targetPos.z - objPos.z);
            var directDistance = Hypotenuse(objectTargetDifference.x, objectTargetDifference.y);
            var altitudeDifference = objPos.y - targetPos.y;
            return new Vector2(directDistance, -altitudeDifference);
        }
        #endregion
        
        #region ---TrajectoryCalculations---
        public static Vector2 TrajectoryVelocityFromInitialSpeed(Vector3 objPos, Vector3 targetPos, float speed, bool tall)
        {
            var displacement = SpacialDifference(objPos, targetPos);
            var angleInRadians = tall ? (float)Math.Atan((Math.Pow(speed, 2) + Math.Sqrt(Math.Pow(speed, 4) - -Physics.gravity.y * 
                    ((-Physics.gravity.y * Math.Pow(displacement.x, 2)) + (2 * displacement.y * Math.Pow(speed, 2))))) / (-Physics.gravity.y * displacement.x))
                    : (float)Math.Atan((Math.Pow(speed, 2) - Math.Sqrt(Math.Pow(speed, 4) - -Physics.gravity.y * 
                    ((-Physics.gravity.y * Math.Pow(displacement.x, 2)) + (2 * displacement.y * Math.Pow(speed, 2))))) / (-Physics.gravity.y * displacement.x));
            
            var velocityForward = Mathf.Cos(angleInRadians) * speed;
            var velocityUpwards = Mathf.Sin(angleInRadians) * speed;
            
            if (_isDebugging) Log($"Speed: {speed}, Dist: {displacement.x}, Alt: {displacement.y} | " +
                                  $"VelocityForward: {velocityForward}, VelocityUpwards: {velocityUpwards}");
            
            return new Vector2(velocityForward, velocityUpwards);
        }
        
        public static Vector2 TrajectoryVelocityFromInitialAngle(Vector3 objPos, Vector3 targetPos, float angle)
        {
            var displacement = SpacialDifference(objPos, targetPos);
            var vTotal = Math.Sqrt((Math.Pow(displacement.x, 2) * -Physics.gravity.y) / 
                                           (displacement.x * Math.Abs(Mathf.Sin(2 * angle)) - 2 * 
                                               displacement.y * Math.Pow(Math.Abs(Mathf.Cos(angle)), 2)));
            
            var vForward = (float)Math.Abs(vTotal * Mathf.Cos(angle));
            var vUpwards = (float)Math.Abs(vTotal * Mathf.Sin(angle));
            
            if (_isDebugging) Log($"Angle: {angle}, Dist: {displacement.x}, Alt: {displacement.y} | " +
                                  $"VelocityForward: {vForward}, VelocityUpwards: {vUpwards}");
            
            return new Vector2(vForward, vUpwards);
        }
        
        public static Vector2 TrajectoryVelocityFromPeakHeight(Vector3 objPos, Vector3 targetPos,float height)
        {
            var displacement = SpacialDifference(objPos, targetPos);
            if (height < displacement.y - 1)
            {
                Debug.LogWarning("Peak trajectory height set lower than target altitude");
                height = displacement.y + 1;
            }
            var velocityForward = Mathf.Sqrt(-2 * Physics.gravity.y * height);
            var velocityUpwards = displacement.x / (Mathf.Sqrt(-((2 * height) / Physics.gravity.y)) + Mathf.Sqrt((2 * (displacement.y - height)) / Physics.gravity.y));

            if (_isDebugging) Log($"Height: {height}, Dist: {displacement.x}, Alt: {displacement.y} | " +
                                  $"VelocityForward: {velocityForward}, VelocityUpwards: {velocityUpwards}");
            
            return new Vector2(velocityUpwards, velocityForward);
        }
        
        public static float RangeFromSpeedAngleAltitude(float speed, float angle, float alt)
        {
            return speed / -Physics.gravity.y * Mathf.Sqrt(Mathf.Pow(speed, 2) + 2 * -Physics.gravity.y * alt);
        }
        #endregion
        
        #region ---Math Formulas---
        private static float Hypotenuse(float a, float b) => (float)Math.Sqrt(a * a + b * b);
        #endregion
    }
}