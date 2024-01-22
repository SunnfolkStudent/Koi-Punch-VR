using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FinalScripts.Fish.Spawning;
using FinalScripts.Fish.Spawning.RandomWeightedTables;
using UnityEngine;

namespace FinalScripts.Fish.BossBattle
{
    public class Boss : MonoBehaviour, IPunchable
    {
        private FishSpawnAreas _fishSpawnAreas;
        private Transform _player;
        private Rigidbody _rigidbody;
        private Rigidbody[] _rigidities;
        private static BossPhase _currentBossState;
        private bool _bossIsDead;
        
        #region ---InspectorSettings---
        [Header("Delay")]
        [SerializeField] private float phase0Delay = 5f;
        [SerializeField] private float attackDelay = 5f;
        
        [Header("BossLaunch")]
        [SerializeField] private float height;
        
        [Header("ZenGained")]
        [SerializeField] private int zenPerHitPhase2 = 5;
        
        [Header("Score")]
        public static int Score;
        [SerializeField] private int scorePerHitPhase2 = 30;
        
        [Header("ZenPunch")]
        [SerializeField] private float zenPunchMultiplier = 100f;
        
        [Header("debug")]
        [SerializeField] private bool isDebugging;
        #endregion
        
        #region ---Debugging---
        private void Log(string message)
        {
            if(isDebugging) Debug.Log(message);
        }
        #endregion
        
        #region ---Initialization---
        private void Awake()
        {
            _fishSpawnAreas = GameObject.FindGameObjectWithTag("FishSpawnManager").GetComponent<FishSpawnAreas>();
            EventManager.StartBossPhase0 += Phase0;
            EventManager.BossPhase0Completed += Phase0Completed;
            EventManager.BossPhaseSuccessful += PhaseSuccessful;
            EventManager.StartBossPhase1 += Phase1;
            EventManager.StartBossPhase2 += Phase2;
            EventManager.StartBossPhase3 += Phase3;
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("MainCamera").transform;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidities = GetComponentsInChildren<Rigidbody>().ToArray();
            StartCoroutine(OnSpawn());
        }

        #endregion
        
        #region ---BossPhases---
        #region >>>---PhaseProperties---
        private enum BossPhase
        {
            Phase1,
            Phase2,
            Phase3,
            BossDefeated,
            Phase0
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
            public readonly Action Event;

            public PhaseInfo(Action @event)
            {
                score = 0;
                Event = @event;
            }
        }
        #endregion
        
        #region >>>---BossPhasesStart---
        private IEnumerator OnSpawn()
        {
            FMODManager.instance.zenMusic.setParameterByName("zenLevel", 0);
            FMODManager.instance.zenMusic.start();
            yield return new WaitForSeconds(phase0Delay);
            EventManager.StartBossPhase0.Invoke();
        }
        
        private void Phase0()
        {
            _currentBossState = BossPhase.Phase0;
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/BossComments/BossSpawn");
            StartCoroutine(AttackWithDelay());
        }
        
        private IEnumerator AttackWithDelay()
        {
            yield return new WaitForSeconds(attackDelay);
            Attack();
        }
        
        private void Attack()
        {
            var spawnPos = _fishSpawnAreas.GetNextFishSpawnPosition();
            transform.position = spawnPos;
            var playerPosition = _player.position;
            transform.LookAt(playerPosition, Vector3.up);
            var velocity2D = FishTrajectory.TrajectoryVelocity2DFromPeakHeight(spawnPos, playerPosition, height);
            FishSpawnManager.LaunchRigiditiesDirectionWithVelocityTowards(_rigidities, (playerPosition - spawnPos).normalized, velocity2D);
        }
        
        private void Phase1()
        {
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/BossComments/PunchTheWeakpoints");
            Score = 0;
            _currentBossState = BossPhase.Phase1;
        }
        
        private void Phase2()
        {
            FMODManager.instance.zenMusic.setParameterByName("zenLevel", 1);
            Score = 0;
            _currentBossState = BossPhase.Phase2;
        }
        
        private void Phase3()
        {
            FMODManager.instance.zenMusic.setParameterByName("zenLevel", 2);
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/BossComments/BossPhase3");
            Score = 0;
            _currentBossState = BossPhase.Phase3;
        }
        #endregion
        
        #region >>>---PhaseCompletion---
        private void Phase0Completed()
        {
            var (key, value) = Phase.FirstOrDefault(keyValuePair => keyValuePair.Value.score == 0);
            Log($"Phase 0 completed go to: {key}");
            value.Event.Invoke();
        }
        
        private void PhaseSuccessful()
        {
            Phase[_currentBossState].score = Score;
            Log($"boss phase: {_currentBossState} completed | invoke: {_currentBossState++}");
            Phase[_currentBossState++].Event.Invoke();
        }
        #endregion
        
        #region >>>---PunchBoss--
        public void PunchObject(ControllerManager controllerManager, string fistUsed)
        {
            switch(_currentBossState){
                case BossPhase.Phase0:
                    Phase0Hit();
                    break;
                case BossPhase.Phase1:
                    Phase1Hit();
                    break;
                case BossPhase.Phase2:
                    Phase2Hit();
                    break;
                case BossPhase.Phase3:
                    Phase3Hit(controllerManager, fistUsed);
                    break;
                case BossPhase.BossDefeated:
                default:
                    Log("Boss not in valid Phase");
                    break;
            }
        }
        
        [ContextMenu("HitBossPhase0")]
        private void Phase0Hit()
        {
            Log("Phase 0 hit");
            // TODO: Play FishScaleVFX
            EventManager.BossPhase0Completed.Invoke();
        }

        [ContextMenu("HitBossPhase1")]
        private void Phase1Hit()
        {
            Log("Can't hit boss directly in phase 1");
        }

        [ContextMenu("HitBossPhase2")]
        private void Phase2Hit()
        {
            Log("Phase 2 hit");
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/BossComments/BossPhase2");
            Score += scorePerHitPhase2;
            ZenMetreManager.Instance.AddHitZen(zenPerHitPhase2);
        }
        
        private void Phase3Hit(ControllerManager controllerManager, string fistUsed)
        {
            if (!SpecialAttackScript.punchCharged) return;
            Log("Phase 3 hit charged");
            
            var controllerVelocity = fistUsed == "LeftFist" ? controllerManager.leftControllerVelocity : controllerManager.rightControllerVelocity;
            var zenPunchForce = SpecialAttackScript.punchForce * controllerVelocity.normalized;
            var force = controllerVelocity + zenPunchForce;
            force *= zenPunchMultiplier;
            
            _bossIsDead = true;
            _rigidbody.AddForce(force);
            EventManager.BossDefeated.Invoke();
            
            var totalScore = (int)(force.magnitude + Phase.Sum(pair => pair.Value.score));
            Log($"BossDefeated | TotalScore: {totalScore}");
            EventManager.GainScore.Invoke(totalScore);

            StartCoroutine(PunchSound());
        }

        private static IEnumerator PunchSound()
        {
            FMODManager.instance.koiPunch.setParameterByName("koiPunchSoundState", 1);
            yield return new WaitForSeconds(4);
            FMODManager.instance.koiPunch.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        #endregion
        #endregion
        
        #region ---Collision---
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Ground"))
            {
                if (_bossIsDead)
                {
                    Destroy(gameObject);
                }
                else
                {
                    EventManager.StartBossPhase0.Invoke();
                }
            }
            else if (other.transform.CompareTag("MainCamera"))
            {
                EventManager.StartBossPhase0.Invoke();
            }
        }
        #endregion
    }
}