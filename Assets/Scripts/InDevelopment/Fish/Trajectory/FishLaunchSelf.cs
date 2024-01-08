using Unity.Mathematics;
using UnityEngine;

namespace InDevelopment.Fish.Trajectory
{
    public class FishLaunchSelf : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float fishSpeed;
        [SerializeField] private Rigidbody[] _rigidbody;
        
        private void Start()
        {
            var fishPosition = transform.position;
            var targetPosition = playerTransform.position;

            RotateObjTowards(transform, targetPosition);
            
            FishTrajectory.LaunchObjectAtTargetWithInitialSpeed(_rigidbody, fishPosition, targetPosition, fishSpeed);
        }

        private static void RotateObjTowards(Transform objTransform, Vector3 target)
        {
            var targetDir = objTransform.position - target;
            var angle = Vector3.Angle(targetDir, objTransform.forward);
            objTransform.rotation = new quaternion(0, angle, 0, (float)Space.World);
        }
    }
}