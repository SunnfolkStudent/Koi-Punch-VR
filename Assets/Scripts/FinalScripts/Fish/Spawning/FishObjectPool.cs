using System.Collections.Generic;
using System.Linq;
using FinalScripts.Fish.FishSrubs;
using FinalScripts.Fish.Spawning.RandomWeightedTables;
using UnityEngine;

namespace FinalScripts.Fish.Spawning
{
    public class FishObjectPool : MonoBehaviour
    {
        private static List<FishPool> _fishPools;
        private static Transform _fishContainer;
        [SerializeField] private FishSrub[] fishTypes;
        
        #region ---Initialization---
        private void Awake()
        {
            _fishContainer = transform.GetChild(0);
            _fishPools = new List<FishPool>();
            foreach (var fishSrub in fishTypes)
            {
                _fishPools.Add(new FishPool(fishSrub.prefab, fishSrub.initialAmountInPool, fishSrub.zenFromFish, fishSrub.weightInRandomTable));
            }
            FishSpawnType.InitializeFishSpawnTypes(_fishPools);
        }
        #endregion
        
        #region ---FishPoolInfo---
        public class FishPool
        {
            public readonly FishRecord FishRecord;
            public readonly List<Fish> Fishes;
            public float Weight;
            public int TimesSpawned;

            public FishPool(GameObject prefab, int initialAmount, float zenFromFish, float weight)
            {
                FishRecord = new FishRecord(prefab, zenFromFish);
                Fishes = new List<Fish>();
                AddMultipleFishToPool(initialAmount, this);
                Weight = weight;
                TimesSpawned = 0;
            }
        }
        
        #region >>>---Prefab---
        public record FishRecord
        {
            public readonly GameObject GameObject;
            public readonly PrefabChild[] Children;
            public readonly float ZenAmount;
            
            public FishRecord(GameObject gameObject, float zenFromFish)
            {
                GameObject = gameObject;
                Children = gameObject.GetComponentsInChildren<Transform>().Select(transform1 => new PrefabChild(transform1)).ToArray();
                ZenAmount = zenFromFish;
            }
        }
        
        public struct PrefabChild
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
                ParentGameObject = Instantiate(fishPool.FishRecord.GameObject, _fishContainer);
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
                fish.Children[i].Transform.position = fish.FishPool.FishRecord.Children[i].InitialTransform.position;
                fish.Children[i].Transform.rotation = fish.FishPool.FishRecord.Children[i].InitialTransform.rotation;
                fish.Children[i].Transform.localScale = fish.FishPool.FishRecord.Children[i].InitialTransform.localScale;
            }
        }
        
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
        #endregion
    }
}