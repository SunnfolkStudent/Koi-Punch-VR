using FinalScripts.Fish;
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
            transform.position += new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal")) * speed;
            
            if (Keyboard.current.hKey.isPressed)
            {
                EventManager.SpawnFish.Invoke();
            }
            
            if (Keyboard.current.jKey.isPressed)
            {
                EventManager.SpawnFish.Invoke();
            }
            
            if (Keyboard.current.kKey.isPressed)
            {
                EventManager.SpawnFish.Invoke();
            }
        }

        private void Update()
        {
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                EventManager.SpawnFish.Invoke();
            }
        }
    }
}