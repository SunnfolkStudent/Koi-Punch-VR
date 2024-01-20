using System;
using System.Numerics;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace InDevelopment.Fish.Trajectory
{
    #region PseudoCode / Written Step-By-Step Overview

        #region Intentions and General Plans:
            // What I want to do in this script a.k.a. Desired Setup (Simplified):
                    
                // Launch an object in a parabolic curve, and calculate estimated landing position on the ground.
                // Assume startPos and landingPos are on the same y-axis level.
                            
                    // If possible later, calculate actual y.difference between startPos and landingPos.
                    // startPos will almost never match landingPos.y, due to terrain level y.differences in-game. 
                    // ...maybe use raycast along the trajectory line of some sort, which ignores everything outside the ground layer.
                         
        // -----------------------------------------------------------------------------------------------------------------
                    
            // Step A-C: Testing Environment with Goal: "PunchObject hits and launches TestObject with a velocityForce."
                    
                // A. Simplified setup with FishObject launching itself with velocityForce, using keyboard.input in Play. 
                // B. When step A is complete: simulate a punch applying Force to TestObject.
                // C. When step B is complete: substitute TestObject with an actual fish.
                        
        // -----------------------------------------------------------------------------------------------------------------
        #endregion

        #region Step-By-Step (Plan A):
        
        // Plan towards reaching Goal A:
        
        // Step A1: Create an Object launching itself with set Velocity and angle, in a parabolic curve.
            // This is to set a trajectory we can use to help do our testing and calculations.
            
                // 1. Create capsule testObject, with a Rigidbody and respective collider.
                // 2. Create empty parent gameObject TestTrajectoryManager with this script attached + following initializations:
                    // 2.1. Initialize Rigidbody _rigidbody, and get ComponentInChildren<RigidBody> in Start().
                    // 2.2. Initialize variable for InitialVelocity that will be sent to the _rigidbody through ForceMode.Velocity.
                    // 2.3. Initialize Transforms for startPos & landingPos.
                    // 2.4. Initialize float gravity (likely -9.81, but maybe -18).
                    // 2.5. Initialize bool isSleeping = true, which will make the rigidbody sleep/not move until hit when entering playMode.
                    // 2.6. Make fields Serializable, to make them editable in Inspector to test different values.
                    
                // 3. Create a method LaunchObject()
                    // 3.1. Inside this function, we apply velocity (force and [x,y,z] direction) to the testObject.
                    // 3.2. Calculate y-apex of the LaunchObject given the velocity and time.
                
                // 4. Set up keyboard.input.spaceBar.wasPressedThisFrame in Update(), to LaunchObject(), to trigger launch.
                // 5. 
                
            
        // Step A2: Make the TestObject return Vector3 InitialVelocity:
            
                // 1. Take in account the received Velocity float value from TestObject.
                    // Received Vector3 Velocity [x,y,z] - (a.k.a. force with 3D direction);
                // 2. Create vectorVertical (y-axis) and vectorHorizontal (x- & z-axis merged). (Create a 2D space).
                // 3. Normalize both vectors.
                // 4. Find a value for the angle between the vectors (which are presumably in a 2D space now).
                // 5. Find a value for timeToTarget; being the time taken from startPos to landingPos.
                // 6. Return 3 parameters; Vector3 vectorVertical, Vector3 vectorHorizontal & float angle.

        // Step A3: Create a function that'll make a calculation based on the 2 vectors and float TimeToTarget.
        
                // 1. Calculate startPos to y-Apex.
                // 2. Calculate y-apex to landingPos.
                // 3. Use the 2 calculations above to have a function with final distance and TimeToTarget created.
                
                    // ...Maybe run this stuff in a separate scene we'll call Physics Scene. 
                    // TODO: Investigate the above comment, and if creating a Physics Scene is worth it.
                 
        // Step A4: Create a LineRenderer Function that draws the estimated trajectory line.
            
                // Using a for loop, we'll create several points along the trajectory curve.
        #endregion
        
        #region Additional Notes:
            // Additional Notes:
                    
            // 1. Horizontal Velocity (velocity.x + velocity.z), by merging the x- & z-axis into new zx-axis.
            // 2. Vertical Velocity (velocity.y)
            // 3. Combine the 2 velocities above into a Vector2 with velocity.

                // Angle (2D) = ArcCos(velocityHorizontal.normalized * velocityVertical.normalized) 
                // Axis (2D) = Sqrt(Vector.Cross(velocityHorizontal.normalized, velocityVertical.normalized))
                
                // Dot Product (often called Scalar Product) = velocity.forward [x, y, z] * velocity.upwards [x, y, z].
                // If dot Product already has normalized values, you don't need to divide by magnitude.
           
                // Worth knowing that you might have to change to ArcSin, depending on whether you calculate angle
            // based on the angular bone being on the x- or y-axis.
                
            // Angle (3D) = 
                
                // If there are issues depending on either of the 4 quadrants, use this instead:
                
                    // atan2(v2.y,v2.x) - atan2(v1.y,v1.x) 
                    // Mathf.Atan2(VelocityUpwards.normalized) - Mathf.Atan2(velocityForward.normalized) = Angle.
                            
                    // TODO: Check if the above vectors in angle calculation needs to be normalized, as it might not be necessary.
                            
                // Take the dot product of the normalized Vector Values.
                // Cos Angle = Dot Product / Magnitude.

            // Why all this? 
                // It will be merged into the script FishChild (for each fish), for other scripts to fetch...
                // ...estimated Velocity Received / Landing Position / Distance Travelled,
                //  in order to play responding SFX & VFX, for e.g. a "Successful Punch" & a "Failed Punch".
                    

        #endregion

    #endregion
    public class PunchedFishTrajectoryManager : MonoBehaviour
    {
        // TODO: Figure out the received variables from punch.
        // TODO: Make a Vector2 forwardVelocity from X & Z-axis positions;
        
        // TODO: Create a punchObject applying velocityForce to the object in the air, and make object stay in the air by using Sleep.
        
        private Rigidbody _fish;
        public Vector3 startPos;
        public Vector3 landingPos;
        public Vector3 punchVelocity;
        
        public Rigidbody punchObject;
        public GameObject landingMarkPrefab;

        [Header("Adjustable Parameters for our testCapsule:")]
        [Header("Force applied with given Direction:")]
        [SerializeField] [Range(0, 150)] private float initialForce = 10f;
        [SerializeField] [Range(0, 90)] private float launchAngleFromXAxis = 60f;

        [Header("Direction for each axis given to testCapsule:")]
        [SerializeField] [Range(0, 300)] private float xDirection;
        [SerializeField] [Range(0, 300)] private float yDirection;
        [SerializeField] [Range(0, 300)] private float zDirection;
        
        [SerializeField] private float gravity = 9.81f;
        [Range(0,1)] private float _timer;
        
        [SerializeField] private bool fishAsleep = true;
        
        void Start()
        {
            _fish = GetComponentInChildren<Rigidbody>();
            startPos = _fish.position;
            print("StartPos in worldSpace:" + startPos);
            print(startPos-startPos);
            
            // Put the Rigidbody to sleep initially (no physics are being calculated)
            if (fishAsleep)
            {
                _fish.Sleep();
            }
        }

        // Step 3 - Create a method LaunchObject()
        public void LaunchObject(Vector3 fistVelocity)
        {
            if (fishAsleep)
            {
                _fish.WakeUp();
                fishAsleep = false;
            }
            
            /*// Convert launch angle to radians
            float launchAngleRad = Mathf.Deg2Rad * launchAngleFromXAxis;

            // Calculate launch direction components
            float xComponent = Mathf.Cos(launchAngleRad);
            float yComponent = Mathf.Sin(launchAngleRad);
            float zComponent = 0f; // Assuming you want to launch along the XY plane*/

            // Normalize the punch velocity to get the launch direction
            Vector3 launchDirection = fistVelocity.normalized;
            _fish.AddForce(launchDirection * initialForce, ForceMode.VelocityChange);
        }

        // TODO: Set a limit to have the below function respond to entering Ground every 1 sec.
        public void FishHitGround()
        {
            if (_timer > 1)
            {
                Instantiate(landingMarkPrefab, _fish.position, Quaternion.identity);
            }
            print("Distance Travelled:" + (_fish.position - startPos));
            _timer = 0;
        }
        
        void CalculateApex()
        {
            // Step 3.2 - Calculate y-apex
            float timeToApex = initialForce / gravity;
            float apexHeight = startPos.y + (0.5f * gravity * Mathf.Pow(timeToApex, 2));

            Debug.Log("Apex Height: " + apexHeight);
        }

        // Step 4 - Set up keyboard input to trigger launch
        private void Update()
        {
            _timer += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LaunchObject(punchVelocity);
            }
        }
        
        private void FixedUpdate()
        {
            landingPos += _fish.position;
        }
        
        // TODO: The below is for LineRenderer - to be set up once predicted trajectory works.
        
        /*public Vector3 PlotTrajectoryAtTime (Vector3 startPos, Vector3 initialVelocity, float time) 
        {
            return startPos + initialVelocity*time + Physics.gravity*time*time*0.5f;
        }
        
        public void PlotTrajectoryWithRaycast (Vector3 lastPos, Vector3 startVelocity, float timeStep, float maxTime) 
        {
            Vector3 previousPos = lastPos;
            for (int i = 1; ;i++) 
            {
                float currentTimeStep = timeStep*i;
                if (currentTimeStep > maxTime) break;
                Vector3 currentPos = PlotTrajectoryAtTime (lastPos, startVelocity, currentTimeStep);
                if (Physics.Linecast (previousPos, currentPos)) break;
                Debug.DrawLine (previousPos,currentPos,Color.red);
                previousPos = currentPos;
            }
        }

        void CalculateVelocities(float punchForce, Vector3 direction)
        {
            var gravity = 9.81f;
            var timeToTarget = (2 * punchForce * direction.normalized.y) / gravity;
            var startPosFish = transform.position;
            var landingPos = startPosFish + punchForce * direction.normalized * timeToTarget -
                             0.5f * gravity * Mathf.Pow(timeToTarget, 2) * Vector3.up;
        }*/
    }
}
