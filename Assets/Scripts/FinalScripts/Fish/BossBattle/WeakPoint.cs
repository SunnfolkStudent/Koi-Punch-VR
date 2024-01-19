using System.Collections;
using InDevelopment.Punch;
using UnityEngine;

namespace FinalScripts.Fish.BossBattle
{
    public class WeakPoint : MonoBehaviour, IPunchable
    {
        [Header("Despawn Time")][Tooltip("Time before weak-point de-spawns")]
        [SerializeField] private float despawnTime = 6f;
        [Header("Rumble")][Tooltip("Rumble controller on hits")]
        [SerializeField] private float rumbleDuration = 1f;
        
        private void OnEnable()
        { 
            Destroy(gameObject, despawnTime);
        }
        
        #region ---IPunchable---
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

        private void Hit()
        {
            Debug.Log("---WeakPointHit---");
            ZenMetreManager.Instance.AddAttackFieldZen();
            Destroy(gameObject);
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
        #endregion
    }
}
