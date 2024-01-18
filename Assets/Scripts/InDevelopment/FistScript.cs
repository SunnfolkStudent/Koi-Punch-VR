using InDevelopment.Punch;
using UnityEngine;

namespace InDevelopment
{
    public class FistScript : MonoBehaviour
    {
        private ControllerManager _controllerManager;
        private string _whichFistUsed;
        
        private void Start()
        {
            _controllerManager = GetComponentInParent<ControllerManager>();
            _whichFistUsed = gameObject.tag;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            
            if (other.gameObject.TryGetComponent(out IPunchable punchableObject))
            {
                    punchableObject.PunchObject(_controllerManager, _whichFistUsed);
            }
        }
    }
}
