using _GameData_.Scripts.Entities.Craft;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Zenject;

namespace _GameData_.AI.GOAP.Actions
{
    public class TakeBricksAction : ActionBase<TakeBricksAction.Data>, IInjectable
    {
        [Inject] private ShredderComponent shredderComponent;
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
            if (data.ItemsCollectorComponent.IsFull)
            {
                return ActionRunState.Stop;
            }
            if (shredderComponent.itemStacks.Value.Empty)
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