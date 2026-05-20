using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items;
using _GameData_.Scripts.Entities.BushCutting;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Pathfinding;

namespace _GameData_.AI.GOAP.Actions
{
    public class CuttingBushAction : ActionBase<CuttingBushAction.Data>
    {
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            
            public AIBushChainsawComponent InstrumentComponent { get; set; }

            public BushSurfaceComponent TargetCache;
        }

        public override void Created()
        {
            
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            if (data.Target is not TransformTarget)
                return;
            var transformTarget = data.Target as TransformTarget;
            data.TargetCache = transformTarget.Transform.GetComponent<BushSurfaceComponent>();
            if (data.TargetCache == null)
                return;
            data.TargetCache.IsAITarget = true;
            data.InstrumentComponent = agent.GetComponent<AIInstrumentsSwitcher>().prevInstrumentGameObject.GetComponent<AIBushChainsawComponent>();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.InstrumentComponent == null || data.TargetCache == null || data.Target == null)
            {
                return ActionRunState.Stop;
            }

            if (!data.InstrumentComponent.Interactable && data.TargetCache.IsActive)
            {
                return ActionRunState.Stop;
            }
            
            if (!data.InstrumentComponent.Interactable && !data.TargetCache.IsComplete)
            {
                data.TargetCache.StartClearingLoop();
                data.InstrumentComponent.StartInteract(data.TargetCache.transform);
                return ActionRunState.Continue;
            }
            
            if (data.TargetCache.IsComplete)
            {
                return ActionRunState.Stop;
            }

            if (data.InstrumentComponent.Interactable)
            {
                return ActionRunState.Continue;
            }

            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
            if (data.TargetCache != null && data.TargetCache.IsAITarget)
            {
                data.TargetCache.EndClearingLoop();
                data.TargetCache.IsAITarget = false;
            }
            if (data.InstrumentComponent != null)
            {
                data.InstrumentComponent.EndInteract();
            }
        }
    }
}