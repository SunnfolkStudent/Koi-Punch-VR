using System.Collections.Generic;
using System.Linq;
using InDevelopment.Fish.RandomWeightedTables;
using InDevelopment.Fish.Trajectory;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class Boss : MonoBehaviour, IPunchable
    {
        [SerializeField] private Transform player;
        [SerializeField] private BossPhase bossState;

        private void Start()
        {
            EventManager.StartBossPhase0 += Phase0;
            EventManager.BossPhase0Completed += Phase0Completed;
            EventManager.StartBossPhase1 += Phase1;
            EventManager.StartBossPhase2 += Phase2;
            EventManager.StartBossPhase3 += Phase3;
        }

        #region ---PhaseProperties---
        private enum BossPhase
        {
            Phase0,
            Phase1,
            Phase2,
            Phase3
        }
        
        private static readonly Dictionary<BossPhase, PhaseInfo> Phase = new()
        {
            { BossPhase.Phase1, new PhaseInfo(() => EventManager.StartBossPhase1.Invoke()) },
            { BossPhase.Phase2, new PhaseInfo(() => EventManager.StartBossPhase2.Invoke()) },
            { BossPhase.Phase3, new PhaseInfo(() => EventManager.StartBossPhase3.Invoke()) }
        };
        
        private class PhaseInfo
        {
            public float score { get; set; }
            public readonly EventManager.Event Event;

            public PhaseInfo(EventManager.Event @event)
            {
                score = 0;
                Event = @event;
            }
        }
        #endregion

        #region ---BossPhases---
        private void Phase0()
        {
            var spawnPos = FishSpawnAreas.GetNextFishSpawnPosition();
            var playerPosition = player.position;
            var rigidities = GetComponentsInChildren<Rigidbody>();
            var velocity2D = FishTrajectory.TrajectoryVelocity2DFromInitialSpeed(spawnPos, playerPosition, 35f);
            FishSpawnManager.LaunchRigiditiesDirectionWithVelocity(rigidities, playerPosition, velocity2D);
        }

        private static void Phase1()
        {
            // TODO: spawn weak-points
        }

        private static void Phase2()
        {
            // TODO: count hits
        }

        private static void Phase2Hit()
        {
            // var scorePerHit = 5;
            // _score += scorePerHit;
        }

        private static void Phase3()
        {
            throw new System.NotImplementedException();
        }
        #endregion
        
        #region ---PhaseCompletion---
        private static void Phase0Completed()
        {
            ZenMetreManager.Instance.TimeStop();
            Phase.FirstOrDefault(keyValuePair => keyValuePair.Value.score == 0).Value.Event.Invoke();
        }

        private static void BossPhaseCompleted(BossPhase bossPhase, float score)
        {
            Phase[bossPhase - 1].score = score;
            Phase[bossPhase].Event.Invoke();
        }
        #endregion

        public void PunchObject(ControllerManager controllerManager, string fistUsed)
        {
            switch(bossState){
                case BossPhase.Phase0:
                    EventManager.BossPhase0Completed.Invoke();
                    break;
                case BossPhase.Phase2:
                    Phase2Hit();
                    break;
                case BossPhase.Phase3:
                    // TODO: make boss fly away when hit by KOI-PUNCH
                    break;
                default:
                    Debug.Log("Can't hit the boss in this Phase");
                    break;
            }
        }
    }
}