using System.Collections.Generic;
using System.Linq;
using InDevelopment.Fish.EditorScripts;
using InDevelopment.Fish.Trajectory;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class FishSpawnManager : MonoBehaviour
    {
        // TODO: Fix why spawned fish have torque on spawning
        // TODO: Spawn fish with properties; Size, Colour, type, flight trajectory type decided by random weighted tables
        [SerializeField] private Transform player;
        
        [Header("Trajectory Height")]
        [SerializeField] private float minHeight = 5f;
        [SerializeField] private float maxHeight = 10f;
        
        [Header("Fish size")]
        [SerializeField] private float minSize = 1f;
        [SerializeField] private float maxSize = 2f;
        
        [Header("Weighted Tables")]
        [SerializeField] private int maxPickRate = 5;
        private static List<SpawnArea> _availableSpawnAreas;
        private static float _totalWeight;
        
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
            var targetPos = player.position;
            var height = Random.Range(minHeight, maxHeight);
            
            fishTransform.position = spawnPos;
            fishTransform.localScale = Vector3.one * Random.Range(minSize, maxSize);
            fishTransform.LookAt(targetPos, Vector3.up);
            
            /* Launch Fish With Max Height */
            // var fishVelocity = FishTrajectory.TrajectoryVelocityFromPeakHeight(spawnPos, targetPos, height);
            
            /* Launch Fish At Angle */
            // var angle =  Random.Range(20f, 60f);
            // var fishVelocity = FishTrajectory.TrajectoryVelocityFromInitialAngle(spawnPos, targetPos, angle);
            
            /* Launch Fish With Speed */
            var speed = Random.Range(27f, 40f);
            var tallArc = Random.Range(0, 2) == 1;
            var fishVelocity = FishTrajectory.TrajectoryVelocityFromInitialSpeed(spawnPos, targetPos, speed, tallArc);
            
            LaunchRigidities(rigidities, spawnPos, targetPos, fishVelocity);
            fish.ParentGameObject.SetActive(true);
        }
        
        private static void LaunchRigidities(IEnumerable<Rigidbody> objRigidbody, Vector3 objPos, Vector3 targetPos, Vector2 fishVelocity)
        {
            var targetDirection = (targetPos - objPos).normalized;
            var velocity = new Vector3(targetDirection.x * fishVelocity.x, fishVelocity.y, targetDirection.z * fishVelocity.x);
            foreach (var rigidbody in objRigidbody)
            {
                rigidbody.velocity = velocity;
            }
        }
        #endregion
    }
}