using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace InDevelopment.Fish
{
    public class FishObjectPool : MonoBehaviour
    {
        public static FishObjectPool SharedInstance;
        public List<GameObject> pooledFishObjects;
        public GameObject fishObjectToPool;
        public int totalFishAmountInPool;
    
        void Awake()
        {
            SharedInstance = this;
        }
    
        void Start()
        {
            pooledFishObjects = new List<GameObject>();
            for(int i = 0; i < totalFishAmountInPool; i++)
            {
                // tmp = temporary gameObject that creates all the inactive Fish.
                var tmp = Instantiate(fishObjectToPool);
                tmp.SetActive(false);
                pooledFishObjects.Add(tmp);
            }
        }
        
        public GameObject GetPooledObject()
        {
            for(int i = 0; i < totalFishAmountInPool; i++)
            {
                if(!pooledFishObjects[i].activeInHierarchy)
                {
                    return pooledFishObjects[i];
                }
            }
            return null;
        }
        
    }
}
