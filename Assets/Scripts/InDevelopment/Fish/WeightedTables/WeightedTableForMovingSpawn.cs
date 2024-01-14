using System;
using UnityEngine;
using Random = System.Random;

namespace InDevelopment.Fish.WeightedTables
{
    /*Start:
    Get all the position of spawnAreas
    Store it to a list of structs
        Compute for the evenly distributed probability
    Store it to the list of structs
        -
        Update:
    Refer to the list of structs
    Update position of each spawnArea in struct
        Add the sum of weight of all structs
        Compute and pick a random number between the sum
    Pick a random number using alias method loop

        **If the selected spawnArea[x].picked>=maxPickRate then set the weight to 0 
    and spread its weight equally to the least picked SAs
    and update the total weight 
        and pick a new number
        **
        Otherwise proceed
        Check for neighbors
        If there are neighbors, distribute weight
        Otherwise find the closest SA and give more weight to it and self*/
    
    public class WeightedTableForMovingSpawn : MonoBehaviour
    {
        
        // if Vector3 variables can be float, then you don't need a struct.
        struct Vector3
        {
            public int x;
            public int y;
            public int z;
        }

        struct SpawnArea
        {
            public int Weight;
            public Vector3 Position;
            public int Picked;
        }

        private int _maxPickRate;

        private SpawnArea[] _spawnAreas;

        // Generally for testing : UpdateSpawnAreaPosition receives the spawnAreas array with positions, and returns a new position value.
        public void UpdateSpawnAreaPosition()
        {
            int placeholderCountOfSA = 10;
            _spawnAreas = new SpawnArea[10];

            var rnd = new Random();
            int randomNumber = 0;
            
            for (int i = 0; i < placeholderCountOfSA; i++)
            {
                randomNumber = rnd.Next(50);
                _spawnAreas[i].Position.x = randomNumber;
                _spawnAreas[i].Position.y = randomNumber;
                _spawnAreas[i].Position.z = randomNumber;
            }
        }

        public void EvenlyDistributeWeights()
        {
            int totalWeight = 0;
            int weightToBeDistributed;

            foreach (SpawnArea s in _spawnAreas)
            {
                totalWeight += s.Weight;
            }

            // dummy total weight
            totalWeight = 1000;
            int placeholderCountOfSA = 10; // this line was added in to avoid compile errors.

            weightToBeDistributed = (int)Math.Ceiling((float)totalWeight / _spawnAreas.Length);

            for(int i = 0; i < placeholderCountOfSA; i++)
            {
                _spawnAreas[i].Weight = weightToBeDistributed;
            }
        }

        // This will return the index of the neighbors of the selected SpawnArea
        public int FindIndexOfNeighbors(int x)
        {
            return new int(); // Line to be removed when function is updated.
        }

        public int PickSpawnArea()
        {
            int pickedSpawnArea = 0;
            int totalWeight = 0; // this line was added in to remove compile errors.
            var rnd = new Random();
            var pick = rnd.Next(totalWeight);
            int sum = 0;

            for (int x = 0; x < _spawnAreas.Length; x++)
            {
                sum += _spawnAreas[x].Weight;
                if (sum >= pick)
                {
                    pickedSpawnArea = x;
                    break;
                }

            }

            if (_spawnAreas[pickedSpawnArea].Picked >= _maxPickRate)
            {
                
            }

            // TODO: Check for neighbors

            return new int(); // Line to be removed when function is updated.
        }
    }
}
