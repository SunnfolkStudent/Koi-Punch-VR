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
        // TODO: Fix why spawned fish have torque on spawning
        // TODO: Spawn fish with properties; Size, Colour, type, flight trajectory type and area spawned in decided by random weighted tables
        [SerializeField] private Transform player;
        private static List<SpawnArea> _spawnAreas;
        
        [Header("Trajectory Height")]
        [SerializeField] private float minHeight = 5f;
        [SerializeField] private float maxHeight = 10f;
        
        [Header("Fish size")]
        [SerializeField] private float minSize = 1f;
        [SerializeField] private float maxSize = 2f;
        
        #region ---Initialization---
        private void Start()
        {
            _spawnAreas = new List<SpawnArea>();
            AddSpawnAreasToSpawnAreaList();
            
            EventManager.SpawnFish += SpawnRandomFish; //TODO: Remove temporary spawning
        }
        #endregion
        
        #region ---SpawnAreas---
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
                EventManager.SpawnFish.Invoke();
            }
            
            if (Keyboard.current.jKey.isPressed)
            {
                EventManager.SpawnFish.Invoke();
            }
            
            if (Keyboard.current.hKey.isPressed)
            {
                EventManager.SpawnFish.Invoke();
            }
        }
        #endregion
        
        #region ---FishSpawning---
        private void SpawnFishAtPosFromPool(Vector3 spawnPos, FishObjectPool.FishPool fishPool)
        {
            var fish = FishObjectPool.GetPooledObject(fishPool);
            var rigidities = fish.Children.Where(child => child.Rigidbody != null).Select(child => child.Rigidbody).ToArray();
            var fishTransform = fish.ParentGameObject.transform;
            var targetPos = player.position;
            var height = Random.Range(minHeight, maxHeight);
            
            fishTransform.position = spawnPos;
            fishTransform.localScale = Vector3.one * Random.Range(minSize, maxSize);
            fishTransform.LookAt(targetPos, Vector3.up);
            
            // ---Launch Fish With Max Height--- \\
            FishTrajectory.LaunchObjectAtTargetWithPeakHeight(rigidities, fishTransform.position, targetPos, height);
            
            // ---Launch Fish At Angle--- \\
            // var angle =  Random.Range(20f, 60f);
            // FishTrajectory.LaunchObjectAtTargetWithInitialAngle(rigidities, fishTransform.position, player.position, angle);
            
            // ---Launch Fish With Speed--- \\
            // var speed = Random.Range(27f, 40f);
            // var tallArc = Random.Range(0, 2) == 1;
            // FishTrajectory.LaunchObjectAtTargetWithInitialSpeed(rigidities, fishTransform.position, player.position, speed, tallArc);
            
            fish.ParentGameObject.SetActive(true);
        }
        #endregion
    }
}