using InDevelopment.Punch;
using UnityEngine;

namespace InDevelopment
{
    public class FistScript : MonoBehaviour
    {
        // [SerializeField] private float sphereCastRadius = 0.05f;
        // [SerializeField] private int maxColliders = 10;
        private ControllerManager _controllerManager;
        private string _whichFistUsed;
    
        private void Start()
        {
            _controllerManager = GetComponentInParent<ControllerManager>();
            _whichFistUsed = gameObject.tag;
        }

        // private void Update()
        // {
        //     var hitColliders = new Collider[maxColliders];
        //     var numColliders = Physics.OverlapSphereNonAlloc(transform.position, sphereCastRadius, hitColliders);
        //     for (var i = 0; i < numColliders; i++)
        //     {
        //         if (!hitColliders[i].gameObject.TryGetComponent(out IPunchable punch)) return;
        //         punch.PunchObject(_controllerManager, _whichFistUsed);
        //     }
        //     
        //     // if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out var hit, 0f))
        //     // {
        //     //     if (!hit.collider.gameObject.TryGetComponent(out IPunchable punch)) return;
        //     //     punch.PunchObject(_controllerManager, _whichFistUsed);
        //     // }
        // }

        private void OnCollisionEnter(Collision other)
        {
            
            if (other.gameObject.TryGetComponent(out IPunchable punchableObject))
            {
                    punchableObject.PunchObject(_controllerManager, _whichFistUsed);
            }
        }
    }
}
