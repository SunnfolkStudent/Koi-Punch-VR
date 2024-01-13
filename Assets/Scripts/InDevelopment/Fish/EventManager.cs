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
        
        //This event is invoked only if you fail one of the boss phases and go back to phase zero and if you reach full first zen bar
        public static Event BossPhase0; 
        
        //These events are invoked when this boss phase is reached
        public static Event BossPhase1;
        public static Event BossPhase2;
        public static Event BossPhase3;
        
        //This event is invoked after you charge your punch.
        //There will be a variable in SpecialAttackScript called punchForce that is a float between 0-300 and where 300 is the max.
        //This variable tells you how much the special attack was charged before the buttons were let go
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
