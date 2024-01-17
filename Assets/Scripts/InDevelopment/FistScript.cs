using InDevelopment.Punch;
using UnityEngine;
using UnityEngine.Serialization;

namespace InDevelopment
{
    public class FistScript : MonoBehaviour
    {
        [SerializeField] private float sphereCastRadius = 0.05f;
        private ControllerManager _controllerManager;
        private string _whichFistUsed;
    
        private void Start()
        {
            _controllerManager = GetComponentInParent<ControllerManager>();
            _whichFistUsed = gameObject.tag;
        }

        private void Update()
        {
            if (Physics.SphereCast(transform.position, sphereCastRadius, Vector3.zero, out var hit))
            {
                if (!hit.collider.gameObject.TryGetComponent(out IPunchable punch)) return;
                punch.PunchObject(_controllerManager, _whichFistUsed);
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
