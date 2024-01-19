using InDevelopment.Punch;
using UnityEngine;

namespace InDevelopment
{
    public class FistScript : MonoBehaviour
    {
        [Header("SphereCast")][Tooltip("SphereCast is for collision detection during time-stop")]
        [SerializeField] private int maxColliders = 10;
        [SerializeField] private float sphereCastRadius = 1.25f;
        
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
        
        private void Update()
        {
            if (Time.timeScale < 1f)
            {
                SphereCastCollision();
            }
        }
        
        private void SphereCastCollision()
        {
            var hitColliders = new Collider[maxColliders];
            var numColliders = Physics.OverlapSphereNonAlloc(transform.position, sphereCastRadius, hitColliders);
            for (var i = 0; i < numColliders; i++)
            {
                if (hitColliders[i].TryGetComponent(out IPunchable punchableObject))
                {
                    punchableObject.PunchObject(_controllerManager, _whichFistUsed);
                }
            }
        }
    }
}