using UnityEngine;

namespace InDevelopment.Fish
{
    public static class EventManager
    {
        #region ---Events---
        public delegate void Event();
        public static Event SpawnFish = SpawnFishDebug;
        public static Event StartLevel;
        public static Event LevelOver = LevelOverDebug;
        public static Event BossPhase0;
        public static Event BossPhase1;
        public static Event BossPhase2;
        public static Event BossPhase3;
        public static Event BossDefeated = BossDefeatedDebug;
        #endregion
        
        #region ---EventDebugs---
        private static void SpawnFishDebug()
        {
            Debug.Log("Fish Spawn; Event Invoked");
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
