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
        // TODO: Spawn Rotation
        // TODO: Spawn frequency increased over time
        // TODO: Spawn fish with properties; Size, Colour, type, flight trajectory type and area spawned in decided by random weighted tables
        [SerializeField] private Transform player;
        private static List<SpawnArea> _spawnAreas;
        [SerializeField] private float height = 10;
        
        [Header("Fish size")]
        [SerializeField] private float minSize = 0.1f;
        [SerializeField] private float maxSize = 0.2f;
        
        public delegate void Delegate();
        public static Delegate SpawnFish; 
        
        #region ---Initialization---
        private void Start()
        {
            _spawnAreas = new List<SpawnArea>();
            AddSpawnAreasToSpawnAreaList();
            
            SpawnFish += SpawnRandomFish; //TODO: Remove temporary spawning
        }
        #endregion
        
        #region ---SpawnArea---
        private class SpawnArea
        {
            public GameObject GameObject;
            public SpawnAreaCircle SpawnAreaCircle;
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
        
        private static SpawnArea RandomSpawnArea()
        {
            return _spawnAreas[Random.Range(0, _spawnAreas.Count)];
        }
        
        private static Vector3 RandomOffset(float offsetMax)
        {
            return new Vector3(Random.Range(-offsetMax, offsetMax), 0, Random.Range(-offsetMax, offsetMax));
        }
        #endregion
        
        #region ---TemporarySpawning---
        // private void Update()
        // {
        //     if (Keyboard.current.lKey.wasPressedThisFrame)
        //     {
        //         SpawnFish.Invoke();
        //         // SpawnRandomFish();
        //     }
        // }

        private void SpawnRandomFish()
        {
            var spawnArea = RandomSpawnArea();
            var offset = spawnArea.SpawnAreaCircle.spawnAreaRadius;
            SpawnFishAtPosFromPool(spawnArea.GameObject.transform.position + RandomOffset(offset), FishObjectPool.FishPools[0]);
        }
        
        private void FixedUpdate()
        {
            if (Keyboard.current.kKey.isPressed)
            {
                SpawnFish.Invoke();
            }
            
            if (Keyboard.current.jKey.isPressed)
            {
                SpawnFish.Invoke();
            }
            
            if (Keyboard.current.hKey.isPressed)
            {
                SpawnFish.Invoke();
            }
        }
        #endregion
        
        #region ---FishSpawning---
        private void SpawnFishAtPosFromPool(Vector3 spawnPos, FishObjectPool.FishPool fishPool)
        {
            var fish = FishObjectPool.GetPooledObject(fishPool);
            if (fish == null)
            {
                FishObjectPool.AddFishToPool(fishPool);
                SpawnFishAtPosFromPool(spawnPos, fishPool);
                return;
            }
            
            FishObjectPool.ResetPropertiesOfFishInPool(fish, fishPool);
            
            var rigidities = fish.Children.Where(child => child.Rigidbody != null).Select(child => child.Rigidbody).ToArray();
            
            fish.ParentGameObject.transform.position = spawnPos;
            fish.ParentGameObject.transform.localScale = Vector3.one * Random.Range(minSize, maxSize);
            
            var targetPos = player.position;
            RotateObjTowardsPos(fish.ParentGameObject.transform, targetPos);
            
            // ---Speed Known--- \\
            // var speed = Random.Range(27f, 40f);
            // var tallArc = Random.Range(0, 2) == 1;
            // FishTrajectory.LaunchObjectAtTargetWithInitialSpeed(rigidities, fishTransform.position, player.position, speed, tallArc);
            
            // ---Angle Known--- \\
            // var angle =  Random.Range(20f, 60f);
            // FishTrajectory.LaunchObjectAtTargetWithInitialAngle(rigidities, fishTransform.position, player.position, angle);
            
            // ---Max Height Known--- \\
            // var height = Random.Range(2f, 5f);
            FishTrajectory.LaunchObjectAtTargetWithPeakHeight(rigidities, fish.ParentGameObject.transform.position, targetPos, height);
            
            fish.ParentGameObject.SetActive(true);
        }
        
        private static void RotateObjTowardsPos(Transform objTransform, Vector3 target)
        {
            objTransform.LookAt(target, Vector3.up);
            objTransform.Rotate(new Vector3(0,-90,0));
        }
        #endregion
    }
}