using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class FishObjectPool : MonoBehaviour
    {
        private static List<Fish> _fishPool;
        [SerializeField] private GameObject fishObjectToPool;
        [SerializeField] private int initialFishAmountInPool = 10;
        [SerializeField] private GameObject fishContainer;
        
        #region ---FishPoolInfo---
        public class Fish
        {
            public readonly GameObject ParentGameObject;
            public readonly Child[] Children;
            
            public Fish(GameObject fishPrefab, GameObject fishContainer)
            {
                ParentGameObject = Instantiate(fishPrefab, fishContainer.transform);
                ParentGameObject.SetActive(false);
                
                Children = ParentGameObject.GetComponentsInChildren<Transform>().Select(transform1 => new Child
                {
                    Transform = transform1,
                    Rigidbody = transform1.gameObject.GetComponent<Rigidbody>(),
                    InitialPosition = transform1.position,
                    InitialRotation = transform1.rotation
                }).ToArray();
            }
        }
        
        public struct Child
        {
            public Transform Transform;
            public Rigidbody Rigidbody;
            public Vector3 InitialPosition;
            public Quaternion InitialRotation;
        }
        #endregion
        
        #region ---Initialization---
        private void Start()
        {
            _fishPool = new List<Fish>();
            AddMultipleFishToPool(initialFishAmountInPool, fishObjectToPool);
        }
        #endregion
        
        #region ---PoolInteraction---
        private void AddMultipleFishToPool(int amount, GameObject fishPrefab)
        {
            for(var i = 0; i < amount; i++)
            {
                AddFishToPool(fishPrefab);
            }
        }

        private void AddFishToPool(GameObject fishPrefab)
        {
            var f = new Fish(fishPrefab, fishContainer);
            _fishPool.Add(f);
        }
        
        public static Fish GetPooledObject()
        {
            return _fishPool.FirstOrDefault(t => !t.ParentGameObject.activeInHierarchy);
        }
        #endregion
    }
}