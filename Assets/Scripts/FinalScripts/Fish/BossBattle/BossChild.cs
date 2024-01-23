using UnityEngine;

namespace FinalScripts.Fish.BossBattle
{
    public class BossChild : MonoBehaviour, IPunchable
    {
        public Boss boss;
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        #region ---Collision---
        private void OnCollisionEnter(Collision other)
        {
            switch (other.transform.tag)
            {
                case "MainCamera":
                    boss.HitPlayer();
                    break;
                case "Ground":
                    boss.HitGround();
                    break;
                case "Bird":
                    boss.HitBird();
                    break;
                case "LeftFist":
                    HapticManager.leftFishPunch = true;
                    break;
                case "RightFist":
                    HapticManager.rightFishPunch = true;
                    break;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Water"))
            {
                boss.HitWater(_rigidbody.velocity);
            }
        }
        
        private void OnCollisionExit(Collision other)
        {
            switch (other.transform.tag)
            {
                case "LeftFist":
                    HapticManager.leftFishPunch = false;
                    break;
                case "RightFist":
                    HapticManager.rightFishPunch = false;
                    break;
            }
        }
        #endregion
        
        #region ---IPunchable---
        public void PunchObject(ControllerManager controllerManager, string fistUsed)
        {
            boss.Punched(controllerManager, fistUsed);
        }
        #endregion
    }
}