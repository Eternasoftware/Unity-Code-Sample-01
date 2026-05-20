using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace _GameData_.AI.GOAP.Actions
{
    public class SellTrashAction : ActionBase<SellTrashAction.Data>, IInjectable
    {
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
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