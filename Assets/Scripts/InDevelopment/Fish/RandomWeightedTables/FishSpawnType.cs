using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InDevelopment.Fish.RandomWeightedTables
{
    public static class FishSpawnType
    {
        private static FishObjectPool.FishPool[] _fishPoolTypes;
        private const float WeightLostFromPicked = 0.5f;
        private const int MaxPickAmount = 5;

        public static void InitializeFishSpawnTypes(List<FishObjectPool.FishPool> fishPools)
        {
            _fishPoolTypes = fishPools.ToArray();
        }
        
        public static FishObjectPool.Fish GetNextFishSpawnType()
        {
            return FishObjectPool.GetPooledObject(PickFishType(_fishPoolTypes));
        }
        
        private static FishObjectPool.FishPool PickFishType(IEnumerable<FishObjectPool.FishPool> fishTypes)
        {
            var fishPools = fishTypes as FishObjectPool.FishPool[] ?? fishTypes.ToArray();
            var weightedTableTotalWeight = fishPools.Sum(prefab => prefab.Weight);
            var rnd = Random.Range(0, weightedTableTotalWeight);
            
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
    }
}