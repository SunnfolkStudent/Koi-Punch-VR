using UnityEngine;

namespace FinalScripts.Fish
{
    public class SetWeakPointsActive : MonoBehaviour
    {
        private Transform[] _weakPoints;
        private void Start()
        {
            _weakPoints = GetComponentsInChildren<Transform>();
            EventManager.StartBossPhase1 += SetAllActive;

            foreach (var transform1 in _weakPoints)
            {
                transform1.gameObject.SetActive(false);
            }
        }
    
        private void SetAllActive()
        {
            foreach (var transform1 in _weakPoints)
            {
                transform1.gameObject.SetActive(true);
            }
        }
    }
}