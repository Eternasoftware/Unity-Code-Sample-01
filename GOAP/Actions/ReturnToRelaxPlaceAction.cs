using _GameData_.Scripts.Entities.AI.GOAP.Behavior.Interfaces;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace _GameData_.AI.GOAP.Actions
{
    public class ReturnToRelaxPlaceAction : ActionBase<ReturnToRelaxPlaceAction.Data>
    {
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            
            public float Timer { get; set; }
            
            [GetComponent]
            public IRelaxSourcer RelaxSourcer { get; set; }
        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Timer = data.RelaxSourcer.TimeToRelax;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;
            if (data.Timer <= 0)
            {
                return ActionRunState.Stop;
            }
            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }
    }
}