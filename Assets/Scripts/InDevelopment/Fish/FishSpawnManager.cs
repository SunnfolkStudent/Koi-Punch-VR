using System;
using System.Collections.Generic;
using System.Linq;
using InDevelopment.Fish.Trajectory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InDevelopment.Fish
{
    public class FishSpawnManager : MonoBehaviour
    {
        // TODO: Fix why spawned fish have torque
        // TODO: Spawn fish with properties: type and flight trajectory type, decided by random weighted tables
        
        #region ---FishLaunchInspectorSettings---
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
        
        private enum LaunchType
        {
            Height,
            Angle,
            Speed
        }
        #endregion
        
        #region ---Initialization---
        private void Awake()
        {
            EventManager.SpawnFish += SpawnFish;
            FishSpawnAreas.InitializeSpawnAreas();
        }
        #endregion
        
        #region ---FishSpawning---
        private void SpawnFish()
        {
            var spawnPos = FishSpawnAreas.GetSpawnPosition();
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