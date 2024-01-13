using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class FishObjectPool : MonoBehaviour
    {
        // TODO: Other properties on fish
        public static List<FishPool> FishPools;
        private static Transform _fishContainer;
        [SerializeField] private InspectorPrefab[] fishPrefab;
        
        [Serializable] private class InspectorPrefab
        {
            public GameObject prefabGameObject;
            public int initialAmountInPool;
        }
        
        #region ---Initialization---
        private void Awake()
        {
            _fishContainer = transform.GetChild(0);
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
        
        public record PrefabChild
        {
            public readonly Transform InitialTransform;
            public readonly Rigidbody Rigidbody;

            public PrefabChild(Transform transform)
            {
                InitialTransform = transform;
                Rigidbody = transform.gameObject.GetComponent<Rigidbody>();
            }
        }
        #endregion
        
        #region >>>---Fish---
        public class Fish
        {
            public readonly FishPool FishPool;
            public readonly GameObject ParentGameObject;
            public readonly Child[] Children;
            
            public Fish(FishPool fishPool)
            {
                FishPool = fishPool;
                ParentGameObject = Instantiate(fishPool.Prefab.GameObject, _fishContainer);
                ParentGameObject.SetActive(false);
                
                Children = ParentGameObject.GetComponentsInChildren<Transform>().Select(transform1 => new Child(transform1)).ToArray();
                
                ParentGameObject.GetComponent<InDevelopment.Fish.Fish>().fish = this;
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
                AddFishInPool(fishPool);
            }
        }

        private static void AddFishInPool(FishPool fishPool)
        {
            fishPool.Fishes.Add(new Fish(fishPool));
        }
        
        public static Fish GetPooledObject(FishPool fishPool)
        {
            var availableFishInPool = fishPool.Fishes.Where(fish => !fish.ParentGameObject.activeInHierarchy).ToArray();
            if (availableFishInPool.Length < 2) AddFishInPool(fishPool);
            return availableFishInPool[0];
        }
        
        public static void DespawnFish(Fish fish)
        {
            ResetPropertiesOfFishInPool(fish);
            fish.ParentGameObject.SetActive(false);
        }

        private static void ResetPropertiesOfFishInPool(Fish fish)
        {
            for (var i = 0; i < fish.Children.Length; i++)
            {
                fish.Children[i].Transform.position = fish.FishPool.Prefab.Children[i].InitialTransform.position;
                fish.Children[i].Transform.rotation = fish.FishPool.Prefab.Children[i].InitialTransform.rotation;
                fish.Children[i].Transform.localScale = fish.FishPool.Prefab.Children[i].InitialTransform.localScale;
            }
        }
        #endregion
    }
}