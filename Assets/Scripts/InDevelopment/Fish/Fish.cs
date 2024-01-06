using UnityEngine;

namespace InDevelopment.Fish
{
    public class Fish : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            FishSpawnManager.DespawnFish(gameObject);
        }

        void Update()
        {
            if (transform.position.y < -10)
            {
                Debug.Log("Destroyded by being to far down");
                FishSpawnManager.DespawnFish(gameObject);
            }
        }
    }
}
