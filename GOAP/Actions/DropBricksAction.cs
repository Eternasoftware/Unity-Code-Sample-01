using _GameData_.Scripts.Entities.Craft;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace _GameData_.AI.GOAP.Actions
{
    public class DropBricksAction : ActionBase<DropBricksAction.Data>
    {
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            
            [GetComponent]
            public ItemsCollectorComponent ItemsCollectorComponent { get; set; }
        }

        public override void Created()
        {
            
        }

        public override void Start(IMonoAgent agent, Data data)
        {
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }
    }
}