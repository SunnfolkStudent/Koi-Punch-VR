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
            Invoke(nameof(Despawn), 5);
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
                    Despawn();
                }
            }
        }
        
        private void Update()
        {
            if (transform.position.y < -25)
            {
                Despawn();
            }
        }
        
        
        private void Despawn()
        {
            FishObjectPool.DespawnFish(fish);
        }
    }
}