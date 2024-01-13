using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InDevelopment.Fish
{
    public class Boss : MonoBehaviour
    {
        #region ---PhaseProperties---
        private enum BossPhase
        {
            Phase0,
            Phase1,
            Phase2,
            Phase3,
            BossDefeated
        }
        
        private static readonly Dictionary<BossPhase, PhaseInfo> Phase = new()
        {
            { BossPhase.Phase0, new PhaseInfo(() => EventManager.StartBossPhase0.Invoke()) },
            { BossPhase.Phase1, new PhaseInfo(() => EventManager.StartBossPhase1.Invoke()) },
            { BossPhase.Phase2, new PhaseInfo(() => EventManager.StartBossPhase2.Invoke()) },
            { BossPhase.Phase3, new PhaseInfo(() => EventManager.StartBossPhase3.Invoke()) },
            { BossPhase.BossDefeated, new PhaseInfo(() => EventManager.BossDefeated.Invoke()) }
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
        
        #region ---PhaseCompletion---
        private void Phase0Completed()
        {
            Phase.FirstOrDefault(keyValuePair => keyValuePair.Value.score == 0).Value.Event.Invoke();
        }
        
        private void Phase1Completed(float score)
        {
            Phase[BossPhase.Phase1].score = score;
            Phase[BossPhase.Phase2].Event.Invoke();
        }

        private void Phase2Completed(float score)
        {
            Phase[BossPhase.Phase2].score = score;
            Phase[BossPhase.Phase3].Event.Invoke();
        }

        private void Phase3Completed(float score)
        {
            Phase[BossPhase.Phase3].score = score;
            Phase[BossPhase.BossDefeated].Event.Invoke();
        }
        #endregion
    }
}