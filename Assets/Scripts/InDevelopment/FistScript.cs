using System;
using System.Collections;
using FinalScripts;
using FMODUnity;
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

        private void Awake()
        {
            RuntimeManager.AttachInstanceToGameObject(FMODManager.instance.koiPunch, gameObject.transform, this.GetComponent<Rigidbody>());
        }

        private void Start()
        {
            _controllerManager = GetComponentInParent<ControllerManager>();
            _whichFistUsed = gameObject.tag;
            
            InternalZenEventManager.playChargeSfx += PlayChargeSfx;
            InternalZenEventManager.playChargeReadySfx += PlayChargeReadySfx;
            InternalZenEventManager.playChargePunchSfx += PlayChargePunchSfxCoroutine;
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

        private void PlayChargeSfx()
        {
            FMODManager.instance.koiPunch.setParameterByName("koiPunchSoundState", 0);
            FMODManager.instance.koiPunch.setParameterByName("koiPunchImpactState", 0);
            FMODManager.instance.koiPunch.start();
        }
        
        private void PlayChargeReadySfx()
        {
            FMODManager.instance.koiPunch.setParameterByName("koiPunchSoundState", 1);
            FMODManager.instance.koiPunch.setParameterByName("koiPunchImpactState", 1);
        }
        
        private void PlayChargePunchSfxCoroutine()
        {
            StartCoroutine(PlayChargePunchSfx());
        }

        private IEnumerator PlayChargePunchSfx()
        {
            //do this in each hand :3
            FMODManager.instance.koiPunch.setParameterByName("koiPunchSoundState", 2);
            yield return new WaitForSecondsRealtime(4f);
            FMODManager.instance.koiPunch.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //this is for the vocals:
            FMODManager.instance.koiPunchVocals.setParameterByName("koiPunchSoundState", 0);
            yield return new WaitForSecondsRealtime(4f);
            FMODManager.instance.koiPunchVocals.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        #endregion
    }
}