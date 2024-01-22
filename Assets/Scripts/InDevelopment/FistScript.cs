using FinalScripts;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;

namespace InDevelopment
{
    public class FistScript : MonoBehaviour
    {
        [Header("SphereCast")][Tooltip("SphereCast is for collision detection during time-stop")]
        [SerializeField] private int maxColliders = 10;
        [SerializeField] private float sphereCastRadius = 0.2f;
        
        private bool _collidedPreviously;
        private ControllerManager _controllerManager;
        private string _whichFistUsed;
        
        private void Start()
        {
            _controllerManager = GetComponentInParent<ControllerManager>();
            _whichFistUsed = gameObject.tag;
            
            InternalZenEventManager.playChargeSfx += PlayChargeSFX;
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
            
            if (numColliders == 0) _collidedPreviously = false;
            if (_collidedPreviously) return;
            
            for (var i = 0; i < numColliders; i++)
            {
                if (!hitColliders[i].TryGetComponent(out IPunchable punchableObject)) continue;
                punchableObject.PunchObject(_controllerManager, _whichFistUsed);
                _collidedPreviously = true;
            }
        }

        #region SFX

        private void PlayChargeSFX()
        {
            RuntimeManager.AttachInstanceToGameObject(FMODManager.instance.koiPunch, gameObject.transform, this.GetComponent<Rigidbody>());
            FMODManager.instance.koiPunch.setParameterByName("koiPunchSoundState", 0);
            FMODManager.instance.koiPunch.setParameterByName("koiPunchImpactState", 0);
            //do this for each hand
            //when charge ready:
            FMODManager.instance.koiPunch.setParameterByName("koiPunchSoundState", 1);
            FMODManager.instance.koiPunch.setParameterByName("koiPunchImpactState", 1);
        }
        
        #endregion
    }
}