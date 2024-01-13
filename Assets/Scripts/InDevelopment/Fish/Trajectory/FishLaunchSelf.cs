using Unity.Mathematics;
using UnityEngine;

namespace InDevelopment.Fish.Trajectory
{
    public class FishLaunchSelf : MonoBehaviour
    {
        // TODO: Remove this script, it is only for testing
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float fishSpeed;
        [SerializeField] private Rigidbody[] _rigidbody;
        
        private void Start()
        {
            var fishTransform = transform;
            var targetPosition = playerTransform.position;

            RotateObjTowards(fishTransform, targetPosition);
            
            FishTrajectory.LaunchObjectAtTargetWithInitialSpeed(_rigidbody, fishTransform.position, targetPosition, fishSpeed);
        }

        private static void RotateObjTowards(Transform objTransform, Vector3 target)
        {
            var targetDir = objTransform.position - target;
            var angle = Vector3.Angle(targetDir, objTransform.forward);
            objTransform.rotation = new Quaternion(0, angle, 0, (float)Space.World);
        }
    }
}