using System.Collections.Generic;
using System.Linq;
using InDevelopment.Fish.EditorScripts;
using UnityEngine;

namespace InDevelopment.Fish.RandomWeightedTables
{
    public class FishSpawnAreas : MonoBehaviour
    {
        private List<SpawnArea> _availableSpawnAreas;
        private List<SpawnArea> _previousNeighbours;
        private List<float> _weightDistributedToNeighbours;

        [Header("How often a spawnArea can be picked, before it's disabled:")]
        [Range(1f, 10f)] [SerializeField] private int maxPickRate = 5;
        [Header("SpawnArea Vector-based search area for a neighbour spawnArea")]
        [Range(0f, 8f)] [SerializeField] private float neighborDistanceThreshold = 5f;
        
        private int _chainCurrentNumber;
        [Header("How long the NeighbourChain can count neighbours in a row (Max 6):")]
        [Range(1f, 6f)][SerializeField] private int chainMaxNumber;
        
        [Header("Percentage after picking a neighbour for the n-th time in a row.")]
        [Range(0f, 1f)] [SerializeField] private float firstPercentage;
        [Range(0f, 1f)] [SerializeField] private float secondPercentage;
        [Range(0f, 1f)] [SerializeField] private float thirdPercentage;
        [Range(0f, 1f)] [SerializeField] private float fourthPercentage;
        [Range(0f, 1f)] [SerializeField] private float fifthPercentage;
        [Range(0f, 1f)] [SerializeField] private float sixthPercentage;
        
        // The below can be used if you want to use a constant percentage decrease in the NeighbourChain.
        private float _weightLostFromPicked = 0.5f;
        [Range(0f, 1f)] private float _weightDistributedToNeighbors = 0.75f;
        
        private class SpawnArea
        {
            public GameObject GameObject;
            public SpawnAreaCircle SpawnAreaCircle;
            public float Weight;
            public int TimesSpawned;
        }
        
        #region ---Initialization---
        public void InitializeSpawnAreas()
        {
            _availableSpawnAreas = new List<SpawnArea>();
            AddSpawnAreasToSpawnAreaList();
            InitializeNeighbourPercentages();
        }

        private void InitializeNeighbourPercentages()
        {
            _weightDistributedToNeighbours = new List<float>
            {
                firstPercentage,
                secondPercentage,
                thirdPercentage,
                fourthPercentage,
                fifthPercentage,
                sixthPercentage
            };
        }
        
        private void AddSpawnAreasToSpawnAreaList()
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
        public Vector3 GetNextFishSpawnPosition()
        {
            var spawnArea = PickSpawnArea(_availableSpawnAreas);
            return spawnArea.GameObject.transform.position + RandomOffset(spawnArea.SpawnAreaCircle.spawnAreaRadius);
        }

        private Vector3 RandomOffset(float offsetMax)
        {
            return new Vector3(Random.Range(-offsetMax, offsetMax), 0, Random.Range(-offsetMax, offsetMax));
        }
        #endregion
        
        #region ---RandomWeightedChoice---
        private SpawnArea PickSpawnArea(List<SpawnArea> availableSpawnAreas)
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
                        _chainCurrentNumber++; 
                    } 
                    NewProbabilities(availableSpawnAreas, area);
                }
                return area;
            }
            
            return null;
        }

        private void NewProbabilities(IReadOnlyCollection<SpawnArea> availableSpawnAreas, SpawnArea area)
        {
            // Function for individually adjustable percentage changes in chain:
            NeighborsNewProbabilities(availableSpawnAreas, area, _chainCurrentNumber);
            
            // Function for constant percentage change in chain:
            // NeighborsNewProbabilities(availableSpawnAreas, area);
            
            NewProbabilitiesForPickedArea(area);
        }
        
        private void NewProbabilitiesForPickedArea(SpawnArea spawnArea)
        {
            spawnArea.TimesSpawned++;
            spawnArea.Weight *= _weightLostFromPicked;

            if (spawnArea.TimesSpawned >= maxPickRate)
            {
                spawnArea.Weight = 0;
            }
        }

        // Go to NewProbabilities and uncomment if you want the below function called upon:
        // (When doing so, just remember to remove the other one, by commenting it out)
        private void NeighborsNewProbabilities(IReadOnlyCollection<SpawnArea> availableSpawnAreas, SpawnArea area)
        {
            var currentNeighbors = availableSpawnAreas.Where(spawnArea =>
                (Vector3.Distance(spawnArea.GameObject.transform.position, area.GameObject.transform.position))
                <= neighborDistanceThreshold).ToArray();
            
            if (currentNeighbors.Length !> 0) return;
            
            var weightToDistribute = area.Weight * (1 - _weightLostFromPicked);
            var weightToNeighbours = weightToDistribute * _weightDistributedToNeighbors / currentNeighbors.Length;
            var weightForTheRest = weightToDistribute * (1 - _weightDistributedToNeighbors) / (availableSpawnAreas.Count - currentNeighbors.Length);
            
            foreach (var spawnArea in _availableSpawnAreas)
            {
                spawnArea.Weight += currentNeighbors.Contains(spawnArea) ? weightToNeighbours : weightForTheRest;
            }
        }
        #endregion
        
        private void NeighborsNewProbabilities(IReadOnlyCollection<SpawnArea> availableSpawnAreas, SpawnArea area, int neighbourChainNumber)
        {
            if (_chainCurrentNumber > chainMaxNumber) { _chainCurrentNumber = chainMaxNumber; }
            _previousNeighbours = new List<SpawnArea>();

            var currentNeighbors = availableSpawnAreas.Where(spawnArea =>
                (Vector3.Distance(spawnArea.GameObject.transform.position, area.GameObject.transform.position))
                <= neighborDistanceThreshold).ToArray();
            
            if (currentNeighbors.Length !> 0) return;
            
            var weightToDistribute = area.Weight * (1 - _weightLostFromPicked);
            var weightToNeighbours = weightToDistribute * _weightDistributedToNeighbours[neighbourChainNumber-1] / currentNeighbors.Length;
            var weightForTheRest = weightToDistribute * (1 - _weightDistributedToNeighbours[neighbourChainNumber-1]) / (availableSpawnAreas.Count - currentNeighbors.Length);
            
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