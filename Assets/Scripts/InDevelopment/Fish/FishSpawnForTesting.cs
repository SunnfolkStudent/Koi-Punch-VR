using UnityEngine;
using UnityEngine.InputSystem;

namespace InDevelopment.Fish
{
    public class FishSpawnForTesting : MonoBehaviour
    {
        // TODO: Remove this script, it is only for testing
        [SerializeField] private GameObject testingFish;
        [SerializeField] private Transform areaToSpawn;

        // Update is called once per frame
        void Update()
        {
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
