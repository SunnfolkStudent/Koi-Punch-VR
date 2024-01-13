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
        
        //This event is invoked when the first zen bar is full and the boss should spawn when hit time will stop
        public static Event ZenBar1Full; 
        
        //This event is invoked only if you fail one of the boss phases and go back to phase zero or when you hit the boss with your final move
        public static Event BossPhase0; 
        
        //This event is invoked when the boss is hit with the first move and time is stopped.
        //This is the phase where weak points will spawn
        public static Event BossPhase1;
        
        //This event is invoked when you clear phase one.
        //Weak points will stop spawning and it is time for the triple score punch out where you punch the boss as much as possible
        public static Event BossPhase2;
        
        //This event is invoked when you clear all both stages and the special attack is ready.
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
