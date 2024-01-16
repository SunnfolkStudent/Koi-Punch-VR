using UnityEngine;

namespace FinalScripts.Fish
{
    public class FishChild : MonoBehaviour, IPunchable
    {
        public Fish fish;
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Ground"))
            {
                fish.FishHitGround();
            }
        }
        
        public void PunchObject(ControllerManager controllerManager, string fistUsed)
        {
            var v = fistUsed == "LeftFist"
                ? controllerManager.leftControllerVelocity
                : controllerManager.rightControllerVelocity;
            
            if (v.magnitude >= fish.velocityNeededForSuccessfulHit)
            {
                PunchObject(v);
            }
            else
            {
                fish.Log("PunchVelocity too slow");
            }
        }

        private void PunchObject(Vector3 velocity)
        {
            if (fish.hasBeenPunched || fish.hasHitGround)
            {
                fish.Log("Already punched or hit the ground");
                return;
            }
            fish.FishPunched();
            
            var direction = velocity.normalized;
            var punchForce = velocity.magnitude * fish.punchVelMultiplier;
            var vectorForce = direction * punchForce;

            _rigidbody.AddForce(vectorForce, ForceMode.VelocityChange);
            fish.Log("VectorForce: " + vectorForce);
        }
    }
}