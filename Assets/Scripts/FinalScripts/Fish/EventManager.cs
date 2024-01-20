using System;
using UnityEngine;

namespace FinalScripts.Fish
{
    public static class EventManager
    {
        #region >>>---FishSpawningActions---
        public static Action SpawnFish; // Spawns 1 fish and launches it to hit the player.
        public static Action FishSpawning = SpawnFishDebug; // Starts fish spawning with frequency gradually increasing.
        public static Action FishSpawningAtMaxRate = SpawnFishDebug; // Starts fish spawning at max rate.
        public static Action StopFishSpawning = StopFishSpawningDebug; // Stops fish spawning.
        
        #region >>>---ActionDebugs---
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
        #endregion
        #endregion
        
        #region >>>---StartBossPhasesActions---
        public static Action SpawnBoss;
        public static Action StartBossPhase0 = StartBossPhase0Debug; // Starts phase 0.
        public static Action BossPhase0Completed; // Stops time and starts next uncompleted phase 
        public static Action BossPhaseSuccessful;
        public static Action StartBossPhase1 = StartBossPhase1Debug; // Starts phase 1.
        public static Action StartBossPhase2 = StartBossPhase2Debug; // Starts phase 2.
        public static Action StartBossPhase3 = StartBossPhase3Debug; // Starts phase 3.
        public static Action BossDefeated = BossDefeatedDebug;
        
        #region >>>---ActionDebugs---
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
        #endregion
        
        #region ---ScoreActions---
        public static Action<float> GainScore; // Points gained call this eks: fish hit ground and end of bossBattle
        public static Action<float> ScoreChanged; // Invoked with the current boss score
        public static Action<float> BossDefeatedTotalScore; // Invoked at the end of the boss fight
        #endregion
    }
}
