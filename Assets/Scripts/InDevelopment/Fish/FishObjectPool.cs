using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
        public struct PrefabChild
        {
            public readonly Transform InitialTransform;

            public PrefabChild(Transform transform)
            {
                InitialTransform = transform;
            }
        }
        #endregion
        
        #region >>>---Fish---
        public class Fish
        {
            public readonly GameObject ParentGameObject;
            public readonly Child[] Children;
            
            public Fish(GameObject fishPrefab)
            {
                ParentGameObject = Instantiate(fishPrefab, _fishContainer);
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
                AddFishToPool(fishPool);
            }
        }

        public static void AddFishToPool(FishPool fishPool)
        {
            fishPool.Fishes.Add(new Fish(fishPool.Prefab.GameObject));
        }
        
        public static Fish GetPooledObject(FishPool fishPool)
        {
            var availableFishInPool = fishPool.Fishes.Where(fish => !fish.ParentGameObject.activeInHierarchy).ToArray();
            if (availableFishInPool.Length < 2) AddFishToPool(fishPool);
            return availableFishInPool[0];
        }
        
        public static void DespawnFish(Fish fish)
        {
            ResetPropertiesOfFishInPool(fish, FishPools.Find(fish => FishPools.Contains(fish)));
            fish.ParentGameObject.SetActive(false);
        }
        
        public static void ResetPropertiesOfFishInPool(Fish fish, FishPool fishPool)
        {
            for (var i = 0; i < fish.Children.Length; i++)
            {
                fish.Children[i].Transform.position = fishPool.Prefab.Children[i].InitialTransform.position;
                fish.Children[i].Transform.rotation = fishPool.Prefab.Children[i].InitialTransform.rotation;
                fish.Children[i].Transform.localScale = fishPool.Prefab.Children[i].InitialTransform.localScale;
            }
        }

        private static FishPool FindPoolOfFish(Fish fish)
        {
            return FishPools.Find(fish => FishPools.Contains(fish));
        }
        #endregion
    }
}