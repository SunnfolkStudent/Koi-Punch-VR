using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = System.Random;

namespace InDevelopment.Fish
{
    // Using the Alias method for our WeightedTable.
    public class WeightedTableScript : MonoBehaviour
    {
        [SerializeField] private int minPickRate;
        [SerializeField] private int maxPickRate;
        
        // We create our own Dictionary with all spawn areas, and attach the probability. 
        private Dictionary<string, int> CreateSpawnAreas (int numSpawnAreas)
        {
            int numberOfSpawnAreas = numSpawnAreas;
            
            int probabilityOfEachArea = (int)Math.Ceiling((1 / (float) numberOfSpawnAreas) * 100);
            Dictionary<string, int> spawnAreas = new Dictionary<string, int>();
            
            // We loop until we reach max numberOfSpawnAreas.
            for (int i = 1; i <= numberOfSpawnAreas; i++)
            {
                // We convert the i to string, because the Dictionary requires a string key. Int 1 = String 1, which holds probability int x.
                // Int 2 = String 2, which holds probability int y, and so on down the list of totalSpawnAreas.
                spawnAreas.Add(i.ToString(), probabilityOfEachArea);
            }
            return spawnAreas;
        }

        // TODO: Know how much a key has been chosen, update the totalSpawnAreas.Value, and remove it from being picked.
        
        private string PickSpawnArea(Dictionary<string, int> dictionaryOfSpawnAreas)
        {
            var rnd = new Random();
            var pick = rnd.Next(dictionaryOfSpawnAreas.Values.Sum());

            int sum = 0;
            string result = "";

            // counter = iteration running in the loop. 
            foreach (var counter in dictionaryOfSpawnAreas)
            {
                // counter.Value is added to the sum, and we get a new sum on the left. "+=" is kinda like an arrow <-, if it helps remember.
                sum += counter.Value;
                if(sum >= pick)
                {
                    // we match both strings together, and is returned below to whoever called PickSpawnArea.
                    result = counter.Key;
                    break;
                }
            }
            return result;
        }

        // We'll call the function outside this class, with "UpdatedSpawnAreas = CalculateNewProbabilities(SpawnAreas, 2)";
        
        private Dictionary<string, int> CalculateNewProbabilities(Dictionary<string, int> spawnAreas, string recentResult)
        {
            var updatedSpawnAreas = spawnAreas;
            int newProbability = (int)Math.Ceiling((float)updatedSpawnAreas[recentResult] / 2);

            int integerResult = Convert.ToInt32(recentResult);
            string previousKey = "";
            string nextKey = "";

            nextKey = integerResult + 1 > spawnAreas.Count ? "1" : (integerResult + 1).ToString();
            previousKey = integerResult - 1 <= 0 ? spawnAreas.Count.ToString() : (integerResult - 1).ToString();

            updatedSpawnAreas[recentResult] = newProbability;
            updatedSpawnAreas[nextKey] += newProbability;
            updatedSpawnAreas[previousKey] += newProbability;
            
            return updatedSpawnAreas;
        }

        private int[] UpdateRemainingPicks(int[] remainingPicks, int chosenIndex)
        {
            // TODO: Add the methods to help update remainingPicks according to max- and minPickRate.
            
            return remainingPicks;
        }
        
    }
}
