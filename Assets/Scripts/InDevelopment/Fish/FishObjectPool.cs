using System.Collections.Generic;
using System.Linq;
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
            public readonly GameObject ParentGameObject;
            public readonly Child[] Children;

            public Fish(GameObject parentGameObject, GameObject fishContainer)
            {
                ParentGameObject = parentGameObject;
                ParentGameObject.SetActive(false);
                ParentGameObject.transform.SetParent(fishContainer.transform);
                
                Children = ParentGameObject.GetComponentsInChildren<Transform>().Select(transform1 => new Child
                {
                    Transform = transform1, 
                    ChildRigidbody = transform1.gameObject.GetComponent<Rigidbody>(),
                    InitialPosition = transform1.position,
                    InitialRotation = transform1.rotation
                }).ToArray();
            }
        }

        public struct Child
        {
            public Transform Transform;
            public Rigidbody ChildRigidbody;
            public Vector3 InitialPosition;
            public Quaternion InitialRotation;
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
                AddFishToPool(fishObjectToPool);
            }
        }

        private void AddFishToPool(GameObject fishPrefab)
        {
            var f = new Fish(Instantiate(fishPrefab), fishContainer);
            _pooledFishObjects.Add(f);
        }

        public Fish GetPooledObject()
        {
            for(var i = 0; i < totalFishAmountInPool; i++)
            {
                if(!_pooledFishObjects[i].ParentGameObject.activeInHierarchy)
                {
                    return _pooledFishObjects[i];
                }
            }
            return null;
        }
    }
}