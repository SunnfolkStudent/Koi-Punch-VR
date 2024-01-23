using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FinalScripts.Fish.Spawning.RandomWeightedTables;
using UnityEngine;

namespace FinalScripts.Fish.BossBattle
{
    public class Boss : MonoBehaviour, IFish
    {
        private FishSpawnAreas _fishSpawnAreas;
        private Transform _player;
        private Animator _animator;
        private Rigidbody[] _rigidities;
        public static BossPhase CurrentBossState;
        private bool _canCollideWithGround;
        private bool _bossIsDead;
        private bool _hasSaidPhase0VoiceLine;
        private LayerMask _spawnAreaMask;
        
        #region ---InspectorSettings---
        [Header("Spawn in FieldOfView")]
        [SerializeField] private float areaSearchRadius = 15;
        [SerializeField] private float viewAngle = 92;
        [SerializeField] private int maxSpawnAreas = 10;
        
        [Header("Attack")]
        [SerializeField] private float speed = 15f;
        [SerializeField] private float bossMoveInterval = 0.01f;
        [SerializeField] private Vector3 spawnOffset = new(0, 2.25f, 0);
        
        [Header("Delay")]
        [SerializeField] private float phase0Delay = 5f;
        [SerializeField] private float attackDelay = 5f;
        
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
            _spawnAreaMask = LayerMask.NameToLayer("SpawnAreas");
            EventManager.StartBossPhase0 += Phase0;
            EventManager.BossPhase0Completed += Phase0Completed;
            EventManager.BossPhaseSuccessful += PhaseSuccessful;
            EventManager.StartBossPhase1 += Phase1;
            EventManager.StartBossPhase2 += Phase2;
            EventManager.StartBossPhase3 += Phase3;
        }

        private void Start()
        {
            _rigidities = GetComponentsInChildren<Rigidbody>();
            foreach (var rigidity in _rigidities)
            {
                var bossChild = rigidity.gameObject.AddComponent<BossChild>();
                bossChild.boss = this;
                rigidity.useGravity = false;
            }
            
            _player = GameObject.FindGameObjectWithTag("MainCamera").transform;
            _animator = GetComponent<Animator>();
            StartCoroutine(OnSpawn());
        }
        
        #endregion
        
        #region ---BossPhases---
        #region >>>---PhaseProperties---

        public enum BossPhase
        {
            Phase1,
            Phase2,
            Phase3,
            BossDefeated,
            Phase0
        }

        private static readonly Dictionary<BossPhase, PhaseInfo> Phase = new()
        {
            { BossPhase.Phase1, new PhaseInfo(() => EventManager.StartBossPhase2.Invoke()) },
            { BossPhase.Phase2, new PhaseInfo(() => EventManager.StartBossPhase2.Invoke()) },
            { BossPhase.Phase3, new PhaseInfo(() => EventManager.StartBossPhase3.Invoke()) }
        };

