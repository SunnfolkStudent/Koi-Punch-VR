using System.Collections.Generic;
using System.Linq;
using InDevelopment.Fish.EditorScripts;
using UnityEngine;

namespace InDevelopment.Fish.RandomWeightedTables
{
    public static class FishSpawnAreas
    {
        // TODO: make not static
        private static List<SpawnArea> _availableSpawnAreas;
        private static List<SpawnArea> _previousNeighbours;

        private static int _chainNumber;
        private const int MaxPickRate = 5;
        private const float WeightLostFromPicked = 0.5f;
        private const float NeighborDistanceThreshold = 5f;
        private const float WeightDistributedToNeighbors = 0.75f;
        
        private class SpawnArea
        {
            public GameObject GameObject;
            public SpawnAreaCircle SpawnAreaCircle;
            public float Weight;
            public int TimesSpawned;
        }
        
        #region ---Initialization---
        public static void InitializeSpawnAreas()
        {
            _availableSpawnAreas = new List<SpawnArea>();
            AddSpawnAreasToSpawnAreaList();
        }
        
        private static void AddSpawnAreasToSpawnAreaList()
        {
            var spawnArea = GameObject.FindGameObjectsWithTag("SpawnArea");
            
            foreach (var obj in spawnArea)
            {
                var f = new SpawnArea { GameObject = obj };
                f.SpawnAreaCircle = f.GameObject.GetComponent<SpawnAreaCircle>();
                _availableSpawnAreas.Add(f);
            }
        }
        #endregion
        
        #region ---GetSpawnPosition---
        public static Vector3 GetNextFishSpawnPosition()
        {
            var spawnArea = PickSpawnArea(_availableSpawnAreas);
            return spawnArea.GameObject.transform.position + RandomOffset(spawnArea.SpawnAreaCircle.spawnAreaRadius);
        }

        private static Vector3 RandomOffset(float offsetMax)
        {
            return new Vector3(Random.Range(-offsetMax, offsetMax), 0, Random.Range(-offsetMax, offsetMax));
        }
        #endregion
        
        #region ---RandomWeightedChoice---
        private static SpawnArea PickSpawnArea(List<SpawnArea> availableSpawnAreas)
        {
            var totalWeight = availableSpawnAreas.Sum(area => area.Weight);
            var rnd = Random.Range(0, totalWeight);
            
            float sum = 0;
            foreach (var area in availableSpawnAreas)
            {
                sum += area.Weight;
                if (sum > rnd)
                {
                    if (_previousNeighbours.Contains(area))
                    {
                        _chainNumber++; 
                    } 
                    NewProbabilities(availableSpawnAreas, area);
                }
                return area;
            }
            
            return null;
        }

        private static void NewProbabilities(IReadOnlyCollection<SpawnArea> availableSpawnAreas, SpawnArea area)
        {
            NeighborsNewProbabilities(availableSpawnAreas, area);
            NewProbabilitiesForPickedArea(area);
        }
        
        private static void NewProbabilitiesForPickedArea(SpawnArea spawnArea)
        {
            spawnArea.TimesSpawned++;
            spawnArea.Weight *= WeightLostFromPicked;

            if (spawnArea.TimesSpawned >= MaxPickRate)
            {
                spawnArea.Weight = 0;
            }
        }

        private static void NeighborsNewProbabilities(IReadOnlyCollection<SpawnArea> availableSpawnAreas, SpawnArea area)
        {
            var currentNeighbors = availableSpawnAreas.Where(spawnArea =>
                (Vector3.Distance(spawnArea.GameObject.transform.position, area.GameObject.transform.position))
                <= NeighborDistanceThreshold).ToArray();
            
            if (currentNeighbors.Length !> 0) return;
            
            var weightToDistribute = area.Weight * (1 - WeightLostFromPicked);
            var weightToNeighbours = weightToDistribute * WeightDistributedToNeighbors / currentNeighbors.Length;
            var weightForTheRest = weightToDistribute * (1 - WeightDistributedToNeighbors) / (availableSpawnAreas.Count - currentNeighbors.Length);
            
            foreach (var spawnArea in _availableSpawnAreas)
            {
                spawnArea.Weight += currentNeighbors.Contains(spawnArea) ? weightToNeighbours : weightForTheRest;
            }
        }
        #endregion
        
        private static void NeighborsNewProbabilities(IReadOnlyCollection<SpawnArea> availableSpawnAreas, SpawnArea area, int neighbourChainNumber)
        {
            _previousNeighbours = new List<SpawnArea>();
            List<float> weightDistributedToNeighbours = new List<float>();
            weightDistributedToNeighbours.Add(0.9f);
            weightDistributedToNeighbours.Add(0.75f);
            weightDistributedToNeighbours.Add(0.4f);
            weightDistributedToNeighbours.Add(0.1f);
            
            var currentNeighbors = availableSpawnAreas.Where(spawnArea =>
                (Vector3.Distance(spawnArea.GameObject.transform.position, area.GameObject.transform.position))
                <= NeighborDistanceThreshold).ToArray();
            
            if (currentNeighbors.Length !> 0) return;
            
            var weightToDistribute = area.Weight * (1 - WeightLostFromPicked);
            var weightToNeighbours = weightToDistribute * weightDistributedToNeighbours[neighbourChainNumber-1] / currentNeighbors.Length;
            var weightForTheRest = weightToDistribute * (1 - weightDistributedToNeighbours[neighbourChainNumber-1]) / (availableSpawnAreas.Count - currentNeighbors.Length);
            
            foreach (var spawnArea in _availableSpawnAreas)
            {
                if (currentNeighbors.Contains(spawnArea))
                {
                    _previousNeighbours.Add(spawnArea);
                }
                spawnArea.Weight += currentNeighbors.Contains(spawnArea) ? weightToNeighbours : weightForTheRest;
            }
        }
        
    }
}