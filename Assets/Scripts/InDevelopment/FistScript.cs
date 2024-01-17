using InDevelopment.Punch;
using UnityEngine;

namespace InDevelopment
{
    public class FistScript : MonoBehaviour
    {
        [SerializeField] private float sphereCast;
        private ControllerManager _controllerManager;
        private string _whichFistUsed;
    
        private void Start()
        {
            _controllerManager = GetComponentInParent<ControllerManager>();
            _whichFistUsed = gameObject.tag;
        }

        private void Update()
        {
            if (Physics.SphereCast(transform.position, sphereCast, transform.forward, out var hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out IPunchable punch))
                {
                    punch.PunchObject(_controllerManager, _whichFistUsed);
                }
            }
        }

        // private void OnCollisionEnter(Collision other)
        // {
        //     
        //     if (other.gameObject.TryGetComponent(out IPunchable punchableObject))
        //     {
        //             punchableObject.PunchObject(controllerManager, whichFistUsed);
        //     }
        // }
    }
}
