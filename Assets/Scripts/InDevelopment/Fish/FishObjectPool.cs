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

        public class Fish
        {
            public GameObject FishGameObject;
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
                // tmp = temporary gameObject that creates all the inactive Fish.
                // var tmp = Instantiate(fishObjectToPool);
                // tmp.SetActive(false);
                
                var f = new Fish { FishGameObject = Instantiate(fishObjectToPool) };
                f.Rigidbody = f.FishGameObject.GetComponent<Rigidbody>();
                f.FishGameObject.SetActive(false);
                _pooledFishObjects.Add(f);
            }
        }
        
        public Fish GetPooledObject()
        {
            for(var i = 0; i < totalFishAmountInPool; i++)
            {
                if(!_pooledFishObjects[i].FishGameObject.activeInHierarchy)
                {
                    return _pooledFishObjects[i];
                }
            }
            return null;
        }
    }
}
