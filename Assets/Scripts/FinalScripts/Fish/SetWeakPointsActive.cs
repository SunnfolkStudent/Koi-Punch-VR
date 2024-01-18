using UnityEngine;

namespace FinalScripts.Fish
{
    public class SetWeakPointsActive : MonoBehaviour
    {
        private void Start()
        {
            EventManager.StartBossPhase1 += SetAllActive;
            gameObject.SetActive(false);
        }
    
        private void SetAllActive()
        {
            gameObject.SetActive(true);
        }
    }
}