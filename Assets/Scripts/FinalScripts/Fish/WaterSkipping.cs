using System;
using UnityEngine;

namespace FinalScripts.Fish
{
    public class WaterSkipping : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        
        [SerializeField] private float attackAngleMaximum = 15f;
        [SerializeField] private float ySpeedAmount = 10f;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                CheckIfCanSkipp();
            }
        }
        
        private void CheckIfCanSkipp()
        {
            var velocity = _rigidbody.velocity;
            var nor = velocity.normalized;
            var attackAngle = AngleBetweenVectors(new Vector3(nor.x,0, nor.z), new Vector3(0, nor.y, 0));
            var ySpeed = velocity.y;
            var fSpeed = Mathf.Abs(velocity.magnitude - ySpeed);
            
            if (attackAngle < attackAngleMaximum && fSpeed > ySpeedAmount)
            {
                Skip();
            }
        }
        
        private void Skip()
        {
            _rigidbody.AddForce(new Vector3(0, ySpeedAmount, 0));
        }
        
        private static double AngleBetweenVectors(Vector3 v1, Vector3 v2)
        {
            double dotProduct = Vector3.Dot(v1, v2);
            double normV1 = v1.magnitude;
            double normV2 = v2.magnitude;

            var angle = Math.Acos(dotProduct / (normV1 * normV2));
            return ToDegrees(angle);
        }
        
        private static double ToDegrees(double radians)
        {
            return radians * (180.0 / Math.PI);
        }
    }
}
