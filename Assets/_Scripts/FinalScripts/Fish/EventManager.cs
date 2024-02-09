using System;
using UnityEngine;

namespace FinalScripts.Fish
{
    public static class EventManager
    {
        private const bool IsDebugging = true;

        #region ---Debugging---
        private static void Log(string message)
        {
            if(IsDebugging) Debug.Log(message);
        }
        #endregion
        
        #region >>>---FishSpawningActions---
        public static Action SpawnFish; // Spawns 1 fish and launches it to hit the player.
        public static Action FishSpawning = SpawnFishDebug; // Starts fish spawning with frequency gradually increasing.
        public static Action FishSpawningAtMaxRate = SpawnFishDebug; // Starts fish spawning at max rate.
        public static Action StopFishSpawning = StopFishSpawningDebug; // Stops fish spawning.
        
        #region >>>---ActionDebugs---
        private static void SpawnFishDebug()
        {
            Log("---Fish Spawning started---");
        }
        
        private static void StopFishSpawningDebug()
        {
            Log("---Fish Spawning stopped---");
        }
        
        private static void StartBossPhase0Debug()
        {
            Log("---BossPhase0 started---");
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
            Log("---BossPhase1 started---");
        }
        
        private static void StartBossPhase2Debug()
        {
            Log("---BossPhase2 started---");
        }
        
        private static void StartBossPhase3Debug()
        {
            Log("---BossPhase3 started---");
        }
        
        private static void BossDefeatedDebug()
        {
            Log("---Boss Defeated---");
        }
        #endregion
        #endregion
        
        #region ---ScoreActions---
        public static Action<float, bool> FishScore = FishScoreDebug; // Calculated by scoreManager Distance, SuccessFullPunch
        public static Action<int> BossScore = GainScoreDebug; // Points gained call this eks: fish hit ground and end of bossBattle
        public static Action<int> PenaltyScore = PenaltyScoreDebug; // Points lost call this (player hit by fish)
        public static Action<int> BonusScore = BonusScoreDebug; // Points Gained from objects (bird)
        
        #region >>>---ActionDebugs---
        private static void FishScoreDebug(float distance, bool successFullPunch)
        {
            Log($"---FishScore from distance: {distance}, SuccessfullPunch: {successFullPunch}---");
        }

        private static void GainScoreDebug(int score)
        {
            Log($"---Gained Score: {score}---");
        }

        private static void PenaltyScoreDebug(int score)
        {
            Log($"---Lost Score: {score}");
        }
        
        private static void BonusScoreDebug(int score)
        {
            Log($"---Bonus Score Gained: {score}");
        }
        #endregion
        #endregion
    }
}