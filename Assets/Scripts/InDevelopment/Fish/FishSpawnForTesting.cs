using UnityEngine;
using UnityEngine.InputSystem;

namespace InDevelopment.Fish
{
    public class FishSpawnForTesting : MonoBehaviour
    {
        // TODO: Remove this script, it is only for testing
        [SerializeField] private GameObject testingFish;
        [SerializeField] private Transform areaToSpawn;
        
        [SerializeField] private InputActionReference leftTrigger;
        [SerializeField] private InputActionReference rightTrigger;

        // Update is called once per frame
        void Update()
        {
            
            
            if (leftTrigger.action.WasPressedThisFrame())
            {
                FishSpawnManager.SpawnFish.Invoke();
                // SpawnFishForTesting();
                // print("fish has spawned");
            }

            if (rightTrigger.action.WasPressedThisFrame())
            {
                Destroy(gameObject);
            }

            
            
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SpawnFishForTesting();
            }
        }

        void SpawnFishForTesting()
        {
            var fish = Instantiate(testingFish);
            fish.transform.position = areaToSpawn.position;
        }
    }
}
