using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace InDevelopment.Fish
{
    // This script will be set on an outside gameObject, and aids in spawning the Fish.
    public class FishSpawnManager : MonoBehaviour
    {
        // TODO: Fetching / Working together with FishObjectPool Script.
            // TODO: When fish are set to Inactive, fish variables will be reset.

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
            
        // TODO: FishSpawnManager will use the static functions created by FishTrajectory, to help launch the fish initially.
        
        // TODO: Object Pooling - When receiving input from a fish object about colliding,
        // TODO: Turn same fish Inactive/Despawn it with a function from this FishSpawnManager.

        public GameObject spawnArea;
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        // TODO: Use the below code in a "SpawnFish" void, for later usage with FishObjectPool Script.
        
        private void SpawnFish()
        {
            GameObject fish = FishObjectPool.SharedInstance.GetPooledObject();

            if (fish != null)
            {
                fish.transform.position = spawnArea.transform.position;
                fish.transform.rotation = spawnArea.transform.rotation;
                fish.SetActive(true);
            }
        }
        
        
        
        
    }
}
