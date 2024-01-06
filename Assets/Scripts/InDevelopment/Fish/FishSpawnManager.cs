using System.Collections;
using System.Collections.Generic;
using InDevelopment.Fish.Trajectory;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace InDevelopment.Fish
{
    // This script will be set on an outside gameObject, and aids in spawning the Fish.
    public class FishSpawnManager : MonoBehaviour
    {
            // Variables the FishSpawnManager handles:
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
        
        public GameObject[] spawnArea;
        [SerializeField] private Transform player;
        
        // Start is called before the first frame update
        private void Start()
        {
            spawnArea = GameObject.FindGameObjectsWithTag("SpawnArea");
        }

        // Update is called once per frame
        private void Update()
        {
            // Testing code, remove the below keyboardInput when we have proper functions for spawning fish.
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                SpawnFish();
            }
        }
        
        // TODO: Use the below code in a "SpawnFish" void, for later usage with FishObjectPool Script.
        
        private void SpawnFish()
        {
            var fish = FishObjectPool.SharedInstance.GetPooledObject();
            if (fish == null) return;
            
            fish.FishGameObject.transform.position = spawnArea[0].transform.position;
            // fish.transform.rotation = spawnArea[0].transform.rotation;
            
            FishTrajectory.LaunchObjectAtTargetWithInitialSpeed(fish.Rigidbody, fish.FishGameObject.transform.position, player.position, 15);
            
            fish.FishGameObject.SetActive(true);
        }
    }
}
