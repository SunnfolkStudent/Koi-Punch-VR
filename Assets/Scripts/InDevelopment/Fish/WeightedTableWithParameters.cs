using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace InDevelopment.Fish
{
    // We create a weightedTable by using an array of the struct SpawnArea vs using Dictionary; 
    // it gives us more freedom to add extra parameters as needed, with <int, int>, vs Dictionary <string, int>.
    
    // In this WeightedTable you can add more of your own parameters, and you can change the max size of the array as
    // needed depending on level and need. Works best with numbers between 3-8. 
    
    public class WeightedTableWithParameters : MonoBehaviour
    {
        struct SpawnArea
        {
            public int Weight;
            public int NumberOfPicked;
        }

        private int _minPickRate;
        private int _maxPickRate;

        public int pickedIndex;

        private SpawnArea[] _spawnAreas;
        public int totalWeight;

        public void InitializeSpawnAreas(int spawnAreasCount)
        {
            pickedIndex = 0;
            _maxPickRate = 10;
            int count = spawnAreasCount;
            int probabilityOfEachArea = (int)Math.Ceiling(1 / (float)count) * 100;

            _spawnAreas = new SpawnArea[count];

            for (int x = 0; x < count; x++)
            {
                _spawnAreas[x].NumberOfPicked = 0;
                _spawnAreas[x].Weight = probabilityOfEachArea;
            }

        }

        // Removes the weight in SpawnAreas that exceed the maximum number of picks, then
        // Calculates the totalWeight of all spawn area in the array SpawnAreas, then
        // Generates a random number between 1 and totalWeight, then
        // Updates the new weight and numberOfPicked of each SpawnArea in SpawnAreas, then
        // Returns the index of the picked spawn area.
        
        public int PickSpawnArea()
        {
            for (int x = 0; x < _spawnAreas.Length; x++)
            {
                if (_spawnAreas[x].NumberOfPicked >= _maxPickRate)
                {
                    RemoveAndSpreadWeight(x);
                }
            }

            int res = 0;
            totalWeight = 0;

            for (int x = 0; x < _spawnAreas.Length; x++)
            {
                totalWeight += _spawnAreas[x].Weight;
            }
            
            var rnd = new Random();
            var pick = rnd.Next(totalWeight);
            int sum = 0;

            for (int x = 0; x < _spawnAreas.Length; x++)
            {
                sum += _spawnAreas[x].Weight;
                if (sum >= pick)
                {
                    pickedIndex = x;
                    break;
                }

            }
            _spawnAreas[pickedIndex].NumberOfPicked++;
            CalculateNewProbabilities(pickedIndex);

            return pickedIndex;
        }

        private void CalculateNewProbabilities(int indexOfPickedSpawnArea)
        {
            int newProbability = (int)Math.Ceiling((float) _spawnAreas[indexOfPickedSpawnArea].Weight / 2);

            _spawnAreas[indexOfPickedSpawnArea].Weight = newProbability;

            int indexOfPreviousSpawnArea = indexOfPickedSpawnArea - 1 < 0 ? _spawnAreas.Length - 1 : indexOfPickedSpawnArea - 1;
            int indexOfNextSpawnArea = indexOfPickedSpawnArea + 1 > (_spawnAreas.Length - 1) ? 0 : indexOfPickedSpawnArea + 1;

            if (_spawnAreas[indexOfPreviousSpawnArea].NumberOfPicked <= _maxPickRate)
            {
                _spawnAreas[indexOfPreviousSpawnArea].Weight += (int)Math.Ceiling((double)newProbability / 2);
            }
            if (_spawnAreas[indexOfNextSpawnArea].NumberOfPicked <= _maxPickRate)
            {
                _spawnAreas[indexOfNextSpawnArea].Weight += (int)Math.Ceiling((double)newProbability / 2);
            }
        }

        private void RemoveAndSpreadWeight(int indexOfPickedSpawnArea)
        {
            int weightToBeDistributed = (int)Math.Ceiling((float)_spawnAreas[indexOfPickedSpawnArea].Weight / 2);

            _spawnAreas[indexOfPickedSpawnArea].Weight = 0;

            int indexOfPreviousSpawnArea = indexOfPickedSpawnArea - 1 < 0 ? _spawnAreas.Length - 1 : indexOfPickedSpawnArea - 1;
            int indexOfNextSpawnArea = indexOfPickedSpawnArea + 1 > (_spawnAreas.Length - 1) ? 0 : indexOfPickedSpawnArea + 1;

            _spawnAreas[indexOfPreviousSpawnArea].Weight += weightToBeDistributed;
            _spawnAreas[indexOfNextSpawnArea].Weight += weightToBeDistributed;
            
        }

        // For debugging
        public int[] GetSpawnAreasWeight()
        {
            int[] weightOfAllSpawnAreas = new int[_spawnAreas.Length];

            for (int x = 0; x < _spawnAreas.Length; x++)
            {
                weightOfAllSpawnAreas[x] = _spawnAreas[x].Weight;
            }
            return weightOfAllSpawnAreas;
        }

        // For debugging
        public int[] GetSpawnAreasPicked()
        {
            int[] pickedOfAllSpawnAreas = new int[_spawnAreas.Length];

            for (int x = 0; x < _spawnAreas.Length; x++)
            {
                pickedOfAllSpawnAreas[x] = _spawnAreas[x].NumberOfPicked;
            }

            return pickedOfAllSpawnAreas;
        }
    }
}
