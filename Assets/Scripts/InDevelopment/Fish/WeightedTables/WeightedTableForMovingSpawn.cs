using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using VHierarchy.Libs;
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
        If there are neighbors, equally redistribute all weights so that neighbours in total have a 90% chance of being picked.
        As neighbours keep getting picked, decrease pick rate (1st pick 90% neighbour chance, 2nd pick 70% neighbour chance, 3rd pick 50% neighbour chance).
        if no neighbours are present, find the closest SA and give more weight to it and current SA*/

    public class WeightedTableForMovingSpawn : MonoBehaviour
    {
        struct SpawnArea
        {
            public int weight;
            public Vector3 position;
            public int picked;
        }

        private int _maxPickRate;
        private int _totalWeight;

        private SpawnArea[] SpawnAreas;

        public void UpdateSpawnAreaPosition()
        {
            int placeholderCountOfSA = 10;
            SpawnAreas = new SpawnArea[10];

            var rnd = new Random();
            int randomNumber = 0;

            for (int i = 0; i < placeholderCountOfSA; i++)
            {
                randomNumber = rnd.Next(50);
                SpawnAreas[i].position.x = randomNumber;
                SpawnAreas[i].position.y = randomNumber;
                SpawnAreas[i].position.z = randomNumber;
            }
        }

        public void EvenlyDistributeWeights()
        {
            int totalWeight = 0;
            int weightToBeDistributed = 0;

            foreach (SpawnArea s in SpawnAreas)
            {
                totalWeight += s.weight;
            }

            // dummy total weight
            totalWeight = 1000;

            weightToBeDistributed = (int)Math.Ceiling((float)totalWeight / SpawnAreas.Length);

            for (int i = 0; i < SpawnAreas.Length; i++)
            {
                SpawnAreas[i].weight = weightToBeDistributed;
            }
        }

        private float _neighborThreshold;
        private List<int> _indicesOfPreviousNeighbors;

        private List<int> _indicesOfCurrentNeighbors;

        private void UpdateIndicesOfCurrentNeighbors(int indexOfCurrentChosen)
        {
            _indicesOfPreviousNeighbors = _indicesOfCurrentNeighbors;
            _indicesOfCurrentNeighbors = new List<int>();

            for (int i = 0; i < SpawnAreas.Length; i++)
            {
                if ((SpawnAreas[i].position - SpawnAreas[indexOfCurrentChosen].position).magnitude <= _neighborThreshold)
                {
                    _indicesOfCurrentNeighbors.Add(i);
                }
            }

        }

        public void UpdateWeightOfCurrentNeighbors()
        {


        }

        /*IEnumerator NeighbourChain()
        {
            // Grant 90% weight to neighbours
            
            yield return new WaitUntil (bool neighbourIsChosen);
            
            // Grant 70% weight to neighbours
            
            yield return new WaitUntil (bool neighbourIsChosen);
            
            // Grant 50% weight to neighbours
            
            yield return new WaitUntil (bool neighbourIsChosen);
            
            // Grant 30% weight to neighbours

            yield return new WaitUntil(bool neighbourIsChosen);
        }*/

        private void UpdateTotalWeight()
        {
            _totalWeight = 0;

            for (int i = 0; i < SpawnAreas.Length; i++)
            {
                _totalWeight += SpawnAreas[i].weight;
            }
        }

        public int PickSpawnArea()
        {
            UpdateSpawnAreaPosition();
            
            //Remove the weight if the SA reached maxPickedRate
            for (int x = 0; x < SpawnAreas.Length; x++)
            {
                if (SpawnAreas[x].picked >= _maxPickRate)
                {
                    SpawnAreas[x].weight = 0;
                }
            }
            
            UpdateTotalWeight();

            int pickedSpawnArea = 0;
            var rnd = new Random();

            // Picks a random number between 1 and total weight of all SA
            var pick = rnd.Next(_totalWeight);
            int sum = 0;

            // Let's say SpawnArea[0] has 100 weight, SpawnArea[1] has 200 weight, totalWeight is 300 and pick is 129
            // First iteration where x=0 and sum = 100; sum isn't >= 129 therefore SpawnArea[0] isn't the picked SA
            for (int x = 0; x < SpawnAreas.Length; x++)
            {
                sum += SpawnAreas[x].weight;
                if (sum >= pick)
                {
                    pickedSpawnArea = x;
                    break;
                }
            }

            // This also updates indicesOfPreviousNeighbors
            UpdateIndicesOfCurrentNeighbors(pickedSpawnArea);

            // This is working under the assumption that indicesOfCurrentNeighbors and indicesOfPreviousNeighbors are already updated
            UpdateWeightOfCurrentNeighbors();

            return pickedSpawnArea;

        }
    }
}
