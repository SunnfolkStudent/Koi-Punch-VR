using System.Collections;
using System.Linq;
using UnityEngine;

namespace FinalScripts.Fish.BossBattle
{
    public class WeakPoint : MonoBehaviour, IPunchable
    {
        [Header("Despawn Time")]
        [SerializeField] private float despawnTime = 2f;
        
        [Header("Rumble")]
        [SerializeField] private float onHitRumbleDuration = 1f;
        
        [Header("Weak Point")]
        [SerializeField] private int weakPointScore = 60;
        
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
            Boss.Score += weakPointScore;
            EventManager.ScoreChanged.Invoke(Boss.Score + Boss.Phase.Sum(pair => pair.Value.score));
            ZenMetreManager.Instance.AddAttackFieldZen();
            Destroy(gameObject);
        }
        
        #region >>>---Rumble---
        private IEnumerator LeftRumble()
        {
            HapticManager.leftZenPunch1 = true;
            yield return new WaitForSecondsRealtime(onHitRumbleDuration);
            HapticManager.leftZenPunch1 = false;
        }
        
        private IEnumerator RightRumble()
        {
            HapticManager.rightZenPunch1 = true;
            yield return new WaitForSecondsRealtime(onHitRumbleDuration);
            HapticManager.rightZenPunch1 = false;
        }
        #endregion
        #endregion
    }
}
