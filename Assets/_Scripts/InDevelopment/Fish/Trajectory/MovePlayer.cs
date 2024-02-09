using System;
using FinalScripts.Fish;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InDevelopment.Fish.Trajectory
{
    public class MovePlayer : MonoBehaviour
    {
        // TODO: Remove this script, it is only for testing, but wait until boss spawning from spawn area in FOV is done!
        [SerializeField] private float speed = 0.5f;

        private Rigidbody _rigidbody;
        private Camera _viewCamera;
        private Vector3 _velocity;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _viewCamera = Camera.main;
        }
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
            Vector3 mousePos = _viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                _viewCamera.transform.position.y));
            transform.LookAt(mousePos + Vector3.up * transform.position.y);
            _velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * speed;
            
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                EventManager.SpawnFish.Invoke();
            }
        }
    }
}