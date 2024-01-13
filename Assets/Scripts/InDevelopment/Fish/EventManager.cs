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
        public static Event StartBossPhase0;
        public static Event StartBossPhase1;
        public static Event StartBossPhase2;
        public static Event StartBossPhase3;
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
        
        private static void BossDefeatedDebug()
        {
            Debug.Log("---Boss Defeated---");
        }
        #endregion
    }
}
