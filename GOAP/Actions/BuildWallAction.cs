using _GameData_.Scripts.Entities.Decoration.Interfaces;
using _GameData_.Scripts.Entities.Player;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace _GameData_.AI.GOAP.Actions
{
    public class BuildAction : ActionBase<BuildAction.Data>
    {
        public class Data : IActionData
        {
            public IItemAIReceiver TargetCache;
            public ITarget Target { get; set; }

            [GetComponent]
            public BackpackComponent BackpackComponent { get; set; }
        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            if (data.Target is not TransformTarget)
                return;
            var transformTarget = data.Target as TransformTarget;
            data.TargetCache = transformTarget.Transform.GetComponent<IItemAIReceiver>();
            if (data.TargetCache == null)
                return;
            data.TargetCache.IsAiTarget = true;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.TargetCache == null)
            {
                return ActionRunState.Stop;
            }
            if (data.BackpackComponent.CurrentCapacity == 0)
            {
                return ActionRunState.Stop;
            }
            if (!data.TargetCache.CanReceive)
            {
                return ActionRunState.Stop;
            }
            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, Data data)
        {
            if (data.TargetCache == null)
                return;
            data.TargetCache.IsAiTarget = false;
        }
    }
}