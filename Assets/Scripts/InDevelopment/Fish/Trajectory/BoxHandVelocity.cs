using System;
using UnityEngine;

namespace InDevelopment.Fish.Trajectory
{
    public class BoxHandVelocity : MonoBehaviour
    {
        [Header("PunchVelocity:")]
        public Vector3 punchVelocity = new (1, 1, 1);
        private Vector3 _punchDirection;
        private float _punchSpeed;
        private void Start()
        {
            // Set initial velocity to move the punch in the specified direction
            _punchSpeed = punchVelocity.magnitude;
            _punchDirection = punchVelocity.normalized;
            Vector3 worldDirection = transform.TransformDirection(_punchDirection);
            GetComponent<Rigidbody>().velocity = worldDirection * _punchSpeed;
        }

        private void Update()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            _punchDirection = punchVelocity.normalized;
            _punchSpeed = punchVelocity.magnitude;
        }
        private void OnCollisionEnter(Collision collision)
        {
            // Check if the collided object has a Rigidbody
            Rigidbody otherRigidbody = collision.collider.GetComponent<Rigidbody>();

            if (otherRigidbody != null)
            {
                // Print the velocity of the punch on collision
                Debug.Log("Collision Velocity: " + collision.relativeVelocity);
            }
        }
        
    }
}
