using UnityEngine;

namespace InDevelopment.Fish
{
    public static class EventManager
    {
        #region ---Events---
        public delegate void Event();
        
        #region >>>---FishSpawning---
        public static Event SpawnFish; // Spawns 1 fish and launches it to hit the player.
        public static Event FishSpawning = SpawnFishDebug; // Starts fish spawning with frequency gradually increasing.
        public static Event FishSpawningAtMaxRate = SpawnFishDebug; // Starts fish spawning at max rate.
        public static Event StopFishSpawning = StopFishSpawningDebug; // Stops fish spawning.
        #endregion
        
        #region >>>---StartBossPhases---
        public static Event StartBossPhase0 = StartBossPhase0Debug; // Starts phase 0.
        public static Event StartBossPhase1 = StartBossPhase1Debug; // Starts phase 1.
        public static Event StartBossPhase2 = StartBossPhase2Debug; // Starts phase 2.
        public static Event StartBossPhase3 = StartBossPhase3Debug; // Starts phase 3.
        public static Event BossDefeated = BossDefeatedDebug;
        #endregion
        #endregion
        
        #region ---EventDebugs---
        private static void SpawnFishDebug()
        {
            Debug.Log("---Fish Spawning started---");
        }
        
        private static void StopFishSpawningDebug()
        {
            Debug.Log("---Fish Spawning stopped---");
        }
        
        private static void StartBossPhase0Debug()
        {
            Debug.Log("---BossPhase0 started---");
        }
        
        private static void StartBossPhase1Debug()
        {
            Debug.Log("---BossPhase1 started---");
        }
        
        private static void StartBossPhase2Debug()
        {
            Debug.Log("---BossPhase2 started---");
        }
        
        private static void StartBossPhase3Debug()
        {
            Debug.Log("---BossPhase3 started---");
        }
        
        private static void BossDefeatedDebug()
        {
            Debug.Log("---Boss Defeated---");
        }
        #endregion
    }
}
