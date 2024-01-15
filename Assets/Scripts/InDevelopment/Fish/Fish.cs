using UnityEngine;

namespace InDevelopment.Fish
{
    public class Fish : MonoBehaviour
    {
        // TODO: Fish collision
        public FishObjectPool.Fish fish { get; set; }
        private float _startTime;
        [SerializeField] private float despawnTime = 5f;
        [SerializeField] private float despawnAltitude = -5f;
        
        private void OnCollisionEnter(Collision other)
        {
            Vector3 initialPunchPosition = new Vector3(0, 0, 0);
            
            if (other.gameObject.CompareTag("LeftFist") || other.gameObject.CompareTag("RightFist"))
            {
                Debug.Log(initialPunchPosition);
            }
            
            if (other.gameObject.CompareTag("Ground"))
            {
                Debug.Log(transform.position-initialPunchPosition);
                Despawn();
            }
        }
        
        private void OnEnable()
        {
            _startTime = Time.time;
        }
        
        private void Update()
        {
            if (transform.position.y < despawnAltitude || _startTime < Time.time - despawnTime)
            {
                Despawn();
            }
        }
        
        // TODO: Punch script needs to make the fish call this function
        private void AddZen()
        {
            ZenMetreManager.Instance.AddHitZen(fish.FishPool.Prefab.ZenAmount);
        }
        
        private void Despawn()
        {
            FishObjectPool.DespawnFish(fish);
        }
    }
}