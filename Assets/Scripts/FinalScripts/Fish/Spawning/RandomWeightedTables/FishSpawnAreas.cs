using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace FinalScripts.Fish.Spawning.RandomWeightedTables
{
    public class FishSpawnAreas : MonoBehaviour
    {
        private static List<SpawnArea> _availableSpawnAreas;
        private List<SpawnArea> _previousNeighbours;
        private SpawnArea[] _previousNeighbouringAreas;
        private List<float> _weightDistributedToNeighbours;

        #region ---InspectorSettings---
        [Header("Spawn Area Settings")]
        [Tooltip("How often a spawnArea can be picked, before it's disabled")]
        [SerializeField] private int maxPickRate = 5;
        [Tooltip("SpawnArea search distance for a neighbouring spawnArea")]
        [SerializeField] private float neighborDistanceThreshold = 5f;
        
        [FormerlySerializedAs("weightLostFromPickedArea")]
        [Header("Percentage of weight lost and distributed to close neighbours")]
        [Range(0f, 1f)] [SerializeField] private float weightLossOfPickedArea = 0.5f;
        // [Range(0f, 1f)] [SerializeField]  private float weightDistributedToNeighbors = 0.75f;
        
        [Header("How long the NeighbourChain can count neighbours in a row:")]
        [SerializeField] private int chainMaxNumber = 2;
        private int _chainCurrentNumber = 0;
        
        [Header("Percentage after picking a neighbour for the n-th time in a row.")]
        [Range(0f, 1f)] [SerializeField] private float firstPercentage;
        [Range(0f, 1f)] [SerializeField] private float secondPercentage;
        [Range(0f, 1f)] [SerializeField] private float thirdPercentage;
        [Range(0f, 1f)] [SerializeField] private float fourthPercentage;
        [Range(0f, 1f)] [SerializeField] private float fifthPercentage;
        [Range(0f, 1f)] [SerializeField] private float sixthPercentage;
        #endregion
        
        private class SpawnArea
        {
            public GameObject GameObject;
            public SpawnAreaCircle SpawnAreaCircle;
            public float Weight = 100;
            public int TimesSpawned;
        }
        
        #region ---Initialization---
        public void Awake()
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
                if (sum < rnd) continue;
                NewProbabilities(availableSpawnAreas, area);
                return area;
            }
            
            return null;
        }

        private void NewProbabilities(IReadOnlyCollection<SpawnArea> availableSpawnAreas, SpawnArea area)
        {
            if (_previousNeighbouringAreas != null)
            {
                if (_previousNeighbouringAreas.Contains(area)) _chainCurrentNumber++;
                else _chainCurrentNumber = 0;
            }
            if (_chainCurrentNumber > chainMaxNumber) _chainCurrentNumber = chainMaxNumber;
            NeighborsNewProbability(availableSpawnAreas, area, _chainCurrentNumber);
            
            // Function for individually adjustable percentage changes in chain:
            // NeighborsNewProbabilities(availableSpawnAreas, area, _chainCurrentNumber);
            
            // Function for constant percentage change in chain:
            // NeighborsNewProbabilities(availableSpawnAreas, area);
            
            NewProbabilitiesForPickedArea(area);
        }
        
        private void NewProbabilitiesForPickedArea(SpawnArea spawnArea)
        {
            spawnArea.TimesSpawned++;
            spawnArea.Weight *= weightLossOfPickedArea;

            if (spawnArea.TimesSpawned >= maxPickRate)
            {
                spawnArea.Weight = 0;
            }
        }

        // Go to NewProbabilities and uncomment if you want the below function called upon:
        // (When doing so, just remember to remove the other one, by commenting it out)
        // private void NeighborsNewProbabilities(IReadOnlyCollection<SpawnArea> availableSpawnAreas, SpawnArea area)
        // {
        //     var currentNeighbors = availableSpawnAreas.Where(spawnArea =>
        //         (Vector3.Distance(spawnArea.GameObject.transform.position, area.GameObject.transform.position))
        //         <= neighborDistanceThreshold).ToArray();
        //     
        //     if (currentNeighbors.Length !> 0) return;
        //     
        //     var weightToDistribute = area.Weight * (1 - weightLostFromPickedArea);
        //     var weightToNeighbours = weightToDistribute * weightDistributedToNeighbors / currentNeighbors.Length;
        //     var weightForTheRest = weightToDistribute * (1 - weightDistributedToNeighbors) / (availableSpawnAreas.Count - currentNeighbors.Length);
        //     
        //     foreach (var spawnArea in _availableSpawnAreas)
        //     {
        //         spawnArea.Weight += currentNeighbors.Contains(spawnArea) ? weightToNeighbours : weightForTheRest;
        //     }
        // }
        #endregion
        
        private void NeighborsNewProbabilities(IReadOnlyCollection<SpawnArea> availableSpawnAreas, SpawnArea area, int neighbourChainNumber)
        {
            if (_chainCurrentNumber > chainMaxNumber) { _chainCurrentNumber = chainMaxNumber; }
            _previousNeighbours = new List<SpawnArea>();
            
            var currentNeighbors = availableSpawnAreas.Where(spawnArea =>
                (Vector3.Distance(spawnArea.GameObject.transform.position, area.GameObject.transform.position))
                <= neighborDistanceThreshold).ToArray();
            
            if (currentNeighbors.Length !> 0) return;
            
            var weightToDistribute = area.Weight * (1 - weightLossOfPickedArea);
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
        
        private void NeighborsNewProbability(IReadOnlyCollection<SpawnArea> availableSpawnAreas, SpawnArea area, int neighbourChainNumber)
        {
            _previousNeighbouringAreas = null;
            
            var currentNeighbors = availableSpawnAreas.Where(spawnArea =>
                (Vector3.Distance(spawnArea.GameObject.transform.position, area.GameObject.transform.position))
                <= neighborDistanceThreshold).ToArray();
            
            if (currentNeighbors.Length !> 0) return;
            
            var weightToDistribute = area.Weight * (1 - weightLossOfPickedArea);
            var weightToNeighbours = weightToDistribute * _weightDistributedToNeighbours[neighbourChainNumber-1] / currentNeighbors.Length;
            var weightForTheRest = weightToDistribute * (1 - _weightDistributedToNeighbours[neighbourChainNumber-1]) / (availableSpawnAreas.Count - currentNeighbors.Length);
            
            _previousNeighbouringAreas = currentNeighbors;
            foreach (var spawnArea in _availableSpawnAreas)
            {
                spawnArea.Weight += currentNeighbors.Contains(spawnArea) ? weightToNeighbours : weightForTheRest;
            }
        }
        
    }
}