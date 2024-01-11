using UnityEngine;

namespace InDevelopment.Fish
{
    public class Fish : MonoBehaviour
    {
        // TODO: Fish collision
        public FishObjectPool.Fish fish { get; set; }
        private bool _canCollide;
        
        private void OnEnable()
        {
            _canCollide = false;
            Invoke(nameof(CanCollide), 1);
        }
        
        private void CanCollide()
        {
            _canCollide = true;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (_canCollide)
            {
                if (other.gameObject.CompareTag("Ground"))
                {
                    FishObjectPool.DespawnFish(gameObject);
                }
            }
        }

        private void Update()
        {
            if (transform.position.y < -25)
            {
                FishObjectPool.DespawnFish(gameObject);
            }
        }
    }
}