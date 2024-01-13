using UnityEngine;

namespace InDevelopment.Fish
{
    public static class EventManager
    {
        #region ---Events---
        public delegate void Event();
        public static Event SpawnFish;
        public static Event FishSpawning = SpawnFishDebug;
        public static Event FishSpawningAtMaxRate = SpawnFishDebug;
        public static Event StopFishSpawning = LevelOverDebug;
        public static Event ZenBar1Full;
        public static Event BossPhase0;
        public static Event BossPhase1;
        public static Event BossPhase2;
        public static Event BossPhase3;
        public static Event ZenPunchReady;
        public static Event BossDefeated = BossDefeatedDebug;
        #endregion
        
        #region ---EventDebugs---
        private static void SpawnFishDebug()
        {
            Debug.Log("Fish Spawning; Event Invoked");
        }
        
        private static void LevelOverDebug()
        {
            Debug.Log("Level End; Event Invoked");
        }
        
        private static void BossDefeatedDebug()
        {
            Debug.Log("Boss Defeated; Event Invoked");
        }
        #endregion
    }
}
