using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InDevelopment.Fish
{
    public class RandomWeightedTable : MonoBehaviour
    {
        // TODO: Make a weighted table with a Fish List:
        // TODO: Change the following variables with RandomWeightedTable - Colour/Skin(Prefabs Fish), Trajectory, Spawn Area & FishSpawnedRotation.

        // TODO: The Weighted Table will have to use parameters that help define how it chooses a specific property, 
        // TODO: and have a minimum + maximum limit to properties, to avoid properties not being used at all.
        // E.g. In a list of 20 fish, make at minimum 4 red fish, 2 yellow, and 3 blue fish.
        // E.g. In a list of 20 fish, make at least 2 fish fly from below, 10 fish from high above etc.
        // E.g. In a list of 20 fish, each prefab of fish can only be spawned max 4 times.

        // TODO: Be able to adjust these parameters in Inspector for testing purposes!
        // TODO: Being able to see in Inspector what properties are given for each fish in this list would be needed as well!

        // TODO: The RandomWeightedTable will make a virtual list(?) where it preemptively applies the properties,
        // TODO: and this list can be picked up by the SpawnManager to help determine spawnedFish properties.
    }

    public class TypesOfFish
        {
            public readonly string Name;
            public readonly int Weight;

            public TypesOfFish(string fishName, int fishWeight)
            {
                Name = fishName;
                Weight = fishWeight;
            }
        }

    class Program
    {
        // public static Random _random = new Random();

        public static TypesOfFish GetFish(List<TypesOfFish> fishies, int totalWeightFishies)
        {
            // totalWeightFishies is the sum of all fishies' weight

            var randomNumber = Random.Range(0, totalWeightFishies);

            TypesOfFish selectedTypesOfFish = null;
            foreach (TypesOfFish fish in fishies)
            {
                if (randomNumber < fish.Weight)
                {
                    selectedTypesOfFish = fish;
                    break;
                }

                randomNumber -= fish.Weight;
            }

            return selectedTypesOfFish;
        }


        static void Main()
        {
            List<TypesOfFish> fishies = new List<TypesOfFish>
            {
                new TypesOfFish("A", 10),
                new TypesOfFish("B", 20),
                new TypesOfFish("C", 20),
                new TypesOfFish("D", 10)
            };

            // total the weight
            int totalWeight = 0;
            foreach (TypesOfFish fish in fishies)
            {
                totalWeight += fish.Weight;
            }

            while (true)
            {
                Dictionary<string, int> result = new Dictionary<string, int>();

                for (int i = 0; i < 1000; i++)
                {
                    var selectedTypesOfFish = GetFish(fishies, totalWeight);
                    if (selectedTypesOfFish != null)
                    {
                        if (result.ContainsKey(selectedTypesOfFish.Name))
                        {
                            result[selectedTypesOfFish.Name] += 1;
                        }
                        else
                        {
                            result.Add(selectedTypesOfFish.Name, 1);
                        }
                    }
                }
                Console.WriteLine("A\t\t" + result["A"]);
                Console.WriteLine("B\t\t" + result["B"]);
                Console.WriteLine("C\t\t" + result["C"]);
                Console.WriteLine("D\t\t" + result["D"]);

                result.Clear();
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}
