using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using GameData.Scripts.Entities.Trash;
using UnityEngine;

namespace _GameData_.AI.GOAP.Actions
{
    public class AttackFloorAction : ActionBase<AttackFloorAction.Data>
    {
        public class Data : IActionData
        {
            public ITarget Target { get; set; }

            public TrashComponent targetCache;
            
            public AIScrapperComponent<TrashComponent> Scrapper;
            
            public float Time { get; set; }
        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            if (data.Target == null)
                return;
            if (data.Target is not TransformTarget)
                return;
            var transformTarget = data.Target as TransformTarget;
            data.targetCache = transformTarget.Transform.GetComponent<TrashComponent>();
            if (data.targetCache == null)
                return;
            data.targetCache.IsAITarget = true;
            
            data.Scrapper = agent.GetComponent<AIInstrumentsSwitcher>().prevInstrumentGameObject.GetComponent<AIScrapperComponent<TrashComponent>>();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.Target == null)
                return ActionRunState.Stop;;
            if (data.targetCache == null)
                return ActionRunState.Stop;
            if (data.targetCache.IsDestroy)
                return ActionRunState.Stop;
            if (data.Scrapper == null)
                return ActionRunState.Stop;
            if (IsInRange(agent, agent.DistanceObserver.GetDistance(agent, data.Target, null), data, null))
            {
                data.Time += context.DeltaTime;
                if (data.Time >= 1f)
                {
                    data.Scrapper.Hit(data.targetCache);
                    return ActionRunState.Stop;
                }
            }
            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, Data data)
        {
            if (data.targetCache == null)
                return;
            data.targetCache.IsAITarget = false;
        }
    }
}