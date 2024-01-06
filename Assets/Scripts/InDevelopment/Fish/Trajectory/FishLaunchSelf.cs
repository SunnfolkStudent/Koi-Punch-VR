using UnityEngine;

namespace InDevelopment.Fish.Trajectory
{
    public class FishLaunchSelf : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float fishSpeed;
        [SerializeField] private float fishAngle;

        private Rigidbody _rigidbody;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            var fishPosition = transform.position;
            var targetPosition = playerTransform.position;
            
            FishTrajectory.LaunchObjectAtTargetWithInitialSpeed(_rigidbody, fishPosition, targetPosition, fishSpeed);
            
            //LaunchObjectAtTargetWithInitialAngle(_rigidbody, fishPosition, targetPosition, fishAngle);
        }
    }
}
