using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class FishObjectPool : MonoBehaviour
    {
        public static List<FishPool> FishPools;
        [SerializeField] private InspectorPrefab[] fishPrefab;
        
        [Serializable] private class InspectorPrefab
        {
            public GameObject prefabGameObject;
            public int initialAmountInPool;
        }
        
        #region ---Initialization---
        private void Awake()
        {
            FishPools = new List<FishPool>();
            foreach (var prefab in fishPrefab)
            {
                FishPools.Add(new FishPool(prefab.prefabGameObject, prefab.initialAmountInPool));
            }
        }
        #endregion
        
        #region ---FishPoolInfo---
        public class FishPool
        {
            public readonly Prefab Prefab;
            public readonly List<Fish> Fishes;

            public FishPool(GameObject prefab, int initialAmount)
            {
                Prefab = new Prefab(prefab);
                Fishes = new List<Fish>();
                AddMultipleFishToPool(initialAmount, this);
            }
        }
        
        #region >>>---Prefab---
        public record Prefab
        {
            public readonly GameObject GameObject;
            public readonly PrefabChild[] Children;
            
            public Prefab(GameObject gameObject)
            {
                GameObject = gameObject;
                Children = gameObject.GetComponentsInChildren<Transform>().Select(transform1 => new PrefabChild(transform1)).ToArray();
            }
        }
        
        public class PrefabChild
        {
            public InitialTransform InitialTransform;

            public PrefabChild(Transform transform)
            {
                InitialTransform = new InitialTransform(transform);
            }
        }
        
        public struct InitialTransform
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 LocalScale;

            public InitialTransform(Transform transform)
            {
                Position = transform.position;
                Rotation = transform.rotation;
                LocalScale = transform.localScale;
            }
        }
        #endregion
        
        #region >>>---Fish---
        public record Fish
        {
            public readonly GameObject ParentGameObject;
            public readonly Child[] Children;
            
            public Fish(GameObject fishPrefab)
            {
                ParentGameObject = Instantiate(fishPrefab);
                ParentGameObject.SetActive(false);
                
                Children = ParentGameObject.GetComponentsInChildren<Transform>().Select(transform1 => new Child(transform1)).ToArray();
            }
        }
        
        public class Child
        {
            public readonly Transform Transform;
            public readonly Rigidbody Rigidbody;
            
            public Child(Transform transform)
            {
                Transform = transform;
                Rigidbody = transform.gameObject.GetComponent<Rigidbody>();
            }
        }
        #endregion
        #endregion
        
        #region ---PoolInteraction---
        private static void AddMultipleFishToPool(int amount, FishPool fishPool)
        {
            for(var i = 0; i < amount; i++)
            {
                AddFishToPool(fishPool);
            }
        }

        public static void AddFishToPool(FishPool fishPool)
        {
            fishPool.Fishes.Add(new Fish(fishPool.Prefab.GameObject));
        }
        
        public static Fish GetPooledObject(FishPool fishPool)
        {
            return fishPool.Fishes.FirstOrDefault(t => !t.ParentGameObject.activeInHierarchy);
        }
        #endregion
    }
}