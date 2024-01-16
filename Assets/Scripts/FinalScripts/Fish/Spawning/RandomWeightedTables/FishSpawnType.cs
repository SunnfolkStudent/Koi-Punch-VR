using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FinalScripts.Fish.Spawning.RandomWeightedTables
{
    public static class FishSpawnType
    {
        // TODO: make not static
        private static FishObjectPool.FishPool[] _fishPoolTypes;
        private const float WeightLostFromPicked = 0.5f;
        private const int MaxPickAmount = 5;

        #region ---Initialization---
        public static void InitializeFishSpawnTypes(List<FishObjectPool.FishPool> fishPools)
        {
            _fishPoolTypes = fishPools.ToArray();
        }
        #endregion

        #region ---GetFish---
        public static FishObjectPool.Fish GetNextFish()
        {
            return FishObjectPool.GetPooledObject(PickFishType(_fishPoolTypes));
        }
        #endregion

        #region ---RandomWeightedChoice---
        private static FishObjectPool.FishPool PickFishType(IEnumerable<FishObjectPool.FishPool> fishTypes)
        {
            var fishPools = fishTypes.ToArray();
            var totalWeight = fishPools.Sum(fishPool => fishPool.Weight);

            if (totalWeight == 0)
            {
                ResetFishTypeWeights(fishPools);   
            }
            
            var rnd = Random.Range(0, totalWeight);
            
            float sum = 0;
            foreach (var fishPool in fishPools)
            {
                sum += fishPool.Weight;
                if (sum < rnd) continue;
                NewProbabilities(fishPools, fishPool);
                return fishPool;
            }
            
            return null;
        }
        
        private static void ResetFishTypeWeights(FishObjectPool.FishPool[] fishTypes)
        {
            foreach (var differentFish in fishTypes)
            {
                differentFish.Weight = 100;
                differentFish.TimesSpawned = 0;
            }
        }
        
        private static void NewProbabilities(IReadOnlyCollection<FishObjectPool.FishPool> fishPools, FishObjectPool.FishPool fishPool)
        {
            fishPool.TimesSpawned++;
            fishPool.Weight *= WeightLostFromPicked;

            if (fishPool.TimesSpawned >= MaxPickAmount)
            {
                fishPool.Weight = 0;
                return;
            }
            
            var weightToDistribute = fishPool.Weight * (1 - WeightLostFromPicked);
            var weightForOthers = weightToDistribute / fishPools.Count;
            
            foreach (var pool in fishPools)
            {
                pool.Weight += weightForOthers;
            }
        }
        #endregion
    }
}