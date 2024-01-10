using UnityEngine;

namespace InDevelopment.Fish
{
    public class Fish : MonoBehaviour
    {
        // TODO: Fish collision
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                FishObjectPool.DespawnFish(gameObject);
            }
        }

        private void Update()
        {
            if (transform.position.y < -25)
            {
                // Debug.Log("Destroyed by being too far down");
                FishObjectPool.DespawnFish(gameObject);
            }
        }
    }
}