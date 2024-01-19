using UnityEngine;
using UnityEngine.Serialization;

namespace InDevelopment.Fish.Trajectory
{
    public class PunchedFishScript : MonoBehaviour
    {
        // Change instance name from PunchedFishTrajectoryManager to be fish later, as you'll be receiving values from Fish.
        public PunchedFishTrajectoryManager punchedFishTrajectoryManager;
        public FishTrajectorySimulator fishTrajectorySimulator;
        public Rigidbody fishRigidbody;

        private bool _punched;
        
        private void OnCollisionEnter(Collision other)
        {
            Vector3 punchVelocity;
            switch (other.transform.tag)
            {
                case "Ground":
                    punchedFishTrajectoryManager.FishHitGround();
                    break;
                case "LeftFist":
                    if (_punched) { break; }
                    HapticManager.leftFishPunch = true;
                    // Get the velocity of the punch
                    punchVelocity = other.relativeVelocity;
                    punchedFishTrajectoryManager.LaunchObject(punchVelocity);
                    fishTrajectorySimulator.SimulateTrajectory();
                    print("Punch Velocity Given:" + punchVelocity);
                    _punched = true;
                    break;
                case "RightFist":
                    if (_punched) { break; }
                    HapticManager.rightFishPunch = true;
                    // Get the velocity of the punch
                    punchVelocity = other.relativeVelocity;
                    punchedFishTrajectoryManager.LaunchObject(punchVelocity);
                    fishTrajectorySimulator.SimulateTrajectory();
                    print("Punch Velocity Given:" + punchVelocity);
                    _punched = true;
                    break;
            }
        }
    }
}
