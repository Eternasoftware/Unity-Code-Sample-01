using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Items;
using _GameData_.Scripts.Entities.Demolish;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace _GameData_.AI.GOAP.Actions
{
    public class GrableDynamicFurnitureAction : ActionBase<GrableDynamicFurnitureAction.Data>, IInjectable
    {
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public AIObjectGrabber AIObjectGrabber { get; set; }
            public DynamicFurnitureComponent TargetCache { get; set; }
        }
        
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            if (data.Target is not TransformTarget)
                return;
            var transformTarget = data.Target as TransformTarget;
            data.TargetCache = transformTarget.Transform.GetComponent<DynamicFurnitureComponent>();
            if (data.TargetCache == null)
                return;
            if (data.TargetCache.IsAiTarget || !data.TargetCache.IsInteractable) // already someone targetet on this obj
            {
                // note stop
                data.TargetCache = null;
                return;
            }
            data.TargetCache.IsAiTarget = true;
            data.AIObjectGrabber = agent.GetComponent<AIInstrumentsSwitcher>().prevInstrumentGameObject.GetComponent<AIObjectGrabber>();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.TargetCache == null)
            {
                return ActionRunState.Stop;
            }
            if (!data.TargetCache.IsInteractable)
            {
                data.TargetCache.IsAiTarget = false;
                return ActionRunState.Stop;
            }
            if (IsInRange(agent, agent.DistanceObserver.GetDistance(agent, data.Target, null), data, null))
            {
                if (data.AIObjectGrabber.StartGrabObject(data.TargetCache))
                {
                    return ActionRunState.Stop;
                }
            }
            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }
    }
}