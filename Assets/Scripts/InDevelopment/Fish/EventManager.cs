using UnityEngine;

namespace InDevelopment.Fish
{
    public static class EventManager
    {
        public delegate void Event();
        public static Event SpawnFish = SpawnFishDebug;
        public static Event LevelOver = LevelOverDebug;
        public static Event SpawnBoss = SpawnBossDebug;
        public static Event BossDefeated = BossDefeatedDebug;

        #region ---EventDebugs---
        private static void SpawnFishDebug()
        {
            Debug.Log("Fish Spawn; Event Invoked");
        }
        
        private static void LevelOverDebug()
        {
            Debug.Log("Level End; Event Invoked");
        }
        
        private static void SpawnBossDebug()
        {
            Debug.Log("Spawn Boss; Event Invoked");
        }
        
        private static void BossDefeatedDebug()
        {
            Debug.Log("Boss Defeated; Event Invoked");
        }
        #endregion
    }
}
