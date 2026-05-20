using _GameData_.Scripts.Entities.Decoration;
using _GameData_.Scripts.Entities.Player;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Zenject;

namespace _GameData_.AI.GOAP.Actions
{
    public class TakeMaterialsAction : ActionBase<TakeMaterialsAction.Data>, IInjectable
    {
        [Inject] private DecorationBox decorationBox;
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }

            [GetComponent]
            public BackpackComponent BackpackComponent { get; set; }
        }

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.BackpackComponent.CanReceiveMaterials = true;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.BackpackComponent.IsFull)
            {
                return ActionRunState.Stop;
            }
            if (decorationBox.CanGiveItems())
            {
                return ActionRunState.Continue;
            }
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
            data.BackpackComponent.CanReceiveMaterials = false;
        }
    }
}