using System;
using System.Collections.Generic;
using System.Linq;
using FinalScripts.Fish.Spawning.RandomWeightedTables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FinalScripts.Fish.Spawning
{
    [RequireComponent(typeof(FishSpawnAreas))]
    public class FishSpawnManager : MonoBehaviour
    {
        // TODO: Fix why spawned fish have torque
        private FishSpawnAreas _fishSpawnAreas;
        private Transform _target;
        
        #region ---FishLaunchInspectorSettings---
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
        
        private enum LaunchType
        {
            Height,
            Angle,
            Speed
        }
        #endregion
        
        #region ---EventFunctions---
        private void Awake()
        {
            _target = GameObject.FindGameObjectWithTag("MainCamera").transform;
            _fishSpawnAreas = GetComponent<FishSpawnAreas>();
            EventManager.SpawnFish += SpawnFish;
        }
        
        private void OnDestroy()
        {
            EventManager.SpawnFish -= SpawnFish;
        }
        #endregion
        
        #region ---FishSpawning---
        private void SpawnFish()
        {
            var spawnPos = _fishSpawnAreas.GetNextFishSpawnPosition();
            var fish = FishSpawnType.GetNextFish();
            SpawnFishAtPosFromPool(spawnPos, fish);
        }
        
        private void SpawnFishAtPosFromPool(Vector3 spawnPos, FishObjectPool.Fish fish)
        {
            var rigidities = fish.Children.Where(child => child.Rigidbody != null).Select(child => child.Rigidbody).ToArray();
            var fishTransform = fish.ParentGameObject.transform;
            var targetPos = _target.position;
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

            LaunchRigiditiesDirectionWithVelocityTowards(rigidities, targetDirection, fishVelocity2D);
            fish.ParentGameObject.SetActive(true);
        }
        
        public static void LaunchRigiditiesDirectionWithVelocityTowards(IEnumerable<Rigidbody> objRigidbody, Vector3 targetDirection, Vector2 fishVelocity)
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