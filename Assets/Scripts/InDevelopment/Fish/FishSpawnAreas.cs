using System.Collections.Generic;
using System.Linq;
using InDevelopment.Fish.EditorScripts;
using UnityEngine;

namespace InDevelopment.Fish
{
    public static class FishSpawnAreas
    {
        private const int MaxPickRate = 5;
        private static List<SpawnArea> _availableSpawnAreas;
        private static float _weightedTableTotalWeight;
        
        // TODO: Remove function after testing of random weighted tables
        // private static SpawnArea RandomSpawnArea()
        // {
        //     return _availableSpawnAreas[Random.Range(0, _availableSpawnAreas.Count)];
        // }
        
        public static void InitializeSpawnAreas()
        {
            _availableSpawnAreas = new List<SpawnArea>();
            AddSpawnAreasToSpawnAreaList();
        }
        
        public class SpawnArea
        {
            public GameObject GameObject;
            public SpawnAreaCircle SpawnAreaCircle;
            public float Weight;
            public int TimesSpawned;
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
        
        public static SpawnArea PickSpawnArea()
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
        
        public static Vector3 RandomOffset(float offsetMax)
        {
            return new Vector3(Random.Range(-offsetMax, offsetMax), 0, Random.Range(-offsetMax, offsetMax));
        }
    }
}