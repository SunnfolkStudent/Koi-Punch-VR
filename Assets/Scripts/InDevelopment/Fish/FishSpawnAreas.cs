using System.Collections.Generic;
using System.Linq;
using InDevelopment.Fish.EditorScripts;
using UnityEngine;

namespace InDevelopment.Fish
{
    public static class FishSpawnAreas
    {
        // TODO: Change RandomWeightedTables to get desired effect
        // TODO: Remove depreciated function at the bottom after testing of random weighted tables
        private const int MaxPickRate = 5;
        private static List<SpawnArea> _availableSpawnAreas;
        private static float _weightedTableTotalWeight;
        
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
            var spawnArea = PickSpawnArea();
            return spawnArea.GameObject.transform.position + FishSpawnAreas.RandomOffset(spawnArea.SpawnAreaCircle.spawnAreaRadius);
        }

        private static Vector3 RandomOffset(float offsetMax)
        {
            return new Vector3(Random.Range(-offsetMax, offsetMax), 0, Random.Range(-offsetMax, offsetMax));
        }
        #endregion
        
        #region ---RandomWeightedTables---
        private static SpawnArea PickSpawnArea()
        {
            _weightedTableTotalWeight = _availableSpawnAreas.Sum(area => area.Weight);
            var rnd = Random.Range(0, _weightedTableTotalWeight);
            
            float sum = 0;
            foreach (var area in _availableSpawnAreas)
            {
                sum += area.Weight;
                if (sum < rnd) continue;
                NewProbabilitiesFor(area);
                return area;
            }
            
            return null;
        }
        
        private static void NewProbabilitiesFor(SpawnArea spawnArea)
        {
            spawnArea.TimesSpawned++;
            spawnArea.Weight /= 2;

            if (spawnArea.TimesSpawned >= MaxPickRate)
            {
                _availableSpawnAreas.Remove(spawnArea);
            }
        }
        #endregion
        
        // private static SpawnArea RandomSpawnArea()
        // {
        //     return _availableSpawnAreas[Random.Range(0, _availableSpawnAreas.Count)];
        // }
    }
}