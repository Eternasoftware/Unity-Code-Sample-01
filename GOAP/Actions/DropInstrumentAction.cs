using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace _GameData_.AI.GOAP.Actions
{
    
    public class DropInstrumentAction : ActionBase<DropInstrumentAction.Data>
    {
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            
            [GetComponent]
            public AIInstrumentsSwitcher AIInstrumentsSwitcher { get; set; }
        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.AIInstrumentsSwitcher == null)
            {
                return ActionRunState.Stop;
            }
            data.AIInstrumentsSwitcher.DropInstrument();
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }
    }
}