        public class PhaseInfo
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
            if (!_hasSaidPhase0VoiceLine)
            {
                FMODManager.instance.PlayOneShot("event:/SFX/FishSounds/DragonRyu", transform.position);
                _hasSaidPhase0VoiceLine = true;
            }
            yield return new WaitForSeconds(phase0Delay);
            EventManager.StartBossPhase0.Invoke();
        }
        
        private void Phase0()
        {
            CurrentBossState = BossPhase.Phase0;
            _canCollideWithGround = false;
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/BossComments/BossSpawn");
            StartCoroutine(AttackWithDelay());
        }
        
        private IEnumerator AttackWithDelay()
        {
            var spawnPos = GetSpawnAreaPos();
            transform.position = spawnPos + spawnOffset;
            _animator.enabled = false;

            foreach (var rigidity in _rigidities)
            {
                rigidity.useGravity = false;
                rigidity.velocity = Vector3.zero;
            }
            
            yield return new WaitForSeconds(attackDelay);
            StartCoroutine(MoveTowardsPlayer());
        }

        // TODO: fix why speed is build up upon respawning and position not actually set
        private IEnumerator MoveTowardsPlayer()
        {
            var targetPos = _player.position;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos, Vector3.up);

            StartCoroutine(Move());
            
            var inRange = false;
            while (!inRange || IsPlaying(_animator, "Jump"))
            {
                var transform1 = transform;
                if (Vector3.Distance(transform1.position, _player.position) < speed && !inRange)
                {
                    Log("Within range");
                    _animator.enabled = true;
                    inRange = true;
                }
                
                yield return new WaitForSeconds(bossMoveInterval);
            }
            
            _animator.enabled = false;
            _canCollideWithGround = true;
            foreach (var rigidity in _rigidities)
            {
                rigidity.useGravity = true;
            }
        }

        private IEnumerator Move()
        {
            while (true)
            {
                var transform1 = transform;
                transform1.position += transform1.forward * (speed * bossMoveInterval);
                yield return new WaitForSeconds(bossMoveInterval);
            }
        }

        private static bool IsPlaying(Animator anim, string stateName)
        {
            return anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                   anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
        }
        
        private Vector3 GetSpawnAreaPos()
        {
            var sourceTransform = _player.transform;
            var spawnArea = new Collider[maxSpawnAreas];
            var spawnAreasCount = Physics.OverlapSphereNonAlloc(sourceTransform.position, areaSearchRadius, spawnArea, _spawnAreaMask);
            
            for (var i = 0; i < spawnAreasCount; i++)
            {
                var spawnAreaPos = spawnArea[i].transform.position;
                var dirToSpawnArea = (spawnAreaPos - sourceTransform.position).normalized;
        
                if (Vector3.Angle(sourceTransform.forward, dirToSpawnArea) > viewAngle * 0.5f) continue;
                
                var distanceToSpawnArea = Vector3.Distance(sourceTransform.position, spawnAreaPos);
        
                if (!Physics.Raycast(sourceTransform.position, dirToSpawnArea, distanceToSpawnArea))
                {
                    return spawnAreaPos;
                }
            }
            
            Log("Couldn't find SpawnArea In FieldOfView | getting spawn pos from FishSpawnArea script");
            return _fishSpawnAreas.GetNextFishSpawnPosition();
        }
        
        private void Phase1()
        {
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/BossComments/PunchTheWeakpoints");
            Score = 0;
            CurrentBossState = BossPhase.Phase1;
        }
        
        private void Phase2()
        {
            FMODManager.instance.zenMusic.setParameterByName("zenLevel", 1);
            Score = 0;
            CurrentBossState = BossPhase.Phase2;
        }
        
        private void Phase3()
        {
            FMODManager.instance.zenMusic.setParameterByName("zenLevel", 2);
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/BossComments/BossPhase3");
            Score = 0;
            CurrentBossState = BossPhase.Phase3;
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
            Phase[CurrentBossState].score = Score;
            Log($"boss phase: {CurrentBossState} completed | invoke: {CurrentBossState++}");
            Phase[CurrentBossState++].Event.Invoke();
        }
        #endregion
        
        #region >>>---PhaseHits--
        [ContextMenu("HitBossPhase0")]
        private void Phase0Hit()
        {
            Log("Phase 0 hit");
            // TODO: Play FishScaleVFX
            _animator.enabled = false;
            StopCoroutine(Move());
            EventManager.BossPhase0Completed.Invoke();
        }

        [ContextMenu("HitBossPhase1")]
        private void Phase1Hit()
        {
            Log("Can't hit boss directly in phase 1");
        }

        [ContextMenu("HitBossPhase2")]
        private void Phase2Hit(string lFist)
        {
            Log("Phase 2 hit");
            FMODManager.instance.PlayOneShot("event:/SFX/Voice/BossComments/BossPhase2");
            Score += scorePerHitPhase2;
            ZenMetreManager.Instance.AddHitZen(zenPerHitPhase2);
            
            if (lFist == "LeftFist") HapticManager.leftZenPunch2 = true;
            else HapticManager.rightZenPunch2 = true;
        }
        
        [ContextMenu("HitBossPhase3")]
        private void Phase3Hit(ControllerManager controllerManager, string fistUsed)
        {
            if (!SpecialAttackScript.punchCharged || _bossIsDead) return;
            Log("Phase 3 hit charged");
            
            var controllerVelocity = fistUsed == "LeftFist" ? controllerManager.leftControllerVelocity : controllerManager.rightControllerVelocity;
            var zenPunchForce = SpecialAttackScript.punchForce * controllerVelocity.normalized;
            var force = controllerVelocity + zenPunchForce;
            force *= zenPunchMultiplier;
            
            _bossIsDead = true;
            // TODO: add force to rigidbody hit // _rigidbody.AddForce(force);
            EventManager.BossDefeated.Invoke();
            
            StartCoroutine(PunchSound());
            
            var totalScore = (int)(force.magnitude + Phase.Sum(pair => pair.Value.score));
            Log($"BossDefeated | TotalScore: {totalScore}");
            EventManager.BossScore.Invoke(totalScore);
            InternalZenEventManager.stopChargeVfx.Invoke();
        }
        
        private IEnumerator PunchSound()
        {
            StartCoroutine(ChargeSfx.PlayChargePunchSfx());
            FMODManager.instance.koiPunchVocals.setParameterByName("koiPunchSoundState", 2);
            yield return new WaitForSeconds(4);
            FMODManager.instance.koiPunchVocals.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        #endregion
        #endregion

        #region ---BossActions---
        public void Punched(ControllerManager controllerManager, string fistUsed)
        {
            switch(CurrentBossState){
                case BossPhase.Phase0:
                    Phase0Hit();
                    break;
                case BossPhase.Phase1:
                    Phase1Hit();
                    break;
                case BossPhase.Phase2:
                    Phase2Hit(fistUsed);
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
        
        public void HitPlayer()
        {
            Log("HitPlayer");
            EventManager.StartBossPhase0.Invoke();
        }

        public void HitGround()
        {
            if (!_canCollideWithGround) return;
            if (_bossIsDead)
            {
                Destroy(gameObject);
                return;
            }
            
            Log("Collision with ground resetting boss to phase0");
            EventManager.StartBossPhase0.Invoke();
        }

        public void HitWater(Vector3 velocity)
        {
            
        }

        public void HitBird()
        {
            
        }
        
        public void PunchedSuccessful()
        {
            
        }
        
        public void PunchedUnsuccessful()
        {
            
        }
        #endregion
    }
}