using UnityEngine;

namespace InDevelopment.Fish.Trajectory
{
    public class MovePlayer : MonoBehaviour
    {
        [SerializeField] private float speed = 0.5f;
        
        private void FixedUpdate()
        {
            transform.position += new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")) * speed;
        }
    }
}