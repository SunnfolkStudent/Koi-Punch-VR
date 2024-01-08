using System.Collections.Generic;
using System.Linq;
using InDevelopment.Fish.Trajectory;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
            public SpawnAreaCircle SpawnAreaCircle;
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
                f.SpawnAreaCircle = f.GameObject.GetComponent<SpawnAreaCircle>();
                _spawnAreas.Add(f);
            }
        }

        private void Update()
        {
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                var spawnArea = RandomSpawnArea();
                var offset = spawnArea.SpawnAreaCircle.spawnAreaRadius;
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

            var children = fish.Children;
            for (var i = 0; i < children.Length; i++)
            {
                children[i].Transform.position = children[i].InitialPosition;
                children[i].Transform.rotation = children[i].InitialRotation;
            }
            var rigidities = fish.Children.Where(child => child.ChildRigidbody != null).Select(child => child.ChildRigidbody).ToArray();
            
            fish.ParentGameObject.transform.position = spawnPos;

            var playerPosition = player.position;
            RotateObjTowards(fish.ParentGameObject.transform, playerPosition);
            
            // ---Speed Known--- \\
            // var speed = Random.Range(27, 40);
            // var tallArc = Random.Range(0, 2) == 1;
            // FishTrajectory.LaunchObjectAtTargetWithInitialSpeed(fish.Rigidbody, fishTransform.position, player.position, speed, tallArc);
            
            // ---Angle Known--- \\
            // var angle =  Random.Range(20f, 60f);
            // FishTrajectory.LaunchObjectAtTargetWithInitialAngle(fish.Rigidbody, fishTransform.position, player.position, angle);

            // ---Max Height Known--- \\
            var height = Random.Range(3f, 60f);
            FishTrajectory.LaunchObjectAtTargetWithPeakHeight(rigidities, fish.ParentGameObject.transform.position, playerPosition, height);
            
            fish.ParentGameObject.SetActive(true);
        }
        
        private static void RotateObjTowards(Transform objTransform, Vector3 target)
        {
            var targetDir = objTransform.position - target;
            var angle = Vector3.Angle(targetDir, objTransform.forward);
            objTransform.rotation = new Quaternion(0, angle, 0, (float)Space.World);
        }

        public static void DespawnFish(GameObject fish)
        {
            fish.SetActive(false);
        }
    }
}