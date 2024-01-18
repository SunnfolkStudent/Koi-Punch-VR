using System.Collections;
using InDevelopment.Punch;
using UnityEngine;

namespace FinalScripts.Zen
{
    public class AttackFieldScript : MonoBehaviour, IPunchable
    {
        [SerializeField] private float despawnTime = 6f;
        [SerializeField] private float rumbleDuration = 1f;
        
        private void OnEnable()
        { 
            StartCoroutine(DeathTimer());
        }

        private void Hit()
        {
            Debug.Log("---WeakPointHit---");
            ZenMetreManager.Instance.AddAttackFieldZen();
            Destroy(gameObject);
        }
    
        private IEnumerator DeathTimer()
        {
            yield return new WaitForSecondsRealtime(despawnTime);
            Destroy(gameObject);
        }
    
        public void PunchObject(ControllerManager controllerManager, string fistUsed)
        {
            switch (fistUsed)
            {
                case "LeftFist":
                    StartCoroutine(LeftRumble());
                    Hit();
                    break;
                case "RightFist":
                    StartCoroutine(RightRumble());
                    Hit();
                    break;
            }
        }

        #region ---Rumble---
        private IEnumerator LeftRumble()
        {
            HapticManager.leftZenPunch1 = true;
            yield return new WaitForSecondsRealtime(rumbleDuration);
            HapticManager.leftZenPunch1 = false;
        }
        
        private IEnumerator RightRumble()
        {
            HapticManager.rightZenPunch1 = true;
            yield return new WaitForSecondsRealtime(rumbleDuration);
            HapticManager.rightZenPunch1 = false;
        }
        #endregion
    }
}
