using System.Collections.Generic;
using System.Linq;
using FinalScripts.Fish.Spawning.RandomWeightedTables;
using UnityEngine;

namespace FinalScripts.Fish.Spawning
{
    public class FishObjectPool : MonoBehaviour
    {
        [SerializeField] private FishSrub[] fishTypes;
        private static List<FishPool> _fishPools;
        private static Transform _fishContainer;
        
        #region ---Initialization---
        private void Awake()
        {
            _fishContainer = transform.GetChild(0);
            _fishPools = new List<FishPool>();
            foreach (var fishSrub in fishTypes)
            {
                _fishPools.Add(new FishPool(fishSrub));
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
            
            public FishPool(FishSrub fishSrub)
            {
                FishRecord = new FishRecord(fishSrub);
                Fishes = new List<Fish>();
                AddMultipleFishToPool(fishSrub.initialAmountInPool, this);
                Weight = fishSrub.weightInRandomTable;
                TimesSpawned = 0;
            }
        }
        
        #region >>>---FishRecord---
        public record FishRecord
        {
            public readonly FishSrub FishSrub;
            public readonly FishRecordChild[] Children;
            
            public FishRecord(FishSrub fishSrub)
            {
                FishSrub = fishSrub;
                if (!fishSrub.prefab.GetComponent<FinalScripts.Fish.Fish>()) fishSrub.prefab.AddComponent<FinalScripts.Fish.Fish>();
                Children = fishSrub.prefab.gameObject.GetComponentsInChildren<Transform>().Select(transform1 => new FishRecordChild(transform1)).ToArray();
            }
        }
        
        public struct FishRecordChild
        {
            public readonly Transform InitialTransform;
            public readonly Rigidbody Rigidbody;
            
            public FishRecordChild(Transform transform)
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
                ParentGameObject = Instantiate(fishPool.FishRecord.FishSrub.prefab, _fishContainer);
                ParentGameObject.SetActive(false);
                ParentGameObject.GetComponent<FinalScripts.Fish.Fish>().fish = this;
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