using UnityEngine;

namespace InDevelopment.Fish.Trajectory
{
    public class BoxHandVelocity : MonoBehaviour
    {
        [Header("Speed.")]
        [SerializeField] private float punchSpeed = 20f;
        
        [Header("Direction.normalized, just give the full velocity in each axis.")]
        [SerializeField] private Vector3 punchDirection = new Vector3(1, 1, 1).normalized;
        private void Start()
        {
            // Set initial velocity to move the punch in the specified direction
            Vector3 worldDirection = transform.TransformDirection(punchDirection);
            GetComponent<Rigidbody>().velocity = worldDirection * punchSpeed;
        }
        private void OnCollisionEnter(Collision collision)
        {
            // Check if the collided object has a Rigidbody
            Rigidbody otherRigidbody = collision.collider.GetComponent<Rigidbody>();

            if (otherRigidbody != null)
            {
                // Print the velocity of the punch on collision
                Debug.Log("Collision Impulse: " + collision.impulse.magnitude);
            }
        }
        
    }
}
