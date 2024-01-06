using System.Collections.Generic;
using InDevelopment.Fish.Trajectory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InDevelopment.Fish
{
    public class FishSpawnManager : MonoBehaviour
    {
        #region ---TODO---
            // TODO: Spawn Frequency of fish, increased over time === Gradually increasing.
                // Use Animation Curves inside Unity in Inspector, and make the code read off of the graph there
                // to gradually increase fish spawn speed.

            // TODO: Spawn Area (with offset) a.k.a. Spawn Position & Fish Rotation === Random Weighted.
                // Use a "weightTable" to help track where former/recent fish have spawned, and avoid the most recently used spawns.
                // Based on input from weightTable, choose spawn area, and then offset position, and randomly weighted rotation on fish being sent.
            
            // TODO: Spawned Fish Properties; Size&Mass + Colour/Skin. === Random Weighted.
                // Use a "weightTable" to help track what former/recent fish properties have been, and avoid the most recently used properties.
                // Based on input from weightTable, choose size&mass, and colour/skin. 
        
        // TODO: Object Pooling - When receiving input from a fish object about colliding, turn same fish Inactive/Despawn it with a function from this FishSpawnManager.
        // TODO: When fish are set to Inactive, fish variables will be reset.
        #endregion
        
        [SerializeField] private Transform player;
        private static List<SpawnArea> _spawnAreas;
        private class SpawnArea
        {
            public GameObject GameObject;
            public SpawnAreaCircle SpawnAreaScript;
        }
        
        private void Awake()
        {
            _spawnAreas = new List<SpawnArea>();
        }
        
        private void Start()
        {
            AddSpawnAreasToSpawnAreaList();
        }

        private static void AddSpawnAreasToSpawnAreaList()
        {
            var spawnArea = GameObject.FindGameObjectsWithTag("SpawnArea");

            foreach (var obj in spawnArea)
            {
                var f = new SpawnArea { GameObject = obj };
                f.SpawnAreaScript = f.GameObject.GetComponent<SpawnAreaCircle>();
                _spawnAreas.Add(f);
            }
        }

        private void Update()
        {
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                var spawnArea = RandomSpawnArea();
                var offset = spawnArea.SpawnAreaScript.spawnAreaRadius;
                SpawnFish(spawnArea.GameObject.transform.position + RandomOffset(offset));
            }
        }

        private static SpawnArea RandomSpawnArea()
        {
            return _spawnAreas[Random.Range(0, _spawnAreas.Count)];
        }

        private static Vector3 RandomOffset(float offsetMax)
        {
            return new Vector3(Random.Range(-offsetMax, offsetMax), 0, Random.Range(-offsetMax, offsetMax));
        }
        
        private void SpawnFish(Vector3 spawnPos)
        {
            var fish = FishObjectPool.SharedInstance.GetPooledObject();
            if (fish == null) return;
            
            fish.GameObject.transform.position = spawnPos;
            var speed = Random.Range(15, 100);
            
            FishTrajectory.LaunchObjectAtTargetWithInitialSpeed(fish.Rigidbody, fish.GameObject.transform.position, player.position, speed);
            fish.GameObject.SetActive(true);
        }

        public static void DespawnFish(GameObject fish)
        {
            fish.SetActive(false);
        }
    }
}