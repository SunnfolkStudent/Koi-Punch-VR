using System.Collections;
using UnityEngine;

namespace FinalScripts.Zen
{
    public class AttackFieldScript : MonoBehaviour//, IPunchable
    {
        [SerializeField] private int maxColliders = 10;
        [SerializeField] private float sphereCastRadius = 1.25f;
        [SerializeField] private float timeUntilDeath = 6f;
        private bool _dead;
        // private float _minVelocityToDestroy = 0f;
    
        void Start()
        { 
            StartCoroutine(DeathTimer());
        }
    
        private void Update()
        {
            var hitColliders = new Collider[maxColliders];
            var numColliders = Physics.OverlapSphereNonAlloc(transform.position, sphereCastRadius, hitColliders);
            for (var i = 0; i < numColliders; i++)
            {
                switch (hitColliders[i].transform.tag)
                {
                    case "LeftFist":
                        Hit();
                        //HapticManager.leftZenPunch1 = true;
                        break;
                    case "RightFist":
                        Hit();
                        //HapticManager.rightZenPunch1 = true;
                        break;
                }
            }
        }

        private void Hit()
        {
            Debug.Log("---WeakPointHit---");
            ZenMetreManager.Instance.AddAttackFieldZen();
            Destroy(gameObject);
        }
    
        private IEnumerator DeathTimer()
        {
            yield return new WaitForSecondsRealtime(timeUntilDeath);
            _dead = true;
            Destroy(gameObject);
        }

        // private void OnDestroy()
        // {
        //     if (!_dead)
        //     {
        //         ZenMetreManager.Instance.AddAttackFieldZen();
        //         
        //     }
        // }
    
        // public void PunchObject(ControllerManager controllerManager, String fistUsed)
        // {
        //     Debug.LogError($"Punched L: {controllerManager.leftControllerVelocity.magnitude} R: {controllerManager.rightControllerVelocity.magnitude}", this);
        //     if (fistUsed == "LeftFist")
        //     {
        //         if (controllerManager.leftControllerVelocity.magnitude > _minVelocityToDestroy)
        //         {
        //             Destroy(gameObject);
        //             HapticManager.leftZenPunch1 = true;
        //         }
        //     }
        //     else if (fistUsed == "RightFist")
        //     {
        //         if (controllerManager.rightControllerVelocity.magnitude > _minVelocityToDestroy)
        //         {
        //             Destroy(gameObject);
        //             HapticManager.rightZenPunch1 = true;
        //         }
        //     }
        // }
    }
}
