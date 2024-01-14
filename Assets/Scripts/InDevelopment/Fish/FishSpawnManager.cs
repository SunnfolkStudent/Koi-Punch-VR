using System;
using System.Collections.Generic;
using System.Linq;
using InDevelopment.Fish.EditorScripts;
using InDevelopment.Fish.Trajectory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InDevelopment.Fish
{
    public class FishSpawnManager : MonoBehaviour
    {
        // TODO: Fix why spawned fish have torque
        // TODO: Spawn fish with properties: type and flight trajectory type, decided by random weighted tables

        #region ---FishLaunchSettings---
        [Header("Fish Target")] [Tooltip("Transform that the fish trajectories will aim to")]
        [SerializeField] private Transform target;
        
        [Header("Launch Type")] [Tooltip("Type chosen to calculate trajectory velocity")]
        [SerializeField] private LaunchType launchType;
        
        [Header("Trajectory Height")] [Tooltip("Chosen when launch type is set to Height")]
        [SerializeField] private float minHeight = 5f;
        [SerializeField] private float maxHeight = 10f;
        
        [Header("Trajectory Speed")] [Tooltip("Chosen when launch type is set to Speed")]
        [SerializeField] private float minSpeed = 20f;
        [SerializeField] private float maxSpeed = 40f;
        
        [Header("Trajectory Angle")] [Tooltip("Chosen when launch type is set to Angle")]
        [SerializeField] private float minAngle = 15f;
        [SerializeField] private float maxAngle = 65f;
        
        [Header("Fish size")] [Tooltip("Fish localScale for spawned fish")]
        [SerializeField] private float minSize = 1f;
        [SerializeField] private float maxSize = 2f;
        
        [Header("Weighted Tables")]
        [SerializeField] private int maxPickRate = 5;
        private static List<SpawnArea> _availableSpawnAreas;
        private static float _totalWeight;

        private enum LaunchType
        {
            Height,
            Angle,
            Speed
        }
        #endregion
        
        #region ---Initialization---
        private void Start()
        {
            _availableSpawnAreas = new List<SpawnArea>();
            AddSpawnAreasToSpawnAreaList();
            
            EventManager.SpawnFish += SpawnFish; //TODO: Remove temporary spawning
        }
        #endregion
        
        #region ---SpawnAreas---
        private class SpawnArea
        {
            public GameObject GameObject;
            public SpawnAreaCircle SpawnAreaCircle;
            public float Weight;
            public int TimesSpawned;
        }
        
        private static void AddSpawnAreasToSpawnAreaList()
        {
            var spawnArea = GameObject.FindGameObjectsWithTag("SpawnArea");
            
            foreach (var obj in spawnArea)
            {
                var f = new SpawnArea { GameObject = obj };
                f.SpawnAreaCircle = f.GameObject.GetComponent<SpawnAreaCircle>();
                _availableSpawnAreas.Add(f);
            }
        }

        #region >>>---Weighted Tables---
        private SpawnArea PickSpawnArea()
        {
            _totalWeight = _availableSpawnAreas.Sum(area => area.Weight);
            var rnd = Random.Range(0, _totalWeight);
            
            float sum = 0;
            foreach (var area in _availableSpawnAreas)
            {
                sum += area.Weight;
                if (sum < rnd) continue;
                NewProbabilitiesFor(area);
                return area;
            }
            
            return null;
        }
        
        private void NewProbabilitiesFor(SpawnArea spawnArea)
        {
            spawnArea.TimesSpawned++;
            spawnArea.Weight /= 2;

            if (spawnArea.TimesSpawned >= maxPickRate)
            {
                _availableSpawnAreas.Remove(spawnArea);
            }
        }
        #endregion
        
        // private static SpawnArea RandomSpawnArea()
        // {
        //     return _availableSpawnAreas[Random.Range(0, _availableSpawnAreas.Count)];
        // }
        
        private static Vector3 RandomOffset(float offsetMax)
        {
            return new Vector3(Random.Range(-offsetMax, offsetMax), 0, Random.Range(-offsetMax, offsetMax));
        }
        #endregion
        
        #region ---FishSpawning---
        private void SpawnFish()
        {
            var spawnArea = PickSpawnArea();
            var spawnPos = spawnArea.GameObject.transform.position + RandomOffset(spawnArea.SpawnAreaCircle.spawnAreaRadius);
            var fishPool = FishObjectPool.FishPools[0];
            SpawnFishAtPosFromPool(spawnPos, fishPool);
        }
        
        private void SpawnFishAtPosFromPool(Vector3 spawnPos, FishObjectPool.FishPool fishPool)
        {
            var fish = FishObjectPool.GetPooledObject(fishPool);
            var rigidities = fish.Children.Where(child => child.Rigidbody != null).Select(child => child.Rigidbody).ToArray();
            var fishTransform = fish.ParentGameObject.transform;
            var targetPos = target.position;
            var targetDirection = (targetPos - spawnPos).normalized;
            
            fishTransform.position = spawnPos;
            fishTransform.localScale = Vector3.one * Random.Range(minSize, maxSize);
            fishTransform.LookAt(targetPos, Vector3.up);
            
            Vector2 fishVelocity2D;
            switch (launchType)
            {
                case LaunchType.Height:
                    var height = Random.Range(minHeight, maxHeight);
                    fishVelocity2D = FishTrajectory.TrajectoryVelocity2DFromPeakHeight(fishTransform.position, targetPos, height);
                    break;
                case LaunchType.Angle:
                    var angle = Random.Range(minAngle, maxAngle);
                    fishVelocity2D = FishTrajectory.TrajectoryVelocity2DFromInitialAngle(fishTransform.position, targetPos, angle);
                    break;
                case LaunchType.Speed:
                    var speed = Random.Range(minSpeed, maxSpeed);
                    fishVelocity2D = FishTrajectory.TrajectoryVelocity2DFromInitialSpeed(fishTransform.position, targetPos, speed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            LaunchRigiditiesDirectionWithVelocity(rigidities, targetDirection, fishVelocity2D);
            fish.ParentGameObject.SetActive(true);
        }
        
        private static void LaunchRigiditiesDirectionWithVelocity(IEnumerable<Rigidbody> objRigidbody, Vector3 targetDirection, Vector2 fishVelocity)
        {
            var velocity = new Vector3(targetDirection.x * fishVelocity.x, fishVelocity.y, targetDirection.z * fishVelocity.x);
            foreach (var rigidbody in objRigidbody)
            {
                rigidbody.velocity = velocity;
            }
        }
        #endregion
    }
}