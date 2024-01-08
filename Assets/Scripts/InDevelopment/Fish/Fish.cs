using UnityEngine;

namespace InDevelopment.Fish
{
    public class Fish : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                FishSpawnManager.DespawnFish(gameObject);
            }
        }

        private void Update()
        {
            if (transform.position.y < -25)
            {
                // Debug.Log("Destroyed by being too far down");
                FishSpawnManager.DespawnFish(gameObject);
            }
        }
    }
}
