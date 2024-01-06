using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace InDevelopment.Trajectory
{
    public class FishTrajectory : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float fishSpeed;

        private Rigidbody _rigidbody;
        private const float Gravity = 9.81f;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            var fishPosition = transform.position;
            var targetPosition = playerTransform.position;
            
            var i = targetPosition- fishPosition;
            var distance = math.sqrt(i.x * i.x + i.z * i.z);

            var altitudeDifference = fishPosition.y - targetPosition.y;

            // RotateTowardsTarget(targetPosition);
            TrajectoryAngleFromSpeedDistanceAltitude(fishSpeed, distance, altitudeDifference);
        }

        private void RotateTowardsTarget(Vector3 targetPosition)
        {
            var direction = targetPosition - transform.position;
            var angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        private void TrajectoryAngleFromSpeedDistanceAltitude(float speed, float dist, float alt)
        {
            var angleInRadians = (float)Math.Atan((NumberExponent(speed, 2) + Math.Sqrt(NumberExponent(speed, 4) - Gravity * 
                                ((Gravity * NumberExponent(dist, 2)) + (2 * alt * NumberExponent(speed, 2))))) / (Gravity * dist));
            
            var vx = Mathf.Cos(angleInRadians) * speed;
            var vy = Mathf.Sin(angleInRadians) * speed;
            
            example(vx, vy);
        }

        private static float NumberExponent(float number, int exponent)
        {
            var num = number;
            for (var i = 1; i < exponent; i++) number *= num;
            return number;
        }


        private void example(float sideSpeed, float upSpeed)
        {
            Vector3 playerDirection = (playerTransform.position - transform.position).normalized;

            playerDirection *= sideSpeed;

            _rigidbody.velocity = new Vector3(playerDirection.x, upSpeed, playerDirection.z);
        }
    }
}