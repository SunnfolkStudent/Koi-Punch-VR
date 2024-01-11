using System.Linq;
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
                
                // if (other.gameObject.CompareTag("LeftFist") || other.gameObject.CompareTag("RightFist"))
                // {
                //     Debug.Log("Hit Player Fist", this);
                //     var rigidities = fish.Children.Where(child => child.Rigidbody != null).Select(child => child.Rigidbody).ToArray();
                //     foreach (var rigidbody1 in rigidities)
                //     {
                //         rigidbody1.velocity = Vector3.zero;
                //     }
                // }
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