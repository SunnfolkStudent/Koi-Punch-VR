using UnityEngine;
using UnityEngine.InputSystem;

namespace InDevelopment.Fish.Trajectory
{
    public class MovePlayer : MonoBehaviour
    {
        // TODO: Remove this script, it is only for testing
        [SerializeField] private float speed = 0.5f;
        
        private void FixedUpdate()
        {
            transform.position += new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")) * speed;
        }
        
        private void Update()
        {
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                FishSpawnManager.SpawnFish.Invoke();
            }
        }
    }
}