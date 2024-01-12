using UnityEngine;

namespace InDevelopment.Fish
{
    public class Fish : MonoBehaviour
    {
        // TODO: Fish collision
        public FishObjectPool.Fish fish { get; set; }
        private float _startTime;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                Despawn();
            }
        }
        private void OnEnable()
        {
            _startTime = Time.time;
        }
        
        private void Update()
        {
            if (transform.position.y < -25 || _startTime < Time.time - 3.5f)
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