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
        [SerializeField] private float zenPerHitPhase2 = 5f;
        private static BossPhase _currentBossState;
        private float _score;
        
        #region ---Initialization---
        private void Start()
        {
            EventManager.StartBossPhase0 += Phase0;
            EventManager.BossPhase0Completed += Phase0Completed;
            EventManager.BossPhaseSuccessful += PhaseSuccessful;
            EventManager.StartBossPhase1 += Phase1;
            EventManager.StartBossPhase2 += Phase2;
            EventManager.StartBossPhase3 += Phase3;
        }
        #endregion
        
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
        
        #region ---BossPhasesStart---
        private void Phase0()
        {
            _currentBossState = BossPhase.Phase0;
            
            var spawnPos = FishSpawnAreas.GetNextFishSpawnPosition();
            var playerPosition = player.position;
            var rigidities = GetComponentsInChildren<Rigidbody>();
            var velocity2D = FishTrajectory.TrajectoryVelocity2DFromInitialSpeed(spawnPos, playerPosition, 35f);
            FishSpawnManager.LaunchRigiditiesDirectionWithVelocity(rigidities, playerPosition, velocity2D);
        }
        
        private static void Phase1()
        {
            _currentBossState = BossPhase.Phase1;
        }
        
        private static void Phase2()
        {
            _currentBossState = BossPhase.Phase2;
        }
        
        private static void Phase3()
        {
            _currentBossState = BossPhase.Phase3;
        }
        #endregion
        
        #region ---PhaseCompletion---
        private static void Phase0Completed()
        {
            Phase.FirstOrDefault(keyValuePair => keyValuePair.Value.score == 0).Value.Event.Invoke();
        }
        
        private static void PhaseSuccessful()
        {
            Phase[_currentBossState].score = ZenMetreManager.Instance.zenMetreValue;
            Phase[_currentBossState++].Event.Invoke();
        }
        #endregion
        
        #region ---PunchBoss--
        public void PunchObject(ControllerManager controllerManager, string fistUsed)
        {
            switch(_currentBossState){
                case BossPhase.Phase0:
                    EventManager.BossPhase0Completed.Invoke();
                    break;
                case BossPhase.Phase1:
                    Debug.Log("Can't hit the boss directly in Phase1");
                    break;
                case BossPhase.Phase2:
                    Phase2Hit();
                    break;
                case BossPhase.Phase3:
                    Phase3Hit(controllerManager, fistUsed);
                    break;
                default:
                    Debug.Log("Boss not in valid Phase");
                    break;
            }
        }
        
        private void Phase2Hit()
        {
            ZenMetreManager.Instance.AddHitZen(zenPerHitPhase2);
        }
        
        private void Phase3Hit(ControllerManager controllerManager, string fistUsed)
        {
            if (!SpecialAttackScript.punchCharged) return;
            
            var controllerVelocity = fistUsed == "LeftFist" ? controllerManager.leftControllerVelocity : controllerManager.rightControllerVelocity;
            var zenPunchForce = SpecialAttackScript.punchForce * controllerVelocity.normalized;
            var force = controllerVelocity + zenPunchForce;
            
            GetComponent<Rigidbody>().AddForce(force);
            
            var totalScore = Phase.Sum(pair => pair.Value.score);
            // TODO: ScoreManager.score += totalScore
        }
        #endregion
    }
}