using System.Collections.Generic;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class FishObjectPool : MonoBehaviour
    {
        public static FishObjectPool SharedInstance;
        private List<Fish> _pooledFishObjects;
        public GameObject fishObjectToPool;
        public int totalFishAmountInPool;
        
        [SerializeField] private GameObject fishContainer;

        public class Fish
        {
            public GameObject GameObject;
            public Rigidbody Rigidbody;
        }

        private void Awake()
        {
            SharedInstance = this;
        }

        private void Start()
        {
            _pooledFishObjects = new List<Fish>();
            for(var i = 0; i < totalFishAmountInPool; i++)
            {
                var f = new Fish { GameObject = Instantiate(fishObjectToPool) };
                f.Rigidbody = f.GameObject.GetComponent<Rigidbody>();
                f.GameObject.SetActive(false);
                f.GameObject.transform.SetParent(fishContainer.transform);
                _pooledFishObjects.Add(f);
            }
        }
        
        public Fish GetPooledObject()
        {
            for(var i = 0; i < totalFishAmountInPool; i++)
            {
                if(!_pooledFishObjects[i].GameObject.activeInHierarchy)
                {
                    return _pooledFishObjects[i];
                }
            }
            return null;
        }
    }
}
