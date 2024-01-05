using System;
using Unity.Mathematics;
using UnityEngine;

namespace InDevelopment.Trajectory
{
    public class FishTrajectory : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float fishSpeed;

        private Rigidbody _rigidbody;
        private const float Gravity = 9.8f;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            var fishPosition = transform.position;
            var targetPosition = playerTransform.position;
            
            var distance = targetPosition.x - fishPosition.x;
            var altitudeDifference = fishPosition.y - targetPosition.y;

            RotateTowardsTarget(targetPosition);
            _rigidbody.velocity = TrajectoryAngleFromSpeedDistanceAltitude(fishSpeed, distance, altitudeDifference);
        }

        private void RotateTowardsTarget(Vector3 targetPosition)
        {
            var direction = targetPosition - transform.position;
            var angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        private Vector3 TrajectoryAngleFromSpeedDistanceAltitude(float speed, float dist, float alt)
        {
            var angleInRadians = (float)Math.Atan((NumberExponent(speed, 2) + Math.Sqrt(NumberExponent(speed, 4) - Gravity * 
                                ((Gravity * NumberExponent(dist, 2)) + (2 * alt * NumberExponent(speed, 2))))) / (Gravity * dist));
            
            return (new Vector3(0, Mathf.Sin(angleInRadians), 0)
                 + transform.forward * Mathf.Cos(angleInRadians)) * speed;
        }

        private static float NumberExponent(float number, int exponent)
        {
            var num = number;
            for (var i = 1; i < exponent; i++) number *= num;
            return number;
        }
    }
}